using Newtonsoft.Json;

namespace PersonalCapital.Response {
    public class IdentifyUserResponse {

        [JsonProperty(PropertyName = "spHeader")]
        public HeaderResponse Header { get; set; }

        [JsonProperty(PropertyName = "spData")]
        public IdentifyUserDataResponse Data { get; set; }
    }
}