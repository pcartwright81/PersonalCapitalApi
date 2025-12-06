using System.Collections.Generic;
using System.Net;

namespace PersonalCapital.Models;

public class OfflineSessionData
{
    public required string Csrf { get; set; }
    public required List<Cookie> Cookies { get; set; }
}
