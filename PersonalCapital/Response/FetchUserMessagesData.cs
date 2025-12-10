﻿using Newtonsoft.Json;
using System.Collections.Generic;

namespace PersonalCapital.Response;

public record FetchUserMessagesData(
    [property: JsonProperty(PropertyName = "userMessages")] List<UserMessageData> UserMessages,
    [property: JsonProperty(PropertyName = "enabled")] bool Enabled
);