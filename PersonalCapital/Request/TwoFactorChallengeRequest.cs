using Newtonsoft.Json;

namespace PersonalCapital.Request;

public record TwoFactorChallengeRequest(
    string Csrf,
    [property: JsonProperty("challengeReason")] string ChallengeReason,
    [property: JsonProperty("challengeMethod")] string ChallengeMethod,
    [property: JsonProperty("challengeType")] string ChallengeType
) : BaseRequest(Csrf);