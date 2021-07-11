using System.Collections.Generic;
using Newtonsoft.Json;

namespace PersonalCapital.Response
{
    public class FetchCategoriesResponse
    {
        [JsonProperty(PropertyName = "spHeader")]
        public HeaderResponse Header { get; set; }

        [JsonProperty(PropertyName = "spData")]
        public List<CategoryData> Data { get; set; }
    }
}