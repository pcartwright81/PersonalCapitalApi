using Newtonsoft.Json;

namespace PersonalCapital.Response {
    public class HeaderOnlyResponse {
        [JsonProperty(PropertyName = "spHeader")]
        public HeaderResponse Header { get; set; }
    }
}