using Newtonsoft.Json;

namespace PersonalCapital.Response
{
    public class FetchPersonResponse
    {
        [JsonProperty(PropertyName = "spHeader")]
        public HeaderResponse Header { get; set; }

        [JsonProperty(PropertyName = "spData")]
        public FetchPersonData Data { get; set; }
    }
}