﻿using Newtonsoft.Json;

namespace PersonalCapital.Response;

public record HeaderOnlyResponse(
    [property: JsonProperty(PropertyName = "spHeader")] SpHeader Header
);