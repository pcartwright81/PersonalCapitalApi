using PersonalCapital.Extensions;
using PersonalCapital.Request;
using PersonalCapital.Response;
using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;

namespace PersonalCapital.Api;

public class PersonalCapitalClient : IDisposable
{
    private const string BaseUrl = "https://pc-api.empower-retirement.com/";
    private const string ParticipantBaseUrl = "https://ira.empower-retirement.com/";
    private const string BaseApiUrl = BaseUrl + "api/";
    private readonly CookieContainer _cookieContainer = new();
    private readonly HttpClient _client;
    private readonly HttpClient _participantClient;
    private readonly HttpClientHandler _clientHandler;
    private readonly PersonalCapitalSessionManager _sessionManager;
    private readonly PersonalCapitalAuthenticator _authenticator;
    private readonly string _sessionFilePath;

    public PersonalCapitalClient(string sessionFilePath, bool useFiddlerProxy)
    {
        _clientHandler = new HttpClientHandler
        {
            Proxy = new WebProxy("http://127.0.0.1:8888"), // Fiddler's default port
            UseProxy = useFiddlerProxy,
            UseCookies = true,
            CookieContainer = new CookieContainer()
        };
        _client = new HttpClient(_clientHandler) { BaseAddress = new Uri(BaseApiUrl) };
        _sessionManager = new PersonalCapitalSessionManager(_client, _clientHandler.CookieContainer, BaseUrl);
        _participantClient = new HttpClient(_clientHandler)
        {
            BaseAddress = new Uri(ParticipantBaseUrl)
        };

        _authenticator = new PersonalCapitalAuthenticator(_client, _participantClient, _sessionManager);

        _participantClient = new HttpClient(_clientHandler)
        {
            BaseAddress = new Uri(BaseUrl)
        };

        SetupHeaders(_client);
        SetupHeaders(_participantClient);
        _sessionFilePath = sessionFilePath;
    }

    public CookieContainer CookieContainer
    {
        get => _clientHandler.CookieContainer;
        private set => _clientHandler.CookieContainer = value;
    }

    public string Csrf => _sessionManager.Csrf;

    public void Dispose()
    {
        _client.Dispose();
        _participantClient.Dispose();
        _clientHandler.Dispose();
        GC.SuppressFinalize(this);
    }

    public void PersistSession()
    {
        _sessionManager.PersistSession(_sessionFilePath);
    }

    public void RestoreSession()
    {
        _sessionManager.RestoreSession(_sessionFilePath);
        CookieContainer = _sessionManager.CookieContainer;
    }

    /// <summary>
    /// Authenticates with Personal Capital using the complete flow including 2FA
    /// </summary>
    /// <param name="username">Personal Capital username/email</param>
    /// <param name="password">Personal Capital password</param>
    /// <param name="twoFactorCodeCallback">Callback function to retrieve the 2FA code from the user</param>
    /// <param name="mode">Two-factor verification mode (SMS or Email)</param>
    /// <returns>True if authentication was successful, false otherwise</returns>
    public async Task<bool> AuthenticateAsync(
    string username,
    string password,
    Func<Task<string>> twoFactorCodeCallback,
    TwoFactorVerificationMode mode = TwoFactorVerificationMode.SMS)
    {
        try
        {
            // Step 1: Login with username and password
            var authResponse = await _authenticator.Login(username, password);

            // Step 2: Send 2FA challenge
            var sendSuccess = await _authenticator.SendTwoFactorChallenge(mode);
            if (!sendSuccess)
            {
                return false;
            }

            // Step 3: Get 2FA code from callback
            var code = await twoFactorCodeCallback();

            if (string.IsNullOrWhiteSpace(code))
            {
                return false;
            }

            // Step 4: Verify 2FA code
            var samlResponseData = await _authenticator.TwoFactorAuthenticate(code);
            if (string.IsNullOrWhiteSpace(samlResponseData))
            {
                return false;
            }

            // Step 5: Complete SSO to establish session on pc-api domain
            var ssoSuccess = await _authenticator.CompleteSSO(samlResponseData);

            return ssoSuccess;
        }
        catch (Exception ex)
        {
            Console.WriteLine(ex.Message);
            return false;
        }
    }



