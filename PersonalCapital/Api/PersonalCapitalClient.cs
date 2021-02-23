using System;
using System.Diagnostics.CodeAnalysis;
using System.IO;
using System.Net;
using System.Net.Http;
using System.Runtime.Serialization.Formatters.Binary;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using Newtonsoft.Json.Linq;
using PersonalCapital.Exceptions;
using PersonalCapital.Extensions;
using PersonalCapital.Request;
using PersonalCapital.Response;

namespace PersonalCapital.Api
{
    [SuppressMessage("ReSharper", "UnusedMember.Global")]
    public class PersonalCapitalClient :  IDisposable
    {
        private const string BaseUrl = "https://home.personalcapital.com/";
        private const string BaseApiUrl = BaseUrl + "api/";

        private readonly HttpClient _client;
        private readonly HttpClientHandler _clientHandler;
        private readonly Regex _csrfRegex = new Regex("globals.csrf='([a-f0-9-]+)'", RegexOptions.Compiled);

        public PersonalCapitalClient()
        {
            _clientHandler = new HttpClientHandler {UseCookies = true, CookieContainer = new CookieContainer()};
            _client = new HttpClient(_clientHandler) {BaseAddress = new Uri(BaseApiUrl)};
        }

        public CookieContainer CookieContainer
        {
            get => _clientHandler.CookieContainer;
            set
            {
                if (_clientHandler.CookieContainer != value) _clientHandler.CookieContainer = value;
            }
        }

        public string Csrf { get; protected set; }

        public void Dispose()
        {
            _client?.Dispose();
            _clientHandler?.Dispose();
        }

        public void PersistSession(string filename)
        {
            using Stream stream = File.Open(filename, FileMode.Create);
            var binaryFormatter = new BinaryFormatter();
            binaryFormatter.Serialize(stream, CookieContainer);
        }

        public void RestoreSession(string filename)
        {
            using Stream stream = File.Open(filename, FileMode.Open);
            var binaryFormatter = new BinaryFormatter();
            CookieContainer = (CookieContainer) binaryFormatter.Deserialize(stream);
        }

        public async Task<IdentifyUserResponse> Login(string username, string password,
            CookieContainer sessionCookies = null)
        {
            if (sessionCookies != null) CookieContainer = sessionCookies;

            var initialCsrf = await GetCsrfFromHomepage();
            var userResponse = await IdentifyUser(username, initialCsrf);

            if (string.IsNullOrEmpty(userResponse?.Header?.Csrf) || string.IsNullOrEmpty(userResponse.Header?.AuthLevel)
            ) throw new Exception("Unable to identify user");
            ParseHeaderForErrors(userResponse.Header);

            if (userResponse.Header.AuthLevel != Constants.AuthLevel.UserRemembered)
                throw new RequireTwoFactorException();
            var authenticationResponse = await AuthenticatePassword(password);
            ParseHeaderForErrors(authenticationResponse.Header);
            return authenticationResponse;
            // If we got here, the user is valid, but isn't remembered by PersonalCapital
            // therefore, the client will need to complete two-factor authentication
        }

        public async Task<string> GetCsrfFromHomepage()
        {
            var httpMessage = await _client.GetAsync(BaseUrl);
            var result = await httpMessage.Content.ReadAsStringAsync();
            try
            {
                return _csrfRegex.Match(result).Groups[1].Captures[0].Value;
            }
            catch (ArgumentOutOfRangeException)
            {
                return null;
            }
        }

        public async Task<IdentifyUserResponse> IdentifyUser(string username, string initialCsrf)
        {
            var data = new IdentifyUserRequest
            {
                Username = username,
                Csrf = initialCsrf
            };

            var httpMessage = await _client.PostHttpEncodedData("login/identifyUser", data);
            var response = await httpMessage.Content.ReadAsAsync<IdentifyUserResponse>();
            if (!string.IsNullOrEmpty(response?.Header?.Csrf)) Csrf = response.Header.Csrf;
            return response;
        }

        public async Task<HeaderOnlyResponse> SendTwoFactorChallenge(TwoFactorVerificationMode mode)
        {
            if (string.IsNullOrEmpty(Csrf))
                throw new Exception("CSRF not set; Identify user before sending two factor challenge");

            string challengeType;
            string method;

            switch (mode)
            {
                case TwoFactorVerificationMode.SMS:
                    challengeType = "challengeSMS";
                    method = "challengeSms";
                    break;
                case TwoFactorVerificationMode.EMail:
                    method = challengeType = "challengeEmail";
                    break;
                default:
                    throw new ArgumentOutOfRangeException(nameof(mode), mode, null);
            }

            var data = new TwoFactorChallengeRequest
            {
                ChallengeReason = "DEVICE_AUTH",
                ChallengeMethod = "OP",
                ChallengeType = challengeType,
                Csrf = Csrf
            };

            var httpMessage = await _client.PostHttpEncodedData($"credential/{method}", data);
            return await httpMessage.Content.ReadAsAsync<HeaderOnlyResponse>();
        }

