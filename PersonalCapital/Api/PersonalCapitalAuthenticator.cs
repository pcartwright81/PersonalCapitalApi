using PersonalCapital.Request;
using PersonalCapital.Response;
using System;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Json;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace PersonalCapital.Api;

public class PersonalCapitalAuthenticator
{
    private readonly HttpClient _client;
    private readonly PersonalCapitalSessionManager _sessionManager;
    private string? _email;

    public PersonalCapitalAuthenticator(HttpClient client, PersonalCapitalSessionManager sessionManager)
    {
        _client = client;
        _sessionManager = sessionManager;
    }

    /// <summary>
    /// Login using direct API authentication (no SAML)
    /// </summary>
    public async Task<bool> LoginAsync(string username, string password)
    {
        try
        {
            Console.WriteLine("=== STEP 1: Initial Authentication ===");

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

            var authResponse = await _client.PostAsJsonAsync("api/auth/multiauth/noauth/authenticate", authData);
            var authContent = await authResponse.Content.ReadAsStringAsync();
            Console.WriteLine($"Status: {authResponse.StatusCode}");

            if (!authResponse.IsSuccessStatusCode)
            {
                Console.WriteLine($"Authentication failed: {authContent}");
                return false;
            }

            var authResult = JsonConvert.DeserializeObject<AuthResponse>(authContent);

            if (authResult == null || !authResult.Success || string.IsNullOrEmpty(authResult.IdToken))
            {
                Console.WriteLine($"No idToken received: {authContent}");
                return false;
            }

            Console.WriteLine($"✓ Received idToken (length: {authResult.IdToken.Length})");

            // STEP 2: Token Authentication
            Console.WriteLine("\n=== STEP 2: Token Authentication ===");

            var tokenRequest = new TokenAuthenticationRequest(authResult.IdToken);

            var tokenResponse = await _client.PostAsJsonAsync("api/credential/authenticateToken", tokenRequest);
            var tokenContent = await tokenResponse.Content.ReadAsStringAsync();
            Console.WriteLine($"Status: {tokenResponse.StatusCode}");

            var tokenResult = JsonConvert.DeserializeObject<AuthResponse>(tokenContent);

            if (tokenResult?.SpHeader == null)
            {
                Console.WriteLine($"No spHeader received: {tokenContent}");
                return false;
            }

            // Check for expected "Authorization required" error (code 200)
            var authRequiredError = tokenResult.SpHeader.Errors
                ?.FirstOrDefault(e => e.Code == 200 && (e.Message?.Contains("Authorization required") ?? false));

            // Extract CSRF token
            if (string.IsNullOrEmpty(tokenResult.SpHeader.Csrf))
            {
                Console.WriteLine("No CSRF token received");
                return false;
            }

            _sessionManager.Csrf = tokenResult.SpHeader.Csrf;
            Console.WriteLine($"✓ Received CSRF token: {_sessionManager.Csrf}");

            // If already authenticated (no 2FA needed)
            if (tokenResult.SpHeader.Success)
            {
                _email = username;
                Console.WriteLine("✓ Already authenticated (no 2FA needed)");
                return true;
            }

            // Verify we got the expected auth required error
            if (authRequiredError == null)
            {
                Console.WriteLine($"Unexpected response: {tokenContent}");
                return false;
            }

            // STEP 3: SMS Challenge
            Console.WriteLine("\n=== STEP 3: SMS Challenge ===");
            await SendSmsChallenge();

            // STEP 4: Get and authenticate SMS code
            Console.WriteLine("\n=== STEP 4: SMS Authentication ===");
            Console.Write("Enter SMS code: ");
            var smsCode = Console.ReadLine();

            if (string.IsNullOrWhiteSpace(smsCode))
            {
                Console.WriteLine("SMS code is required");
                return false;
            }

            await AuthenticateSmsCode(smsCode);

            _email = username;
            Console.WriteLine("\n✅ Login successful!");
            return true;
        }
        catch (Exception ex)
        {
            Console.WriteLine($"❌ Login failed: {ex.Message}");
            Console.WriteLine($"Stack trace: {ex.StackTrace}");
            return false;
        }
    }

    private async Task SendSmsChallenge()
    {
        var challengeRequest = new ChallengeSmsRequest(_sessionManager.Csrf!);

        var response = await _client.PostAsJsonAsync("api/credential/challengeSmsFreemium", challengeRequest);
        var content = await response.Content.ReadAsStringAsync();

        Console.WriteLine($"Status: {response.StatusCode}");

        if (!response.IsSuccessStatusCode)
        {
            throw new Exception($"SMS challenge failed: {content}");
        }

        var result = JsonConvert.DeserializeObject<AuthResponse>(content);

        if (result?.SpHeader?.Success != true)
        {
            throw new Exception($"SMS challenge failed: {content}");
        }

        Console.WriteLine("✓ SMS code sent");
    }

    private async Task AuthenticateSmsCode(string code)
    {
        var authSmsRequest = new AuthenticateSmsRequest(code, _sessionManager.Csrf!);

        var response = await _client.PostAsJsonAsync("api/credential/authenticateSmsFreemium", authSmsRequest);
        var content = await response.Content.ReadAsStringAsync();

        Console.WriteLine($"Status: {response.StatusCode}");

        if (!response.IsSuccessStatusCode)
        {
            throw new Exception($"SMS authentication failed: {content}");
        }

        var result = JsonConvert.DeserializeObject<AuthResponse>(content);

        if (result?.SpHeader?.Success != true)
        {
            throw new Exception($"SMS authentication failed: {content}");
        }

        // Update CSRF if provided
        if (!string.IsNullOrEmpty(result.SpHeader.Csrf))
        {
            _sessionManager.Csrf = result.SpHeader.Csrf;
        }

        Console.WriteLine("✓ SMS authentication successful");
        Console.WriteLine($"Auth Level: {result.SpHeader.AuthLevel}");
        Console.WriteLine($"User Stage: {result.SpHeader.UserStage}");
    }

    public bool IsLoggedIn()
    {
        return !string.IsNullOrEmpty(_email) && !string.IsNullOrEmpty(_sessionManager.Csrf);
    }

    public string? GetEmail() => _email;
}