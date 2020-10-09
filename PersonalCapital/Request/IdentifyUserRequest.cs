using Newtonsoft.Json;

namespace PersonalCapital.Request
{
    public class IdentifyUserRequest : BaseRequest
    {
        [JsonProperty(PropertyName = "username")]
        public string Username { get; set; }
    }
}