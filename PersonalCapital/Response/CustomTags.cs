﻿using Newtonsoft.Json;
using System.Collections.Generic;

namespace PersonalCapital.Response;

public record CustomTags(
    [property: JsonProperty(PropertyName = "systemTags")] List<long> SystemTags,
    [property: JsonProperty(PropertyName = "userTags")] List<long> UserTags
);
