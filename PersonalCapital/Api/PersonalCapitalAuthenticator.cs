using PersonalCapital.Extensions;
using PersonalCapital.Request;
using PersonalCapital.Response;
using System;
using System.Net.Http;
using System.Threading.Tasks;

namespace PersonalCapital.Api;

public class PersonalCapitalAuthenticator(HttpClient client, PersonalCapitalSessionManager sessionManager)
{
    public async Task<AuthResponse> Login(string username, string password)
    {
        await sessionManager.InitializeCsrf();

        var authenticationResponse = await AuthenticatePassword(username, password);
      
        //ParseHeaderForErrors(authenticationResponse.Header);
        return authenticationResponse;
    }

    public async Task<HeaderOnlyResponse> SendTwoFactorChallenge(TwoFactorVerificationMode mode)
    {
        if (string.IsNullOrEmpty(sessionManager.Csrf))
            throw new InvalidOperationException("User is not logged in. Call Login() before sending a two-factor challenge.");

        var method = mode switch
        {
            TwoFactorVerificationMode.SMS => "challengeSms",
            TwoFactorVerificationMode.EMail => "challengeEmail",
            _ => throw new ArgumentOutOfRangeException(nameof(mode), mode, null)
        };

        var data = new TwoFactorChallengeRequest(sessionManager.Csrf, "DEVICE_AUTH", "OP", method);

        // Send data as HTTP Encoded
        var httpMessage = await client.PostHttpEncodedData($"credential/{method}", data);
        return await httpMessage.Content.ReadAsAsync<HeaderOnlyResponse>();
    }

    public async Task<HeaderOnlyResponse> TwoFactorAuthenticate(TwoFactorVerificationMode mode, string code)
    {
        if (string.IsNullOrEmpty(sessionManager.Csrf))
            throw new InvalidOperationException("User is not logged in. Call Login() before authenticating a two-factor challenge.");

        var method = mode switch
        {
            TwoFactorVerificationMode.SMS => "authenticateSms",
            TwoFactorVerificationMode.EMail => "authenticateEmail",
            _ => throw new ArgumentOutOfRangeException(nameof(mode), mode, null)
        };

        if (!int.TryParse(code, out var numericCode))
        {
            throw new ArgumentException("The provided two-factor code is not a valid number.", nameof(code));
        }

        var data = new TwoFactorAuthenticationRequest(sessionManager.Csrf, "DEVICE_AUTH", "OP", numericCode);
        var httpMessage = await client.PostHttpEncodedData($"credential/{method}", data);
        return await httpMessage.Content.ReadAsAsync<HeaderOnlyResponse>();
    }

    private async Task<AuthResponse> AuthenticatePassword(string username, string password)
    {
        var authData = new AuthenticationData(
            DeviceFingerPrint: "1a7c37451da15092050556ea76dea4f8",
            UserAgent: "Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/120.0.0.0 Safari/537.36",
            Language: "en-US",
            HasLiedLanguages: false,
            HasLiedResolution: false,
            HasLiedOs: false,
            HasLiedBrowser: false,
            UserName: username,
            Password: password,
            FlowName: "mfa",
            Accu: "MYERIRA"
        );

        var httpMessage = await client.PostAsJsonAsync("auth/multiauth/noauth/authenticate", authData);
        return await httpMessage.Content.ReadAsAsync<AuthResponse>();
    }
}