﻿﻿﻿using Newtonsoft.Json;

namespace PersonalCapital.Response;

public record CredentialInfo(
    [property: JsonProperty(PropertyName = "status")] string Status,
    [property: JsonProperty(PropertyName = "name")] string Name
);