﻿using Newtonsoft.Json;

namespace PersonalCapital.Response;

public record Account(
    [property: JsonProperty(PropertyName = "priorBalance")] decimal PriorBalance,
    [property: JsonProperty(PropertyName = "originalName")] string OriginalName,
    [property: JsonProperty(PropertyName = "balance")] decimal Balance,
    [property: JsonProperty(PropertyName = "accountId")] string AccountId,
    [property: JsonProperty(PropertyName = "userAccountId")] long UserAccountId,
    [property: JsonProperty(PropertyName = "name")] string Name,
    [property: JsonProperty(PropertyName = "firmName")] string FirmName,
    [property: JsonProperty(PropertyName = "productType")] string ProductType,
    [property: JsonProperty(PropertyName = "accountType")] string AccountType,
    [property: JsonProperty(PropertyName = "accountTypeGroup")] string AccountTypeGroup,
    [property: JsonProperty(PropertyName = "fundFees")] decimal FundFees,
    [property: JsonProperty(PropertyName = "feesPerYear")] string FeesPerYear,
    [property: JsonProperty(PropertyName = "totalFee")] decimal TotalFee,
    [property: JsonProperty(PropertyName = "originalFirmName")] string OriginalFirmName,
    [property: JsonProperty(PropertyName = "lastPayment")] decimal LastPayment,
    [property: JsonProperty(PropertyName = "runningBalance")] decimal RunningBalance,
    [property: JsonProperty(PropertyName = "availableCredit")] decimal AvailableCredit,
    [property: JsonProperty(PropertyName = "creditLimit")] decimal CreditLimit
);