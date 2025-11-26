using Newtonsoft.Json;

namespace PersonalCapital.Request
{
    public class AuthenticationData
    {
        [JsonProperty("deviceFingerPrint")]
        public string DeviceFingerPrint { get; set; }

        [JsonProperty("userAgent")]
        public string UserAgent { get; set; }

        [JsonProperty("language")]
        public string Language { get; set; }

        [JsonProperty("hasLiedLanguages")]
        public bool HasLiedLanguages { get; set; }

        [JsonProperty("hasLiedResolution")]
        public bool HasLiedResolution { get; set; }

        [JsonProperty("hasLiedOs")]
        public bool HasLiedOs { get; set; }

        [JsonProperty("hasLiedBrowser")]
        public bool HasLiedBrowser { get; set; }

        [JsonProperty("userName")]
        public string UserName { get; set; }

        [JsonProperty("password")]
        public string Password { get; set; }

        [JsonProperty("flowName")]
        public string FlowName { get; set; }

        [JsonProperty("accu")]
        public string Accu { get; set; }
    }
}