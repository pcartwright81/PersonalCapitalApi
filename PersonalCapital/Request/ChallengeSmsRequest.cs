using Newtonsoft.Json;

namespace PersonalCapital.Request;

public record ChallengeSmsRequest(
    [property: JsonProperty("challengeReason")] string ChallengeReason,
    [property: JsonProperty("challengeMethod")] string ChallengeMethod,
    [property: JsonProperty("bindDevice")] string BindDevice,
    [property: JsonProperty("csrf")] string Csrf,
    [property: JsonProperty("apiClient")] string ApiClient
)
{
    public ChallengeSmsRequest(string csrf)
        : this(
            ChallengeReason: "DEVICE_AUTH",
            ChallengeMethod: "OP",
            BindDevice: "true",
            Csrf: csrf,
            ApiClient: "WEB"
        )
    { }
}