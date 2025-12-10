using Newtonsoft.Json;

namespace PersonalCapital.Request;

public record TokenAuthenticationRequest(
    [property: JsonProperty("deviceFingerPrint")] string DeviceFingerPrint,
    [property: JsonProperty("userAgent")] string UserAgent,
    [property: JsonProperty("language")] string Language,
    [property: JsonProperty("hasLiedLanguages")] bool HasLiedLanguages,
    [property: JsonProperty("hasLiedResolution")] bool HasLiedResolution,
    [property: JsonProperty("hasLiedOs")] bool HasLiedOs,
    [property: JsonProperty("hasLiedBrowser")] bool HasLiedBrowser,
    [property: JsonProperty("flowName")] string FlowName,
    [property: JsonProperty("accu")] string Accu,
    [property: JsonProperty("requestSrc")] string RequestSrc,
    [property: JsonProperty("idToken")] string IdToken,
    [property: JsonProperty("authProvider")] string AuthProvider,
    [property: JsonProperty("csrf")] string? Csrf,
    [property: JsonProperty("apiClient")] string ApiClient
)
{
    public TokenAuthenticationRequest(string idToken, string? csrf = null)
        : this(
            DeviceFingerPrint: "520cc91e9af663c4c590fff24c3bd777",
            UserAgent: "Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/143.0.0.0 Safari/537.36",
            Language: "en-US",
            HasLiedLanguages: false,
            HasLiedResolution: false,
            HasLiedOs: false,
            HasLiedBrowser: false,
            FlowName: "mfa",
            Accu: "MYERIRA",
            RequestSrc: "empower_browser",
            IdToken: idToken,
            AuthProvider: "EMPOWER",
            Csrf: csrf,
            ApiClient: "WEB"
        )
    { }
}