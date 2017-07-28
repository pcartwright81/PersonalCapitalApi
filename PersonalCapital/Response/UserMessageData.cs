using System;
using System.Collections.Generic;
using Newtonsoft.Json;

namespace PersonalCapital.Response {
    public class UserMessageData {
        [JsonProperty(PropertyName = "template")]
        public string Template { get; set; }

        [JsonProperty(PropertyName = "summary")]
        public string Summary { get; set; }

        //[JsonProperty(PropertyName = "updatedTime")]
        //public DateTime UpdatedTime { get; set; }

        //[JsonProperty(PropertyName = "lastViewedTime")]
        //public DateTime LastViewedTime { get; set; }

        [JsonProperty(PropertyName = "isValid")]
        public bool IsValid { get; set; }

        //[JsonProperty(PropertyName = "resources")]
        //public List<?> Resources { get; set; }

        [JsonProperty(PropertyName = "title")]
        public string Title { get; set; }

        [JsonProperty(PropertyName = "userMessageId")]
        public int UserMessageId { get; set; }

        [JsonProperty(PropertyName = "priority")]
        public string Priority { get; set; }

        [JsonProperty(PropertyName = "component")]
        public string Component { get; set; }

        [JsonProperty(PropertyName = "action")]
        public List<ActionData> Action { get; set; }

        //[JsonProperty(PropertyName = "createdTime")]
        //public DateTime CreatedTime { get; set; }

        [JsonProperty(PropertyName = "isStale")]
        public bool IsStale { get; set; }
    }
}
