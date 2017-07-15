using System.Collections.Generic;
using Newtonsoft.Json;

namespace PersonalCapital.Response {
    public class ErrorInfoResponse {
        [JsonProperty(PropertyName = "code")]
        public int Code { get; set; }

        [JsonProperty(PropertyName = "message")]
        public string Message { get; set; }

        //[JsonProperty(PropertyName = "details")]
        //public List<ErrorDetailsResponse> Details { get; set; }
    }

    public class ErrorDetailsResponse {
        [JsonProperty(PropertyName = "fieldname")]
        public int FieldName { get; set; }
    }
}
