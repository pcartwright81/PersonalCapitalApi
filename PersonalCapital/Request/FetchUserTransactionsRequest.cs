using Newtonsoft.Json;
using System;

namespace PersonalCapital.Request;

public record FetchUserTransactionsRequest(
    [property: JsonProperty("startDate")] string StartDate,
    [property: JsonProperty("endDate")] string EndDate
)
{
    public FetchUserTransactionsRequest(DateTime startDate, DateTime endDate)
        : this(startDate.ToString("yyyy-MM-dd"), endDate.ToString("yyyy-MM-dd"))
    {
    }
}