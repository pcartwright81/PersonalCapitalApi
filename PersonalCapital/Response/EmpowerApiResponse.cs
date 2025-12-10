using Newtonsoft.Json;

namespace PersonalCapital.Response;

public record EmpowerApiResponse<T>(
    [property: JsonProperty(PropertyName = "spHeader")] SpHeader Header,
    [property: JsonProperty(PropertyName = "spData")] T Data
);