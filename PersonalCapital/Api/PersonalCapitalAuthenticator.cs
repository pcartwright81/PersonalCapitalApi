using PersonalCapital.Request;
using PersonalCapital.Response;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Json;
using System.Threading.Tasks;

namespace PersonalCapital.Api;

public class PersonalCapitalAuthenticator(HttpClient client, PersonalCapitalSessionManager sessionManager)
{
    public async Task<AuthResponse> Login(string username, string password)
    {
        await sessionManager.InitializeCsrf();
        Console.WriteLine($"Initial CSRF: {sessionManager.Csrf}");

        var authData = new AuthenticationData(
           DeviceFingerPrint: "1a7c37451da15092050556ea76dea4f8",
           UserAgent: "Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/143.0.0.0 Safari/537.36",
           Language: "en-US",
           HasLiedLanguages: false,
           HasLiedResolution: false,
           HasLiedOs: false,
           HasLiedBrowser: false,
           UserName: username,
           Password: password,
           FlowName: "mfa",
           Accu: "MYERIRA",
           RequestSrc: "empower_browser"
       );

        var httpMessage = await client.PostAsJsonAsync("auth/multiauth/noauth/authenticate", authData);
        Console.WriteLine($"Login Response Status: {httpMessage.StatusCode}");

        var responseContent = await httpMessage.Content.ReadAsStringAsync();
        Console.WriteLine($"Login Response Body: {responseContent}");

        var response = await httpMessage.Content.ReadAsAsync<AuthResponse>();
        Console.WriteLine($"IdToken present: {!string.IsNullOrEmpty(response.IdToken)}");
        Console.WriteLine($"Success: {response.Success}");

        // Complete authentication with the idToken
        if (!string.IsNullOrEmpty(response.IdToken))
        {
            Console.WriteLine("Calling AuthenticateToken...");
            await AuthenticateToken(response.IdToken);
            Console.WriteLine("AuthenticateToken completed");
        }
        else
        {
            Console.WriteLine("WARNING: No IdToken received from login!");
        }

        return response;
    }

    private async Task AuthenticateToken(string idToken)
    {
        Console.WriteLine($"AuthenticateToken - IdToken length: {idToken.Length}");

        var tokenData = new
        {
            deviceFingerPrint = "520cc91e9af663c4c590fff24c3bd777",
            userAgent = "Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/143.0.0.0 Safari/537.36",
            language = "en-US",
            hasLiedLanguages = false,
            hasLiedResolution = false,
            hasLiedOs = false,
            hasLiedBrowser = false,
            flowName = "mfa",
            accu = "MYERIRA",
            requestSrc = "empower_browser",
            idToken,
            authProvider = "EMPOWER"
        };

        var tokenRequest = new HttpRequestMessage(HttpMethod.Post, "https://ira.empower-retirement.com/participant-web-services/rest/nonauth/authenticateToken")
        {
            Content = JsonContent.Create(tokenData)
        };

        tokenRequest.Headers.Add("Origin", "https://ira.empower-retirement.com");
        tokenRequest.Headers.Referrer = new Uri("https://ira.empower-retirement.com/participant/");

        var tokenResponse = await client.SendAsync(tokenRequest);
        Console.WriteLine($"AuthenticateToken Response Status: {tokenResponse.StatusCode}");

        var tokenResponseContent = await tokenResponse.Content.ReadAsStringAsync();
        Console.WriteLine($"AuthenticateToken Response: {tokenResponseContent}");

        tokenResponse.EnsureSuccessStatusCode();
    }


    public async Task<bool> SendTwoFactorChallenge(TwoFactorVerificationMode mode, string accu = "MYERIRA")
    {
        // Get available delivery options
        var deliveryOptions = await GetDeliveryOptions();

        // Select the appropriate delivery option based on mode
        var deliveryOption = mode == TwoFactorVerificationMode.SMS
            ? deliveryOptions.FirstOrDefault(d => d.StartsWith("sms:", StringComparison.OrdinalIgnoreCase))
            : deliveryOptions.FirstOrDefault(d => d.StartsWith("email:", StringComparison.OrdinalIgnoreCase));

        if (string.IsNullOrEmpty(deliveryOption))
        {
            throw new InvalidOperationException($"No {mode} delivery option available");
        }

        var response = await client.PostAsJsonAsync(
            "/rest/partialauth/mfa/createAndDeliverActivationCode",
            new
            {
                deliveryOption,
                accu
            }
        );

        return response.IsSuccessStatusCode;
    }


    public async Task<List<string>> GetDeliveryOptions()
    {
        var response = await client.GetAsync("/rest/partialauth/mfa/deliveryOptions");

        if (!response.IsSuccessStatusCode)
        {
            throw new HttpRequestException($"Failed to get delivery options: {response.StatusCode}");
        }

        var options = await response.Content.ReadFromJsonAsync<List<string>>();
        return options ?? new List<string>();
    }

    public async Task<bool> TwoFactorAuthenticate(
        TwoFactorVerificationMode mode,
        string verificationCode)
    {
        var response = await client.PostAsJsonAsync(
            "/rest/partialauth/mfa/verifycode",
            new
            {
                verificationCode,
                deliveryOptions = mode.ToString().ToLower()
            }
        );

        return response.IsSuccessStatusCode;
    }

}
