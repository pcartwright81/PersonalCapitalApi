using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Net.Http;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace PersonalCapital.Api;

public partial class PersonalCapitalSessionManager(HttpClient client, CookieContainer cookieContainer, string baseUrl)
{

    [GeneratedRegex(@"window\.csrf ='([a-f0-9-]+)'", RegexOptions.Compiled)]
    private static partial Regex CsrfRegex();

    public CookieContainer CookieContainer { get; private set; } = cookieContainer;
    public string Csrf { get; private set; } = string.Empty;

    public void UpdateCsrf(string csrf)
    {
        if (!string.IsNullOrEmpty(csrf))
        {
            Csrf = csrf;
        }
    }

    public async Task InitializeCsrf()
    {
        var httpMessage = await client.GetAsync(baseUrl);
        httpMessage.EnsureSuccessStatusCode();

        var result = await httpMessage.Content.ReadAsStringAsync();
        var match = CsrfRegex().Match(result);

        if (match.Success)
        {
            Csrf = match.Groups[1].Value;
        }
    }

    public void PersistSession(string filename)
    {
        var sessionData = new
        {
            Csrf,
            Cookies = CookieContainer.GetAllCookies()
        };
        var json = JsonConvert.SerializeObject(sessionData, Formatting.Indented);
        File.WriteAllText(filename, json);
    }

    public void RestoreSession(string filename)
    {
        if (!File.Exists(filename)) return;

        var json = File.ReadAllText(filename);

        try
        {
            var sessionData = JsonConvert.DeserializeObject<dynamic>(json);

            // Restore CSRF
            if (sessionData?.Csrf != null)
            {
                Csrf = sessionData.Csrf.ToString();
            }

            // Restore cookies
            var cookiesJson = sessionData?.Cookies?.ToString();
            if (!string.IsNullOrEmpty(cookiesJson))
            {
                var cookies = JsonConvert.DeserializeObject<List<Cookie>>(cookiesJson);
                if (cookies != null)
                {
                    var newCookieContainer = new CookieContainer();
                    foreach (var cookie in cookies)
                    {
                        // Ensure cookies aren't expired on restore
                        if (cookie.Expires < DateTime.Now)
                            cookie.Expires = DateTime.Now.AddYears(1);
                        newCookieContainer.Add(cookie);
                    }
                    CookieContainer = newCookieContainer;
                }
            }
        }
        catch
        {
            // Fallback to old format (just cookies array)
            var cookies = JsonConvert.DeserializeObject<List<Cookie>>(json);
            if (cookies != null)
            {
                var newCookieContainer = new CookieContainer();
                foreach (var cookie in cookies)
                {
                    if (cookie.Expires < DateTime.Now)
                        cookie.Expires = DateTime.Now.AddYears(1);
                    newCookieContainer.Add(cookie);
                }
                CookieContainer = newCookieContainer;
            }
        }
    }
}
