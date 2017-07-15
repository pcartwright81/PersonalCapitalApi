using Newtonsoft.Json;
using System.Collections.Generic;

namespace PersonalCapital.Response {
    public class HeaderResponse {

        [JsonProperty(PropertyName = "status")]
        public string Status { get; set; }

        [JsonProperty(PropertyName = "username")]
        public string Username { get; set; }

        [JsonProperty(PropertyName = "qualifiedLead")]
        public string QualifiedLead { get; set; }

        [JsonProperty(PropertyName = "userGuid")]
        public string UserGuid { get; set; }

        [JsonProperty(PropertyName = "SP_HEADER_VERSION")]
        public int HeaderVersion { get; set; }

        [JsonProperty(PropertyName = "userStage")]
        public string UserStage { get; set; }

        [JsonProperty(PropertyName = "accountsMetaData")]
        public List<string> AccountsMetaData { get; set; }

        [JsonProperty(PropertyName = "success")]
        public bool Success { get; set; }

        [JsonProperty(PropertyName = "authLevel")]
        public string AuthLevel { get; set; }

        [JsonProperty(PropertyName = "csrf")]
        public string Csrf { get; set; }

        [JsonProperty(PropertyName = "errors")]
        public List<ErrorInfoResponse> Errors { get; set; }

        [JsonProperty(PropertyName = "betaTester")]
        public bool BetaTester { get; set; }

        [JsonProperty(PropertyName = "developer")]
        public bool Developer { get; set; }
    }
}
