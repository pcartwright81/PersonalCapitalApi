using Newtonsoft.Json;

namespace PersonalCapital.Request;

public record BaseRequest(
    [property: JsonProperty("csrf")] string Csrf,
    [property: JsonProperty("apiClient")] string ApiClient = "WEB",
    [property: JsonProperty("bindDevice")] string BindDevice = "false",
    [property: JsonProperty("skipLinkAccount")] string SkipLinkAccount = "false",
    [property: JsonProperty("redirectTo")] string RedirectTo = "",
    [property: JsonProperty("skipFirstUse")] string SkipFirstUse = "",
    [property: JsonProperty("referrerId")] string ReferrerId = ""
);