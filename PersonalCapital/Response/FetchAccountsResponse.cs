using Newtonsoft.Json;

namespace PersonalCapital.Response
{
    public class FetchAccountsResponse
    {
        [JsonProperty(PropertyName = "spHeader")]
        public HeaderResponse Header { get; set; }

        [JsonProperty(PropertyName = "spData")]
        public FetchAccountsData Data { get; set; }
    }
}