﻿using Newtonsoft.Json;
using System.Collections.Generic;

namespace PersonalCapital.Response;

public record FetchAccountsData(
    [property: JsonProperty(PropertyName = "creditCardAccountsTotal")] decimal CreditCardAccountsTotal,
    [property: JsonProperty(PropertyName = "assets")] decimal Assets,
    [property: JsonProperty(PropertyName = "otherLiabilitiesAccountsTotal")] decimal OtherLiabilitiesAccountsTotal,
    [property: JsonProperty(PropertyName = "cashAccountsTotal")] decimal CashAccountsTotal,
    [property: JsonProperty(PropertyName = "liabilities")] decimal Liabilities,
    [property: JsonProperty(PropertyName = "networth")] decimal Networth,
    [property: JsonProperty(PropertyName = "investmentAccountsTotal")] decimal InvestmentAccountsTotal,
    [property: JsonProperty(PropertyName = "mortgageAccountsTotal")] decimal MortgageAccountsTotal,
    [property: JsonProperty(PropertyName = "loanAccountsTotal")] decimal LoanAccountsTotal,
    [property: JsonProperty(PropertyName = "otherAssetAccountsTotal")] decimal OtherAssetAccountsTotal,
    [property: JsonProperty(PropertyName = "accounts")] List<Account> Accounts
);