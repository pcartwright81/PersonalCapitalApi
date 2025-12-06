using Newtonsoft.Json;
using PersonalCapital.Models;
using System;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace PersonalCapital.Api;

public class PersonalCapitalSessionManager(HttpClient client, CookieContainer cookieContainer)
{
    private const string BaseUrl = "https://pc-api.empower-retirement.com/";
    private static readonly Regex CsrfRegex = new("window.csrf ='([a-f0-9-]+)'", RegexOptions.Compiled);

    public CookieContainer CookieContainer { get; private set; } = cookieContainer;
    public string Csrf { get; private set; }

    public async Task InitializeCsrf()
    {
        var httpMessage = await client.GetAsync(BaseUrl);
        httpMessage.EnsureSuccessStatusCode();

        var result = await httpMessage.Content.ReadAsStringAsync();
        var match = CsrfRegex.Match(result);

        if (match.Success)
        {
            Csrf = match.Groups[1].Value;
        }
    }

    public void PersistSession(string filename)
    {
        var sessionData = new OfflineSessionData
        {
            Csrf = Csrf,
            Cookies = CookieContainer.GetAllCookies().Cast<Cookie>().ToList()
        };
        var json = JsonConvert.SerializeObject(sessionData, Formatting.Indented);
        File.WriteAllText(filename, json);
    }

    public void RestoreSession(string filename)
    {
        if (!File.Exists(filename)) return;

        var json = File.ReadAllText(filename);
        var sessionData = JsonConvert.DeserializeObject<OfflineSessionData>(json);

        if (sessionData == null) return;
        // Restore CSRF
        if (!string.IsNullOrEmpty(sessionData.Csrf))
        {
            Csrf = sessionData.Csrf;
        }

        // Restore cookies
        var newCookieContainer = new CookieContainer();
        foreach (var cookie in sessionData.Cookies)
        {
            if (cookie.Expires < DateTime.Now) cookie.Expires = DateTime.Now.AddYears(1);
            newCookieContainer.Add(cookie);
        }
        CookieContainer = newCookieContainer;
    }

    public void UpdateCsrf(string csrf)
    {
        if (!string.IsNullOrEmpty(csrf))
        {
            Csrf = csrf;
        }
    }
}