namespace PersonalCapital.Response
{
    using Newtonsoft.Json;
    using System.Collections.Generic;

    public class FetchTagsResponse
    {
        [JsonProperty(PropertyName = "spHeader")]
        public HeaderResponse Header { get; set; }

        [JsonProperty(PropertyName = "spData")]
        public List<FetchTagsData> Data { get; set; }
    }
}
