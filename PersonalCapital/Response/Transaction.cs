using Newtonsoft.Json;
using System;
using PersonalCapital.Extensions;

namespace PersonalCapital.Response;

public record Transaction(
    [property: JsonProperty(PropertyName = "symbol")] string Symbol,
    [property: JsonProperty(PropertyName = "isInterest")] bool IsInterest,
    [property: JsonProperty(PropertyName = "netCost")] decimal NetCost,
    [property: JsonProperty(PropertyName = "cusipNumber")] string CusipNumber,
    [property: JsonProperty(PropertyName = "accountName")] string AccountName,
    [property: JsonProperty(PropertyName = "description")] string Description,
    [property: JsonProperty(PropertyName = "memo")] string Memo,
    [property: JsonProperty(PropertyName = "isCredit")] bool IsCredit,
    [property: JsonProperty(PropertyName = "isEditable")] bool IsEditable,
    [property: JsonProperty(PropertyName = "isCashOut")] bool IsCashOut,
    [property: JsonProperty(PropertyName = "merchantId")] string MerchantId,
    [property: JsonProperty(PropertyName = "price")] decimal Price,
    [property: JsonProperty(PropertyName = "holdingType")] string HoldingType,
    [property: JsonProperty(PropertyName = "lotHandling")] string LotHandling,
    [property: JsonProperty(PropertyName = "userTransactionId")] long UserTransactionId,
    [property: JsonProperty(PropertyName = "currency")] string Currency,
    [property: JsonProperty(PropertyName = "isDuplicate")] bool IsDuplicate,
    [property: JsonProperty(PropertyName = "resultType")] string ResultType,
    [property: JsonProperty(PropertyName = "originalDescription")] string OriginalDescription,
    [property: JsonProperty(PropertyName = "isSpending")] bool IsSpending,
    [property: JsonProperty(PropertyName = "amount")] decimal Amount,
    [property: JsonProperty(PropertyName = "checkNumber")] string CheckNumber,
    [property: JsonProperty(PropertyName = "isIncome")] bool IsIncome,
    [property: JsonProperty(PropertyName = "includeInCashManager")] bool IncludeInCashManager,
    [property: JsonProperty(PropertyName = "merchant")] string Merchant,
    [property: JsonProperty(PropertyName = "isNew")] bool IsNew,
    [property: JsonProperty(PropertyName = "isCashIn")] bool IsCashIn,
    [property: JsonProperty(PropertyName = "transactionDate")] DateTime TransactionDate,
    [property: JsonProperty(PropertyName = "transactionType")] string TransactionType,
    [property: JsonProperty(PropertyName = "accountId")] string AccountId,
    [property: JsonConverter(typeof(NanToNullConverter))] [property: JsonProperty(PropertyName = "originalAmount")] decimal? OriginalAmount,
    [property: JsonProperty(PropertyName = "isCost")] bool IsCost,
    [property: JsonProperty(PropertyName = "userAccountId")] long UserAccountId,
    [property: JsonProperty(PropertyName = "simpleDescription")] string SimpleDescription,
    [property: JsonProperty(PropertyName = "catKeyword")] string CatKeyword,
    [property: JsonProperty(PropertyName = "runningBalance")] decimal RunningBalance,
    [property: JsonProperty(PropertyName = "hasViewed")] bool HasViewed,
    [property: JsonProperty(PropertyName = "categoryId")] int CategoryId,
    [property: JsonProperty(PropertyName = "status")] string Status,
    [property: JsonProperty(PropertyName = "customTags")] CustomTags CustomTags
);