using PersonalCapital.Request;
using PersonalCapital.Response;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Json;
using System.Threading.Tasks;

namespace PersonalCapital.Api;

public class PersonalCapitalAuthenticator(HttpClient client, HttpClient participantClient, PersonalCapitalSessionManager sessionManager)
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
        var deliveryOptionsResponse = await GetDeliveryOptions();

        // Select the appropriate delivery option based on mode
        var deliveryOption = mode == TwoFactorVerificationMode.SMS
            ? deliveryOptionsResponse.DeliverySet.FirstOrDefault(d => d.DeliveryType.StartsWith("sms:", StringComparison.OrdinalIgnoreCase))
            : deliveryOptionsResponse.DeliverySet.FirstOrDefault(d => d.DeliveryType.StartsWith("email:", StringComparison.OrdinalIgnoreCase));

        if (deliveryOption == null)
        {
            throw new InvalidOperationException($"No {mode} delivery option available");
        }

        var response = await participantClient.PostAsJsonAsync(
            "participant-web-services/rest/partialauth/mfa/createAndDeliverActivationCode",
            new
            {
                deliveryOption = deliveryOption.DeliveryType,
                accu
            }
        );

        if (response.StatusCode == System.Net.HttpStatusCode.BadRequest)
        {
            var errorContent = await response.Content.ReadAsStringAsync();
            if (errorContent.Contains("AUAC_12") || errorContent.Contains("Max number of activation challenges"))
            {
                throw new InvalidOperationException("Too many code requests. Please wait 15-30 minutes before trying again.");
            }
        }


        return response.IsSuccessStatusCode;
    }

    public async Task<bool> CompleteSSO(string samlResponseData)
    {
        // Post the SAML response directly to pc-api
        var content = new MultipartFormDataContent
    {
        { new StringContent(samlResponseData), "SAMLResponse" }
    };

        var ssoResponse = await client.PostAsync("/empower/sso/saml2", content);

        return ssoResponse.IsSuccessStatusCode;
    }




    public async Task<DeliveryOptionsResponse> GetDeliveryOptions()
    {
        var response = await participantClient.GetAsync("participant-web-services/rest/partialauth/mfa/deliveryOptions");

        if (!response.IsSuccessStatusCode)
        {
            throw new HttpRequestException($"Failed to get delivery options: {response.StatusCode}");
        }

        var options = await response.Content.ReadFromJsonAsync<DeliveryOptionsResponse>();
        return options ?? new DeliveryOptionsResponse();
    }


    public async Task<string?> TwoFactorAuthenticate(string verificationCode, bool rememberDevice = true)
    {
        var response = await participantClient.PostAsJsonAsync(
            "participant-web-services/rest/partialauth/mfa/verifycode",
            new
            {
                rememberDevice,
                verificationCode,
                flowName = "mfa"
            }
        );

        if (!response.IsSuccessStatusCode)
        {
            return null;
        }

        // The response contains the SAML data we need
        var content = await response.Content.ReadAsStringAsync();
        return content; // Return the whole response - it has the auth info
    }

}
