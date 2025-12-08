using Newtonsoft.Json;

namespace PersonalCapital.Request;

public record AuthenticateSmsRequest(
    [property: JsonProperty("code")] string Code,
    [property: JsonProperty("challengeReason")] string ChallengeReason,
    [property: JsonProperty("challengeMethod")] string ChallengeMethod,
    [property: JsonProperty("bindDevice")] string BindDevice,
    [property: JsonProperty("csrf")] string Csrf,
    [property: JsonProperty("apiClient")] string ApiClient
)
{
    public AuthenticateSmsRequest(string code, string csrf)
        : this(
            Code: code,
            ChallengeReason: "DEVICE_AUTH",
            ChallengeMethod: "OP",
            BindDevice: "true",
            Csrf: csrf,
            ApiClient: "WEB"
        )
    { }
}