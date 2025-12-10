namespace PersonalCapital.Response;

using System.Text.Json.Serialization;
public record AuthResponse(
    [property: JsonPropertyName("authProvider")] string AuthProvider,
    [property: JsonPropertyName("success")] bool Success,
    [property: JsonPropertyName("idToken")] string IdToken,
    [property: JsonPropertyName("accuCode")] string AccuCode,
    [property: JsonPropertyName("destinationUrl")] string DestinationUrl
);