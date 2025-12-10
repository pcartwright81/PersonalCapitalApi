﻿using Newtonsoft.Json;

namespace PersonalCapital.Response;

public record ErrorInfoResponse(
    [property: JsonProperty(PropertyName = "code")] int Code,
    [property: JsonProperty(PropertyName = "message")] string Message
    //[JsonProperty(PropertyName = "details")]
    //public List<ErrorDetailsResponse> Details { get; set; }
);

public record ErrorDetailsResponse(
    [property: JsonProperty(PropertyName = "fieldname")] int FieldName
);