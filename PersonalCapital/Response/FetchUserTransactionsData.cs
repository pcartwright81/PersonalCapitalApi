﻿using Newtonsoft.Json;
using System.Collections.Generic;

namespace PersonalCapital.Response;

public record FetchUserTransactionsData(
    [property: JsonProperty(PropertyName = "intervalType")] string IntervalType,
    [property: JsonProperty(PropertyName = "endDate")] string EndDate,
    [property: JsonProperty(PropertyName = "startDate")] string StartDate,
    [property: JsonProperty(PropertyName = "moneyIn")] decimal MoneyIn,
    [property: JsonProperty(PropertyName = "netCashflow")] decimal NetCashflow,
    [property: JsonProperty(PropertyName = "averageOut")] decimal AverageOut,
    [property: JsonProperty(PropertyName = "moneyOut")] decimal MoneyOut,
    [property: JsonProperty(PropertyName = "averageIn")] decimal AverageIn,
    [property: JsonProperty(PropertyName = "transactions")] List<Transaction> Transactions
);