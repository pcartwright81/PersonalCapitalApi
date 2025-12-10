namespace PersonalCapital.Response;

using System.Collections.Generic;

public class DeliveryOptionsResponse
{
    public string? Destination { get; set; }
    public string? FlowName { get; set; }
    public List<DeliveryOption> DeliverySet { get; set; } = new();
    public bool EditContactInfoAllowed { get; set; }
    public bool IntlPhoneSupportAllowed { get; set; }
}
