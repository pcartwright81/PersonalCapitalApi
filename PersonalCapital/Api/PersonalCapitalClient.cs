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
    private const string BaseApiUrl = BaseUrl + "api/";

    private readonly HttpClient _client;
    private readonly HttpClientHandler _clientHandler;
    private readonly PersonalCapitalSessionManager _sessionManager;
    private readonly PersonalCapitalAuthenticator _authenticator;

    public PersonalCapitalClient()
    {
        _clientHandler = new HttpClientHandler { UseCookies = true, CookieContainer = new CookieContainer() };
        _client = new HttpClient(_clientHandler) { BaseAddress = new Uri(BaseApiUrl) };
        _sessionManager = new PersonalCapitalSessionManager(_client, _clientHandler.CookieContainer);
        _authenticator = new PersonalCapitalAuthenticator(_client, _sessionManager);

        // 1. Critical Headers for Session Validation
        _client.DefaultRequestHeaders.Referrer = new Uri("https://participant.empower-retirement.com/");
        _client.DefaultRequestHeaders.Add("Origin", "https://participant.empower-retirement.com");

        // 2. Standard Browser Headers
        _client.DefaultRequestHeaders.Add("User-Agent", "Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/143.0.0.0 Safari/537.36");
        _client.DefaultRequestHeaders.Add("Accept", "application/json, text/javascript, */*; q=0.01");

        // 3. Security Headers (Chrome-specific)
        _client.DefaultRequestHeaders.Add("sec-ch-ua", "\"Google Chrome\";v=\"143\", \"Chromium\";v=\"143\", \"Not A(Brand\";v=\"24\"");
        _client.DefaultRequestHeaders.Add("sec-ch-ua-mobile", "?0");
        _client.DefaultRequestHeaders.Add("sec-ch-ua-platform", "\"Windows\"");
        _client.DefaultRequestHeaders.Add("sec-fetch-dest", "empty");
        _client.DefaultRequestHeaders.Add("sec-fetch-mode", "cors");
        _client.DefaultRequestHeaders.Add("sec-fetch-site", "same-site");

        // 4. Cache control
        _client.DefaultRequestHeaders.Add("cache-control", "no-cache");
        _client.DefaultRequestHeaders.Add("pragma", "no-cache");
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
        _clientHandler.Dispose();
        GC.SuppressFinalize(this);
    }

    public void PersistSession(string filename)
    {
        _sessionManager.PersistSession(filename);
    }

    public void RestoreSession(string filename)
    {
        _sessionManager.RestoreSession(filename);
        // After restoring, the client's cookie container needs to be updated
        CookieContainer = _sessionManager.CookieContainer;
    }

    public async Task<AuthResponse> Login(string username, string password,
        CookieContainer? sessionCookies = null)
    {
        if (sessionCookies != null) CookieContainer = sessionCookies;
        return await _authenticator.Login(username, password);
    }

    public async Task<EmpowerApiResponse<object?>> SendTwoFactorChallenge(TwoFactorVerificationMode mode)
    {
        return await _authenticator.SendTwoFactorChallenge(mode);
    }

    public async Task<EmpowerApiResponse<object?>> TwoFactorAuthenticate(TwoFactorVerificationMode mode, string code)
    {
        return await _authenticator.TwoFactorAuthenticate(mode, code);
    }

    public async Task<EmpowerApiResponse<T>> Fetch<T>(string url, object? data = null)
    {
        if (string.IsNullOrEmpty(Csrf)) throw new InvalidOperationException("User is not logged in. Call Login() first.");

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
            throw new UnauthorizedAccessException("The session is not authenticated.");
        }
#endif
        return response;
    }

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
}