    public async Task<EmpowerApiResponse<T>> Fetch<T>(string url, object? data = null)
    {
        if (string.IsNullOrEmpty(_sessionManager.Csrf))
            throw new InvalidOperationException("User is not logged in. Call Login() first");

        var payload = (data ?? new { }).ToDynamic();
        payload.lastServerChangeId = "-1";
        payload.csrf = _sessionManager.Csrf;
        payload.apiClient = "WEB";

        var httpMessage = await _client.PostMultipartData(url, (object)payload);
        httpMessage.EnsureSuccessStatusCode();
        var response = await httpMessage.Content.ReadAsAsync<EmpowerApiResponse<T>>();
#if DEBUG
        if (response.Header.AuthLevel == Constants.AuthLevel.None)
        {
            throw new UnauthorizedAccessException("The session is not authenticated");
        }
#endif
        return response;
    }

    #region Mapped Fetch/Get Api Methods

    public Task<EmpowerApiResponse<FetchAccountsData>> FetchAccounts(object? data = null)
    {
        return Fetch<FetchAccountsData>(@"newaccount/getAccounts2", data);
    }

    public Task<EmpowerApiResponse<List<CategoryData>>> FetchCategories(object? data = null)
    {
        return Fetch<List<CategoryData>>(@"transactioncategory/getCategories", data);
    }

    public Task<EmpowerApiResponse<FetchUserMessagesData>> FetchUserMessages(object? data = null)
    {
        return Fetch<FetchUserMessagesData>(@"message/getUserMessages", data);
    }

    public Task<EmpowerApiResponse<FetchPersonData>> FetchPerson(object? data = null)
    {
        return Fetch<FetchPersonData>(@"person/getPerson", data);
    }

    public Task<EmpowerApiResponse<FetchUserTransactionsData>> FetchUserTransactions(FetchUserTransactionsRequest data)
    {
        return Fetch<FetchUserTransactionsData>(@"transaction/getUserTransactions", data);
    }

    public Task<EmpowerApiResponse<UpdateTransactionResponseData>> UpdateTransaction(UpdateTransactionRequest data)
    {
        return Fetch<UpdateTransactionResponseData>(@"transaction/updateUserTransactions2", data);
    }

    public Task<EmpowerApiResponse<List<BillReminderData>>> FetchBills()
    {
        return Fetch<List<BillReminderData>>(@"account/getBillReminders");
    }

    public Task<EmpowerApiResponse<List<FetchTagsData>>> FetchTags()
    {
        return Fetch<List<FetchTagsData>>(@"transactiontag/getTags");
    }

    #endregion

    private static void SetupHeaders(HttpClient client)
    {
        client.DefaultRequestHeaders.Referrer = new Uri("https://participant.empower-retirement.com/");
        client.DefaultRequestHeaders.Add("Origin", "https://participant.empower-retirement.com");
        client.DefaultRequestHeaders.Add("User-Agent", "Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/143.0.0.0 Safari/537.36");
        client.DefaultRequestHeaders.Add("Accept", "application/json, text/plain, */*");
        client.DefaultRequestHeaders.Add("sec-ch-ua", "\"Google Chrome\";v=\"143\", \"Chromium\";v=\"143\", \"Not A(Brand\";v=\"24\"");
        client.DefaultRequestHeaders.Add("sec-ch-ua-mobile", "?0");
        client.DefaultRequestHeaders.Add("sec-ch-ua-platform", "\"Windows\"");
        client.DefaultRequestHeaders.Add("sec-fetch-dest", "empty");
        client.DefaultRequestHeaders.Add("sec-fetch-mode", "cors");
        client.DefaultRequestHeaders.Add("sec-fetch-site", "same-site");
    }
}
