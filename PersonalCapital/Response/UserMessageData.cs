using System;
using System.Collections.Generic;
using Newtonsoft.Json;
using PersonalCapital.Extensions;

namespace PersonalCapital.Response
{
    public class UserMessageData
    {
        [JsonProperty(PropertyName = "template")]
        public string Template { get; set; }

        [JsonProperty(PropertyName = "summary")]
        public string Summary { get; set; }

        [JsonConverter(typeof(UnixEpochConverter))]
        [JsonProperty(PropertyName = "updatedTime", NullValueHandling = NullValueHandling.Ignore)]
        public DateTime? UpdatedTime { get; set; }

        [JsonConverter(typeof(UnixEpochConverter))]
        [JsonProperty(PropertyName = "lastViewedTime", NullValueHandling = NullValueHandling.Ignore)]
        public DateTime? LastViewedTime { get; set; }

        [JsonProperty(PropertyName = "isValid")]
        public bool IsValid { get; set; }

        //[JsonProperty(PropertyName = "resources")]
        //public List<?> Resources { get; set; }

        [JsonProperty(PropertyName = "title")] public string Title { get; set; }

        [JsonProperty(PropertyName = "userMessageId")]
        public int UserMessageId { get; set; }

        [JsonProperty(PropertyName = "priority")]
        public string Priority { get; set; }

        [JsonProperty(PropertyName = "component")]
        public string Component { get; set; }

        [JsonProperty(PropertyName = "action")]
        public List<ActionData> Action { get; set; }

        [JsonConverter(typeof(UnixEpochConverter))]
        [JsonProperty(PropertyName = "createdTime", NullValueHandling = NullValueHandling.Ignore)]
        public DateTime? CreatedTime { get; set; }

        [JsonProperty(PropertyName = "isStale")]
        public bool IsStale { get; set; }
    }
}