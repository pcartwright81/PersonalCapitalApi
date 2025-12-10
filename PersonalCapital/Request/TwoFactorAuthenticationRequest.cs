using Newtonsoft.Json;

namespace PersonalCapital.Request;

public record TwoFactorAuthenticationRequest(
    string Csrf,
    [property: JsonProperty("challengeReason")] string ChallengeReason,
    [property: JsonProperty("challengeMethod")] string ChallengeMethod,
    [property: JsonProperty("code")] int Code
) : BaseRequest(Csrf);