using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using PersonalCapital.Api;
using PersonalCapital.ConsoleApp.Extensions;
using PersonalCapital.Exceptions;
using PersonalCapital.Request;

namespace PersonalCapital.ConsoleApp;

internal class Program
{
    private static ILogger<Program> _logger = null!;

    private static void Initialize()
    {
        var host = Host.CreateDefaultBuilder()
            .ConfigureAppConfiguration((_, _) => { })
            .ConfigureServices((_, services) =>
            {
                services.AddLogging(opt => { opt.AddSimpleConsole(c => { c.TimestampFormat = "[HH:mm:ss] "; }); });
            })
            .Build();
        using var serviceScope = host.Services.CreateScope();
        var provider = serviceScope.ServiceProvider;
        _logger = provider.GetRequiredService<ILogger<Program>>();
    }

    private static async Task Main()
    {
        Initialize();
        const string file = "PersonalCapitalSession_new.bin";
        // Get username from settings
        var username = Environment.GetEnvironmentVariable("PEW_EMAIL");
        if (username == null)
        {
            _logger.LogError("Set the username in the appSettings section of the app.config");
            Pause();
            return;
        }

        // Get password from vault
        var password = Environment.GetEnvironmentVariable("PEW_PASSWORD");
        if (password == null)
        {
            // Get password from the console
            Console.Write("Password: ");
            password = Console.ReadLine();
            Console.Clear();
        }

        _logger.LogInformation("Attempting log in");

        using (var pcClient = new PersonalCapitalClient())
        {
            try
            {
                // Restore session from the file configured if it exists
                if (File.Exists(file))
                {
                    _logger.LogInformation("Restoring session from {File}", file);
                    pcClient.RestoreSession(file);
                }

                // Attempt to log in; successful if no exceptions are thrown
                await pcClient.Login(username, password);
                _logger.LogInformation("Logged in Successfully");
            }
            catch (RequireTwoFactorException)
            {
                await pcClient.SendTwoFactorChallenge(TwoFactorVerificationMode.SMS);
                string? code;
                do
                {
                    Console.Write("Code: ");
                    code = Console.ReadLine();
                    if (string.IsNullOrEmpty(code)) break;
                    var result = await pcClient.TwoFactorAuthenticate(TwoFactorVerificationMode.SMS, code);
                    if (result.Header.AuthLevel == Constants.AuthLevel.DeviceAuthorized) break;
                } while (!string.IsNullOrEmpty(code));

                if (!string.IsNullOrEmpty(code))
                {
                    // Challenge passed successfully
                    var result = await pcClient.AuthenticatePassword(username, password);
                    if (result.Header.AuthLevel == Constants.AuthLevel.SessionAuthenticated)
                    {
                        _logger.LogInformation("Logged in Successfully");
                        pcClient.PersistSession(file);
                    }
                }
                else
                {
                    _logger.LogError("Two factor authentication failed. Unable to log in.");
                    Pause();
                    return;
                }
            }
            catch (PersonalCapitalException e)
            {
                _logger.LogError(e.Message);
                Pause();
                return;
            }

            await pcClient.FetchBills();
            var messages = await pcClient.FetchUserMessages();
            var outputs = messages.Data.UserMessages.Select(c => c.Summary).ToList();
            LogOutputs(outputs);
            var accountResponse = await pcClient.FetchAccounts();
            outputs = [$"Net Worth: {accountResponse.Data.Networth}"];
            foreach (var group in accountResponse.Data.Accounts.GroupBy(x => x.ProductType))
            {
                outputs.Add(group.Key?.Replace('_', ' ') + ":");
                outputs.AddRange(group.Select(account =>
                    $"{account.UserAccountId} -> {account.Name}: ${account.Balance}"));
            }

            LogOutputs(outputs);
            var transactionsResponse = await pcClient.FetchUserTransactions(
                new FetchUserTransactionsRequest(DateTime.Today.AddDays(-7), DateTime.Today));
            outputs =
            [
                $"Net Cash Flow: {transactionsResponse.Data.NetCashflow}",
                $"Date Range: {transactionsResponse.Data.StartDate} - {transactionsResponse.Data.EndDate}"
            ];
            outputs.AddRange(transactionsResponse.Data.Transactions.Where(x => x.IsSpending).Select(tx =>
                $"{tx.SimpleDescription.WhenNullOrEmpty(tx.TransactionType)} : ${tx.Amount}"));
            LogOutputs(outputs);
        }

        Pause();
    }

    private static void LogOutputs(List<string> messages)
    {
        _logger.LogInformation(string.Join(Environment.NewLine, messages.Where(x => !string.IsNullOrWhiteSpace(x))));
    }

    /// <summary>
    ///     Implement the pause console command
    /// </summary>
    private static void Pause()
    {
        Console.Write("Press any key to continue...");
        Console.ReadKey(true);
    }
}