using Newtonsoft.Json;

namespace PersonalCapital.Request;

public record AuthenticationData(
    [property: JsonProperty("deviceFingerPrint")] string DeviceFingerPrint,
    [property: JsonProperty("userAgent")] string UserAgent,
    [property: JsonProperty("language")] string Language,
    [property: JsonProperty("hasLiedLanguages")] bool HasLiedLanguages,
    [property: JsonProperty("hasLiedResolution")] bool HasLiedResolution,
    [property: JsonProperty("hasLiedOs")] bool HasLiedOs,
    [property: JsonProperty("hasLiedBrowser")] bool HasLiedBrowser,
    [property: JsonProperty("userName")] string UserName,
    [property: JsonProperty("password")] string Password,
    [property: JsonProperty("flowName")] string FlowName,
    [property: JsonProperty("accu")] string Accu
);