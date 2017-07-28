using System;
using System.Collections.Generic;
using Newtonsoft.Json;

namespace PersonalCapital.Response {
    public class ActionData {
        [JsonProperty(PropertyName = "key")]
        public string Key { get; set; }

        [JsonProperty(PropertyName = "title")]
        public string Title { get; set; }

        [JsonProperty(PropertyName = "url")]
        public string Url { get; set; }

        [JsonProperty(PropertyName = "type")]
        public string Type { get; set; }

        [JsonProperty(PropertyName = "Summary")]
        public string Summary { get; set; }

        [JsonProperty(PropertyName = "trackingFlag")]
        public bool? TrackingFlag { get; set; }

        [JsonProperty(PropertyName = "label")]
        public string Label { get; set; }
    }
}
