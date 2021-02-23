using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;

namespace PersonalCapital.Request
{
    [SuppressMessage("ReSharper", "InconsistentNaming")]
    public class UpdateTransactionRequest
    {
        public UpdateTransactionRequest(List<long> userTransactionIds, string description, long transactionCategoryId,
            bool isDuplicate, string customTags)
        {
            this.userTransactionIds = $"[{string.Join(",", userTransactionIds)}]";
            this.description = description;
            this.transactionCategoryId = transactionCategoryId;
            this.isDuplicate = isDuplicate.ToString().ToLower();
            this.customTags = customTags;
        }

        public string userTransactionIds { get; }

        public string description { get; }

        public long transactionCategoryId { get; }

        public string isDuplicate { get; }

        public string customTags { get; }
    }
}