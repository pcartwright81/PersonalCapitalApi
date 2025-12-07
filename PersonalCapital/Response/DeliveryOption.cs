namespace PersonalCapital.Response;

public class DeliveryOption
{
    public string DeliveryType { get; set; } = string.Empty;
    public string? DeliveryPhoneType { get; set; }
    public string DeliveryMask { get; set; } = string.Empty;
    public string? OriginalValue { get; set; }
}