using Newtonsoft.Json;

namespace PersonalCapital.Response {
    public class FetchUserTransactionsResponse {
        [JsonProperty(PropertyName = "spHeader")]
        public HeaderResponse Header { get; set; }

        [JsonProperty(PropertyName = "spData")]
        public FetchUserTransactionsData Data { get; set; }
    }
}
