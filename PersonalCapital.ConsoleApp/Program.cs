using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using PersonalCapital.Api;
using PersonalCapital.ConsoleApp.Extensions;
using PersonalCapital.Exceptions;
using PersonalCapital.Request;

const string sessionFile = "PersonalCapitalSession.json";

IHost host = InitializeApp(args);

var logger = host.Services.GetRequiredService<ILogger<Program>>();
var config = host.Services.GetRequiredService<IConfiguration>();
var usernamePassword = InitializeUsernamePassword();

if(usernamePassword.username == null || usernamePassword.password == null)
{
    return;
}

logger.LogInformation("Attempting log in for {Username}", usernamePassword.username);

using var pcClient = host.Services.GetRequiredService<PersonalCapitalClient>();

var loggedIn = await HandleLoginAsync(pcClient, usernamePassword.username, usernamePassword.password);

if (!loggedIn)
{
    logger.LogError("Login failed. Please check credentials and 2FA code if prompted.");
    Pause();
    return;
}

logger.LogInformation("Logged in Successfully");

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

async Task<bool> HandleLoginAsync(PersonalCapitalClient client, string user, string pass)
{
    try 
    {
        // Restore session from the file if it exists
        if (File.Exists(sessionFile))
        {
            logger.LogInformation("Restoring session from {File}", sessionFile);
            client.RestoreSession(sessionFile);
        }

        // Attempt to log in; successful if no exceptions are thrown
        await client.Login(user, pass);
        client.PersistSession(sessionFile);
        return true;
    }
    catch (RequireTwoFactorException)
    {
        logger.LogInformation("Two-factor authentication is required.");
        await client.SendTwoFactorChallenge(TwoFactorVerificationMode.SMS);

        Console.Write("Code: ");
        var code = Console.ReadLine();
        if (string.IsNullOrEmpty(code))
        {
            logger.LogError("Two-factor code was not provided.");
            return false;
        }

        var twoFaResult = await client.TwoFactorAuthenticate(TwoFactorVerificationMode.SMS, code);
        if (twoFaResult.Header.AuthLevel != PersonalCapital.Api.Constants.AuthLevel.DeviceAuthorized)
        {
            logger.LogError("Invalid two-factor code.");
            return false;
        }

        // Challenge passed successfully, now authenticate the session
        var authResult = await client.Login(user, pass);
        if (authResult.Success)
        {
            client.PersistSession(sessionFile);
            return true;
        }

        return false;
    }
    catch (PersonalCapitalException e)
    {
        logger.LogError(e, "An error occurred during login: {Message}", e.Message);
        return false;
    }
}



void LogOutputs(IEnumerable<string> messages)
{
    var output = string.Join(Environment.NewLine, messages.Where(x => !string.IsNullOrWhiteSpace(x)));
    if (!string.IsNullOrEmpty(output))
    {
        logger.LogInformation("{Output}", Environment.NewLine + output);
    }
}

/// <summary>
/// Pauses the console by waiting for a key press.
/// </summary>
void Pause()
{
    Console.Write("Press any key to continue...");
    Console.ReadKey(true);
}

static IHost InitializeApp(string[] args)
{
    return Host.CreateDefaultBuilder(args)
        .ConfigureLogging(logging =>
        {
            logging.ClearProviders();
            logging.AddConsole();
        })
        .ConfigureServices((_, services) =>
        {
            services.AddSingleton<PersonalCapitalClient>();
        })
        .Build();
}

(string? username, string? password) InitializeUsernamePassword()
{
    // Get credentials from User Secrets or environment variables via IConfiguration
    var username = config["PEW_EMAIL"];
    var password = config["PEW_PASSWORD"];


    if (string.IsNullOrWhiteSpace(username) || string.IsNullOrWhiteSpace(password))
    {
        if (string.IsNullOrWhiteSpace(username))
        {
            logger.LogError("Username 'PEW_EMAIL' is not set. Use 'dotnet user-secrets set \"PEW_EMAIL\" \"your-email\"' to configure it.");
        }
        if (string.IsNullOrWhiteSpace(password))
        {
            logger.LogError("Password 'PEW_PASSWORD' is not set. Use 'dotnet user-secrets set \"PEW_PASSWORD\" \"your-password\"' to configure it.");
        }
        Pause();
    }
    return (username, password);
}