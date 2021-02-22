using Newtonsoft.Json;

namespace PersonalCapital.Response
{
    public class CatagoryData
    {
        [JsonProperty(PropertyName = "isEditable")]
        public bool IsEditable { get; set; }

        [JsonProperty(PropertyName = "name")] public string Name { get; set; }

        [JsonProperty(PropertyName = "isCustom")]
        public bool IsCustom { get; set; }

        [JsonProperty(PropertyName = "isOverride")]
        public bool IsOverride { get; set; }

        [JsonProperty(PropertyName = "transactionCategoryId")]
        public int TransactionCategoryId { get; set; }

        [JsonProperty(PropertyName = "shortDescription")]
        public string ShortDescription { get; set; }

        [JsonProperty(PropertyName = "type")] public string Type { get; set; }

        public string DataProvider { get; set; } = "Personal Capital";
    }
}