using Newtonsoft.Json;

namespace PersonalCapital.Response {
    public class FetchUserMessagesResponse {
        [JsonProperty(PropertyName = "spHeader")]
        public HeaderResponse Header { get; set; }

        [JsonProperty(PropertyName = "spData")]
        public FetchUserMessagesData Data { get; set; }
    }
}
