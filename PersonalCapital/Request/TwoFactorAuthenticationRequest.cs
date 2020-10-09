using Newtonsoft.Json;

namespace PersonalCapital.Request
{
    public class TwoFactorAuthenticationRequest : BaseRequest
    {
        [JsonProperty(PropertyName = "challengeReason")]
        public string ChallengeReason { get; set; }

        [JsonProperty(PropertyName = "challengeMethod")]
        public string ChallengeMethod { get; set; }

        [JsonProperty(PropertyName = "code")] public int Code { get; set; }
    }
}