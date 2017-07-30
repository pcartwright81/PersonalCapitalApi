using CredentialManagement;
using PersonalCapital.Api;
using PersonalCapital.Exceptions;
using System;
using System.IO;
using System.Linq;
using Newtonsoft.Json;
using PersonalCapital.Request;
using TestApplication.Extensions;

namespace TestApplication {
    internal class Program {
        private static void Main(string[] args) {
            // Get username from settings
            var username = Settings.Username;
            if (username == null) {
                Console.WriteLine("Set the username in the appSettings section of the app.config");
                Pause();
                return;
            }

            // Get password from vault
            var password = GetPasswordFromVault(Settings.KeystoreTarget, username);
            if (password == null) {
                // Get password from the console
                Console.Write("Password: ");
                password = Console.ReadLine();
                Console.Clear();
            }

            Console.WriteLine("Attempting log in");

            using (var pcClient = new PersonalCapitalClient()) {
                try {
                    // Restore session from the file configured if it exists
                    if (Settings.SessionFile != null && File.Exists(Settings.SessionFile)) {
                        Console.WriteLine($"Restoring session from {Settings.SessionFile}");
                        pcClient.RestoreSession(Settings.SessionFile);
                    }
                    // Attempt to login; successful if no exceptions are thrown
                    pcClient.Login(username, password).GetAwaiter().GetResult();
                    Console.WriteLine("Logged in Successfully");
                }
                catch (RequireTwoFactorException) {
                    pcClient.SendTwoFactorChallenge(TwoFactorVerificationMode.SMS).GetAwaiter().GetResult();
                    string code;
                    do {
                        Console.Write("Code: ");
                        code = Console.ReadLine();
                        if (string.IsNullOrEmpty(code)) {
                            break;
                        }
                        var result = pcClient.TwoFactorAuthenticate(TwoFactorVerificationMode.SMS, code).GetAwaiter().GetResult();
                        if (result.Header.AuthLevel == Constants.AuthLevel.DeviceAuthorized) {
                            break;
                        }
                    } while (!string.IsNullOrEmpty(code));
                    if (!string.IsNullOrEmpty(code)) {
                        // Challenge passed successfully
                        var result = pcClient.AuthenticatePassword(password).GetAwaiter().GetResult();
                        if (result.Header.AuthLevel == Constants.AuthLevel.SessionAuthenticated) {
                            Console.WriteLine("Logged in Successfully");
                            if (Settings.SessionFile != null) {
                                pcClient.PersistSession(Settings.SessionFile);
                            }
                        }
                    }
                    else {
                        Console.WriteLine("Two factor authentication failed. Unable to log in.");
                        Pause();
                        return;
                    }
                }
                catch (PersonalCapitalException e) {
                    Console.Write($"{e.GetType().Name}: ");
                    Console.WriteLine(e.Message);
                    Pause();
                    return;
                }

                var accountResponse = pcClient.FetchAccounts().GetAwaiter().GetResult();
                Console.WriteLine($"Net Worth: {accountResponse.Data.Networth}");
                foreach (var group in accountResponse.Data.Accounts.GroupBy(x=>x.ProductType)) {
                    Console.WriteLine(group.Key?.Replace('_',' ') + ":");
                    foreach (var account in group) {
                        Console.WriteLine($"    {account.UserAccountId} -> {account.Name}: ${account.Balance}");
                    }
                }
                //Console.WriteLine();
                //var transactionsResponse = pcClient.FetchUserTransactions(new FetchUserTransactionsRequest(DateTime.Today.AddDays(-7), DateTime.Today)).GetAwaiter().GetResult();
                //Console.WriteLine($"Net Cash Flow: {transactionsResponse.Data.NetCashflow}");
                //Console.WriteLine($"Date Range: {transactionsResponse.Data.StartDate} - {transactionsResponse.Data.EndDate}");
                //foreach (var tx in transactionsResponse.Data.Transactions.Where(x=>x.IsSpending)) {
                //    Console.WriteLine($"{tx.SimpleDescription.WhenNullOrEmpty(tx.TransactionType)} : ${tx.Amount}");
                //}
            }
            Pause();
        }

        /// <summary>
        /// Implement the pause console command
        /// </summary>
        private static void Pause() {
            Console.WriteLine();
            Console.Write("Press any key to continue...");
            Console.ReadKey(true);
        }

        /// <summary>
        /// Gets the password from the credential vault for the associated username and target
        /// </summary>
        /// <param name="target"></param>
        /// <param name="username"></param>
        /// <returns></returns>
        private static string GetPasswordFromVault(string target, string username) {
            using (var cred = new Credential()) {
                cred.Target = target;
                cred.Load();
                return cred.Username == username ? cred.Password : null;
            }
        }
    }
}
