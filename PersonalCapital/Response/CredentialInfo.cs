using Newtonsoft.Json;

namespace PersonalCapital.Response
{
    public class CredentialInfo
    {
        [JsonProperty(PropertyName = "status")]
        public string Status { get; set; }

        [JsonProperty(PropertyName = "name")] public string Name { get; set; }
    }
}