        public async Task<HeaderOnlyResponse> TwoFactorAuthenticate(TwoFactorVerificationMode mode, string code)
        {
            if (string.IsNullOrEmpty(Csrf))
                throw new Exception("CSRF not set; Identify user before authenticating two factor challenge");

            var method = mode switch
            {
                TwoFactorVerificationMode.SMS => "authenticateSms",
                TwoFactorVerificationMode.EMail => "authenticateEmail",
                _ => throw new ArgumentOutOfRangeException(nameof(mode), mode, null)
            };

            var data = new TwoFactorAuthenticationRequest
            {
                Code = int.Parse(code),
                ChallengeReason = "DEVICE_AUTH",
                ChallengeMethod = "OP",
                Csrf = Csrf
            };

            var httpMessage = await _client.PostHttpEncodedData($"credential/{method}", data);
            return await httpMessage.Content.ReadAsAsync<HeaderOnlyResponse>();
        }

        public async Task<IdentifyUserResponse> AuthenticatePassword(string password)
        {
            var data = new AuthenticatePasswordRequest
            {
                Csrf = Csrf,
                Password = password,
                BindDevice = "true",
                DeviceName = ""
            };

            var httpMessage = await _client.PostHttpEncodedData("credential/authenticatePassword", data);
            return await httpMessage.Content.ReadAsAsync<IdentifyUserResponse>();
        }

        public async Task<T> Fetch<T>(string url, object data = null)
        {
            if (string.IsNullOrEmpty(Csrf)) throw new Exception("CSRF not set; Log in before calling Fetch");

            var payload = (data ?? new { }).ToDynamic();
            // Set required values
            payload.lastServerChangeId = "-1";
            payload.csrf = Csrf;
            payload.apiClient = "WEB";
            // Send data as HTTP Encoded
            var httpMessage = await _client.PostHttpEncodedData(url, (object) payload);
            // Parse into type expected
            return await httpMessage.Content.ReadAsAsync<T>();
        }

        public async Task<dynamic> Fetch(string url, object data = null)
        {
            if (string.IsNullOrEmpty(Csrf)) throw new Exception("CSRF not set; Log in before calling Fetch");

            var payload = (data ?? new { }).ToDynamic();
            // Set required values
            payload.lastServerChangeId = "-1";
            payload.csrf = Csrf;
            payload.apiClient = "WEB";
            // Send data as HTTP Encoded
            var httpMessage = await _client.PostHttpEncodedData(url, (object) payload);
            // Return raw JSON as a dynamic object
            return JObject.Parse(await httpMessage.Content.ReadAsStringAsync());
        }

        private static void ParseHeaderForErrors(HeaderResponse header)
        {
            if (header == null) return;
            // Check that the user is active
            if (header.Status != Constants.Status.Active) throw new UnknownUserException();
            // Check for any errors that can be handled
            if (header.Errors == null) return;
            foreach (var error in header.Errors)
                switch (error.Code)
                {
                    case 312: // Incorrect password entered
                        throw new IncorrectPasswordException(error.Message);
                }
        }

        #region Mapped Fetch/Get Api Methods

        public Task<FetchAccountsResponse> FetchAccounts(object data = null)
        {
            return Fetch<FetchAccountsResponse>(@"newaccount\getAccounts2", data);
        }

        public Task<FetchCategoriesResponse> FetchCategories(object data = null)
        {
            return Fetch<FetchCategoriesResponse>(@"transactioncategory\getCategories", data);
        }

        public Task<FetchUserMessagesResponse> FetchUserMessages(object data = null)
        {
            return Fetch<FetchUserMessagesResponse>(@"message/getUserMessages", data);
        }

        public Task<FetchPersonResponse> FetchPerson(object data = null)
        {
            return Fetch<FetchPersonResponse>(@"person/getPerson", data);
        }

        public Task<FetchUserTransactionsResponse> FetchUserTransactions(FetchUserTransactionsRequest data)
        {
            return Fetch<FetchUserTransactionsResponse>(@"transaction/getUserTransactions", data);
        }

        public Task<UpdateTransactionResponse> UpdateTransaction(UpdateTransactionRequest data)
        {
            return Fetch<UpdateTransactionResponse>(@"transaction/updateUserTransactions2", data);
        }

        #endregion
    }
}