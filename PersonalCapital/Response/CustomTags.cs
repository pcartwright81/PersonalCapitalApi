using Newtonsoft.Json;
using System.Collections.Generic;

namespace PersonalCapital.Response
{
    public class CustomTags
    {
        [JsonProperty(PropertyName = "systemTags")]
        public List<long> SystemTags { get; set; }

        [JsonProperty(PropertyName = "userTags")]
        public List<long> UserTags { get; set; }
    }
}
