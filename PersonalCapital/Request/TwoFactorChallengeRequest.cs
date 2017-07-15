using Newtonsoft.Json;

namespace PersonalCapital.Request {
    public class TwoFactorChallengeRequest : BaseRequest {
        [JsonProperty(PropertyName = "challengeReason")]
        public string ChallengeReason { get; set; }

        [JsonProperty(PropertyName = "challengeMethod")]
        public string ChallengeMethod { get; set; }

        [JsonProperty(PropertyName = "challengeType")]
        public string ChallengeType { get; set; }
    }
}