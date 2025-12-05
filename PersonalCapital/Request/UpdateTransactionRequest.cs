using Newtonsoft.Json;
using System.Collections.Generic;

namespace PersonalCapital.Request;

public record UpdateTransactionRequest(
    [property: JsonProperty("userTransactionIds")] IEnumerable<long> UserTransactionIds,
    [property: JsonProperty("description")] string Description,
    [property: JsonProperty("transactionCategoryId")] long TransactionCategoryId,
    [property: JsonProperty("isDuplicate")] bool IsDuplicate,
    [property: JsonProperty("customTags")] string CustomTags
);