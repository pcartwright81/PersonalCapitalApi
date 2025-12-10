using Newtonsoft.Json;

namespace PersonalCapital.Request;

public record IdentifyUserRequest(
    string Csrf,
    [property: JsonProperty("username")] string Username
) : BaseRequest(Csrf);