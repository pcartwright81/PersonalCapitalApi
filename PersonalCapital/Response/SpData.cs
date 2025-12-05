using System.Collections.Generic;
using Newtonsoft.Json;

namespace PersonalCapital.Response
{
    // 3. Data Section
    public record SpData(
        [property: JsonProperty("userMessages")] List<UserMessageData> UserMessages,
        [property: JsonProperty("enabled")] bool Enabled
    );
}