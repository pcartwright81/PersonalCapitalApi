using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using PersonalCapital.Api;
using PersonalCapital.ConsoleApp.Extensions;
using PersonalCapital.Request;

IHost host = InitializeApp(args);

var logger = host.Services.GetRequiredService<ILogger<Program>>();
var config = host.Services.GetRequiredService<IConfiguration>();
using var pcClient = host.Services.GetRequiredService<PersonalCapitalClient>();
var (username, password) = InitializeUsernamePassword();

if (username == null || password == null)
{
    return;
}

if (logger.IsEnabled(LogLevel.Information))
{
    logger.LogInformation("Attempting log in for {Username}", username);
}

var authenticated = await pcClient.AuthenticateAsync(
    username,
    password,
    twoFactorCodeCallback: async () =>
    {
        logger.LogInformation("2FA code sent. Please enter the code:");
        Console.Write("Enter 2FA code: ");
        return await Task.FromResult(Console.ReadLine() ?? string.Empty);
    },
    mode: TwoFactorVerificationMode.SMS
);

if (!authenticated)
{
    logger.LogError("Authentication failed. Please check credentials and 2FA code.");
    Pause();
    return;
}

logger.LogInformation("Authentication successful!");

var bills = await pcClient.FetchBills();

var messages = await pcClient.FetchUserMessages();
var messageSummaries = messages.Data.UserMessages.Select(c => c.Summary);
LogOutputs(messageSummaries);

var accountResponse = await pcClient.FetchAccounts();
var accountOutputs = new List<string> { $"Net Worth: {accountResponse.Data.Networth:C}" };
foreach (var group in accountResponse.Data.Accounts.GroupBy(x => x.ProductType))
{
    accountOutputs.Add(group.Key?.Replace('_', ' ') + ":");
    accountOutputs.AddRange(group.Select(account =>
        $"{account.UserAccountId} -> {account.Name}: {account.Balance:C}"));
}
LogOutputs(accountOutputs);

var transactionsResponse = await pcClient.FetchUserTransactions(
    new FetchUserTransactionsRequest(DateTime.Today.AddDays(-7), DateTime.Today));
var transactionOutputs = new List<string>
{
    $"Net Cash Flow: {transactionsResponse.Data.NetCashflow:C}",
    $"Date Range: {transactionsResponse.Data.StartDate} - {transactionsResponse.Data.EndDate}"
};
transactionOutputs.AddRange(transactionsResponse.Data.Transactions.Where(x => x.IsSpending).Select(tx =>
    $"{tx.SimpleDescription.WhenNullOrEmpty(tx.TransactionType)} : {tx.Amount:C}"));
LogOutputs(transactionOutputs);

void LogOutputs(IEnumerable<string> messages)
{
    if (logger.IsEnabled(LogLevel.Information))
    {
        var output = string.Join(Environment.NewLine, messages.Where(x => !string.IsNullOrWhiteSpace(x)));
        if (!string.IsNullOrEmpty(output))
        {
            logger.LogInformation("{Output}", Environment.NewLine + output);
        }
    }
}

void Pause()
{
    Console.Write("Press any key to continue...");
    Console.ReadKey(true);
}

static IHost InitializeApp(string[] args)
{
    return Host.CreateDefaultBuilder(args)
         .ConfigureAppConfiguration((context, config) =>
         {
             config.SetBasePath(Directory.GetCurrentDirectory())
                   .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true)
                   .AddEnvironmentVariables();
         })
        .ConfigureLogging(logging =>
        {
            logging.ClearProviders();
            logging.AddConsole();
        })
        .ConfigureServices((_, services) =>
        {
            services.AddSingleton(provider =>
            {
                var config = provider.GetRequiredService<IConfiguration>();
                var sessionFilePath = config.GetValue<string>("PersonalCapital:SessionFilePath")!;
                var useProxy = config.GetValue<bool>("PersonalCapital:UseProxy"); 
                return new PersonalCapitalClient(sessionFilePath, useProxy);
            });
        })
        .Build();
}

(string? username, string? password) InitializeUsernamePassword()
{
    var username = config["PEW_EMAIL"];
    var password = config["PEW_PASSWORD"];

    if (string.IsNullOrWhiteSpace(username) || string.IsNullOrWhiteSpace(password))
    {
        if (string.IsNullOrWhiteSpace(username))
        {
            logger.LogError("Username 'PEW_EMAIL' is not set. Use 'dotnet user-secrets set \"PEW_EMAIL\" \"your-email\"' to configure it");
        }
        if (string.IsNullOrWhiteSpace(password))
        {
            logger.LogError("Password 'PEW_PASSWORD' is not set. Use 'dotnet user-secrets set \"PEW_PASSWORD\" \"your-password\"' to configure it");
        }
        Pause();
    }
    return (username, password);
}
