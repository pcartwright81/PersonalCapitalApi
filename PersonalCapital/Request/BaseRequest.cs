using Newtonsoft.Json;

namespace PersonalCapital.Request {
    public class BaseRequest {
        [JsonProperty(PropertyName = "csrf")]
        public string Csrf { get; set; }

        [JsonProperty(PropertyName = "apiClient")]
        public string ApiClient { get; set; }

        [JsonProperty(PropertyName = "bindDevice")]
        public string BindDevice { get; set; }

        [JsonProperty(PropertyName = "skipLinkAccount")]
        public string SkipLinkAccount { get; set; }

        [JsonProperty(PropertyName = "redirectTo")]
        public string RedirectTo { get; set; }

        [JsonProperty(PropertyName = "skipFirstUse")]
        public string SkipFirstUse { get; set; }

        [JsonProperty(PropertyName = "referrerId")]
        public string ReferrerId { get; set; }

        public BaseRequest() {
            ReferrerId = string.Empty;
            BindDevice = "false";
            SkipLinkAccount = "false";
            RedirectTo = string.Empty;
            SkipFirstUse = string.Empty;
            ApiClient = "WEB";
        }
    }
}
