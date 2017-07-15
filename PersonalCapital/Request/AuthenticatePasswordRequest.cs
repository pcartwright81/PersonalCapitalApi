using Newtonsoft.Json;

namespace PersonalCapital.Request {
    public class AuthenticatePasswordRequest : BaseRequest {
        [JsonProperty(PropertyName = "passwd")]
        public string Password { get; set; }

        [JsonProperty(PropertyName = "deviceName")]
        public string DeviceName { get; set; }
    }
}