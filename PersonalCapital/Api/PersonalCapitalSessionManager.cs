using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
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
        var cookies = CookieContainer.GetAllCookies();
        var json = JsonConvert.SerializeObject(cookies, Formatting.Indented);
        File.WriteAllText(filename, json);
    }

    public void RestoreSession(string filename)
    {
        if (!File.Exists(filename)) return;

        var json = File.ReadAllText(filename);
        var cookies = JsonConvert.DeserializeObject<List<Cookie>>(json);

        var newCookieContainer = new CookieContainer();
        foreach (var cookie in cookies)
        {
            // Ensure cookies aren't expired on restore
            if (cookie.Expires < DateTime.Now) cookie.Expires = DateTime.Now.AddYears(1);
            newCookieContainer.Add(cookie);
        }
        CookieContainer = newCookieContainer;
    }
}