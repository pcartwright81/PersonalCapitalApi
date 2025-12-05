using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace PersonalCapital.Response;

public record ResourceData(
    [property: JsonProperty("type")] string Type,
    [property: JsonProperty("url")] string Url,
    [property: JsonProperty("size")] string Size,
    [property: JsonProperty("alt")] string Alt,

    // JToken handles complex/dynamic nested objects (like ScorecardData vs simple Links)
    [property: JsonProperty("jsonContent")] JToken JsonContent
);