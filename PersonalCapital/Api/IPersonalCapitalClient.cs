using PersonalCapital.Request;
using PersonalCapital.Response;
using System;
using System.Collections.Generic;
using System.Net;
using System.Threading.Tasks;

namespace PersonalCapital.Api;

/// <summary>
/// Interface for Personal Capital API client operations
/// </summary>
public interface IPersonalCapitalClient : IDisposable
{
    /// <summary>
    /// Gets the cookie container for the HTTP client
    /// </summary>
    CookieContainer CookieContainer { get; }

    /// <summary>
    /// Gets the current CSRF token for authenticated requests
    /// </summary>
    string Csrf { get; }

    /// <summary>
    /// Persists the current session to disk
    /// </summary>
    void PersistSession();

    /// <summary>
    /// Restores a previously persisted session from disk
    /// </summary>
    void RestoreSession();

    /// <summary>
    /// Authenticates with Personal Capital using the complete flow including 2FA
    /// </summary>
    /// <param name="username">Personal Capital username/email</param>
    /// <param name="password">Personal Capital password</param>
    /// <param name="twoFactorCodeCallback">Callback function to retrieve the 2FA code from the user</param>
    /// <param name="mode">Two-factor verification mode (SMS or Email)</param>
    /// <returns>True if authentication was successful, false otherwise</returns>
    Task<bool> AuthenticateAsync(
        string username,
        string password,
        Func<Task<string>> twoFactorCodeCallback,
        TwoFactorVerificationMode mode = TwoFactorVerificationMode.SMS);

    /// <summary>
    /// Makes an authenticated request to the Personal Capital API
    /// </summary>
    /// <typeparam name="T">The type of data to deserialize from the response</typeparam>
    /// <param name="url">API endpoint URL</param>
    /// <param name="data">Optional request data</param>
    /// <returns>Response containing the requested data</returns>
    Task<EmpowerApiResponse<T>> Fetch<T>(string url, object? data = null);

    #region Account Operations

    /// <summary>
    /// Fetches all accounts associated with the user
    /// </summary>
    Task<EmpowerApiResponse<FetchAccountsData>> FetchAccounts(object? data = null);

    #endregion

    #region Transaction Operations

    /// <summary>
    /// Fetches user transactions based on the provided criteria
    /// </summary>
    Task<EmpowerApiResponse<FetchUserTransactionsData>> FetchUserTransactions(FetchUserTransactionsRequest data);

    /// <summary>
    /// Updates a user transaction
    /// </summary>
    Task<EmpowerApiResponse<UpdateTransactionResponseData>> UpdateTransaction(UpdateTransactionRequest data);

    /// <summary>
    /// Fetches all transaction categories
    /// </summary>
    Task<EmpowerApiResponse<List<CategoryData>>> FetchCategories(object? data = null);

    /// <summary>
    /// Fetches all transaction tags
    /// </summary>
    Task<EmpowerApiResponse<List<FetchTagsData>>> FetchTags();

    #endregion

    #region Message Operations

    /// <summary>
    /// Fetches user messages
    /// </summary>
    Task<EmpowerApiResponse<FetchUserMessagesData>> FetchUserMessages(object? data = null);

    #endregion

    #region Person Operations

    /// <summary>
    /// Fetches person/user information
    /// </summary>
    Task<EmpowerApiResponse<FetchPersonData>> FetchPerson(object? data = null);

    #endregion

    #region Bill Operations

    /// <summary>
    /// Fetches bill reminders
    /// </summary>
    Task<EmpowerApiResponse<List<BillReminderData>>> FetchBills();

    #endregion
}