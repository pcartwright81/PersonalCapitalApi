using Newtonsoft.Json;

namespace PersonalCapital.Response;

public record ActionData(
    [property: JsonProperty("key")] string Key,
    [property: JsonProperty("title")] string Title,
    [property: JsonProperty("url")] string Url,
    [property: JsonProperty("type")] string Type,
    [property: JsonProperty("summary")] string Summary,
    [property: JsonProperty("trackingFlag")] bool TrackingFlag,
    [property: JsonProperty("label")] string Label
);
