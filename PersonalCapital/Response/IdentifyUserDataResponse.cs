using System.Collections.Generic;
using Newtonsoft.Json;

namespace PersonalCapital.Response
{
    public class IdentifyUserDataResponse
    {
        [JsonProperty(PropertyName = "credentials")]
        public List<string> Credentials { get; set; }

        [JsonProperty(PropertyName = "userStatus")]
        public string UserStatus { get; set; }

        [JsonProperty(PropertyName = "allCredentials")]
        public List<CredentialInfo> AllCredentials { get; set; }
    }
}