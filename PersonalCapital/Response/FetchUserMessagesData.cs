using System.Collections.Generic;
using Newtonsoft.Json;

namespace PersonalCapital.Response
{
    public class FetchUserMessagesData
    {
        [JsonProperty(PropertyName = "userMessages")]
        public List<UserMessageData> UserMessages { get; set; }

        [JsonProperty(PropertyName = "enabled")]
        public bool Enabled { get; set; }
    }
}