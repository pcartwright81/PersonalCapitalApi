namespace TestApplication
{
    using Microsoft.Extensions.DependencyInjection;
    using Microsoft.Extensions.Hosting;
    using Microsoft.Extensions.Logging;
    using PersonalCapital.Api;
    using PersonalCapital.Exceptions;
    using PersonalCapital.Request;
    using PersonalCapital.ConsoleApp.Extensions;

    internal class Program
    {
        private static void Initialize()
        {
            var host = Host.CreateDefaultBuilder()
                .ConfigureAppConfiguration(
                    (hostingContext, config) => { })
                .ConfigureServices(
                    (ctx, services) =>
                    {
                        services.AddLogging(
                            opt =>
                            {
                                opt.AddSimpleConsole(
                                    c =>
                                    {
                                        c.TimestampFormat = "[HH:mm:ss] ";
                                    });
                            });
                    })
                .Build();
            using var serviceScope = host.Services.CreateScope();
            var provider = serviceScope.ServiceProvider;
            logger = provider.GetRequiredService<ILogger<Program>>();
        }

        private static async Task Main()
        {
            Initialize();
            var file = "PersonalCapitalSession_new.bin";
            // Get username from settings
            var username = Environment.GetEnvironmentVariable("PEW_EMAIL");
            if (username == null)
            {
                logger.LogError("Set the username in the appSettings section of the app.config");
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

            logger.LogInformation("Attempting log in");

            using (var pcClient = new PersonalCapitalClient())
            {
                try
                {
                    // Restore session from the file configured if it exists
                    if (File.Exists(file))
                    {
                        logger.LogInformation($"Restoring session from {file}");
                        pcClient.RestoreSession(file);
                    }
                    // Attempt to login; successful if no exceptions are thrown
                    await pcClient.Login(username, password);
                    logger.LogInformation("Logged in Successfully");
                }
                catch (RequireTwoFactorException)
                {
                    await pcClient.SendTwoFactorChallenge(TwoFactorVerificationMode.SMS);
                    string code;
                    do
                    {
                        Console.Write("Code: ");
                        code = Console.ReadLine();
                        if (string.IsNullOrEmpty(code))
                        {
                            break;
                        }
                        var result = await pcClient.TwoFactorAuthenticate(TwoFactorVerificationMode.SMS, code);
                        if (result.Header.AuthLevel == Constants.AuthLevel.DeviceAuthorized)
                        {
                            break;
                        }
                    }
                    while (!string.IsNullOrEmpty(code));
                    if (!string.IsNullOrEmpty(code))
                    {
                        // Challenge passed successfully
                        var result = await pcClient.AuthenticatePassword(password);
                        if (result.Header.AuthLevel == Constants.AuthLevel.SessionAuthenticated)
                        {
                            logger.LogInformation("Logged in Successfully");
                            pcClient.PersistSession(file);
                        }
                    }
                    else
                    {
                        logger.LogError("Two factor authentication failed. Unable to log in.");
                        Pause();
                        return;
                    }
                }
                catch (PersonalCapitalException e)
                {
                    logger.LogError(e.Message);
                    Pause();
                    return;
                }

                var bills = await pcClient.FetchBills();
                var usermessage = await pcClient.FetchUserMessages();
                var outputs = usermessage.Data.UserMessages.Select(c => c.Summary).ToList();
                LogOutputs(outputs);
                var accountResponse = await pcClient.FetchAccounts();
                outputs = new List<string>
                {
                    $"Net Worth: {accountResponse.Data.Networth}"
                };
                foreach (var group in accountResponse.Data.Accounts.GroupBy(x => x.ProductType))
                {
                    outputs.Add((group.Key?.Replace('_', ' ')) + ":");
                    foreach (var account in group)
                    {
                        outputs.Add($"{account.UserAccountId} -> {account.Name}: ${account.Balance}");
                    }
                }
                LogOutputs(outputs);
                var transactionsResponse = await pcClient.FetchUserTransactions(
                    new FetchUserTransactionsRequest(DateTime.Today.AddDays(-7), DateTime.Today));
                outputs = new List<string>
                {
                    $"Net Cash Flow: {transactionsResponse.Data.NetCashflow}",
                    $"Date Range: {transactionsResponse.Data.StartDate} - {transactionsResponse.Data.EndDate}"
                };

                foreach (var tx in transactionsResponse.Data.Transactions.Where(x => x.IsSpending))
                {
                    outputs.Add($"{tx.SimpleDescription.WhenNullOrEmpty(tx.TransactionType)} : ${tx.Amount}");
                }
                LogOutputs(outputs);
            }
            Pause();
        }

        private static void LogOutputs(List<string> messages)
        {
            logger.LogInformation(string.Join(Environment.NewLine, messages.Where(x => !string.IsNullOrWhiteSpace(x))));
        }

        /// <summary>
        /// Implement the pause console command
        /// </summary>
        private static void Pause()
        {
            Console.Write("Press any key to continue...");
            Console.ReadKey(true);
        }
        private static ILogger<Program> logger;
    }
}
