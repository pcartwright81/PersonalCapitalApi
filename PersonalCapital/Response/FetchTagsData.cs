namespace PersonalCapital.Response
{
    using Newtonsoft.Json;

    public class FetchTagsData
    {
        [JsonProperty("tagId")]
        public long TagId { get; set; }

        [JsonProperty("tagName")]
        public string TagName { get; set; }

        public string DataProvider { get; set; } = "Personal Capital";
    }
}