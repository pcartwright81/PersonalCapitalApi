using System;
using Newtonsoft.Json;

namespace PersonalCapital.Response
{
    public class Transaction
    {
        //Original amount might be "NaN" which is mapped to Null
        private decimal? _originalAmount;

        private string _originalAmountString;

        [JsonProperty(PropertyName = "symbol")]
        public string Symbol { get; set; }

        [JsonProperty(PropertyName = "isInterest")]
        public bool IsInterest { get; set; }

        [JsonProperty(PropertyName = "netCost")]
        public decimal NetCost { get; set; }

        // ReSharper disable StringLiteralTypo
        [JsonProperty(PropertyName = "cusipNumber")]
        // ReSharper restore StringLiteralTypo
        public string CusipNumber { get; set; }

        [JsonProperty(PropertyName = "accountName")]
        public string AccountName { get; set; }

        [JsonProperty(PropertyName = "description")]
        public string Description { get; set; }

        [JsonProperty(PropertyName = "memo")] public string Memo { get; set; }

        [JsonProperty(PropertyName = "isCredit")]
        public bool IsCredit { get; set; }

        [JsonProperty(PropertyName = "isEditable")]
        public bool IsEditable { get; set; }

        [JsonProperty(PropertyName = "isCashOut")]
        public bool IsCashOut { get; set; }

        [JsonProperty(PropertyName = "merchantId")]
        public string MerchantId { get; set; }

        [JsonProperty(PropertyName = "price")] public decimal Price { get; set; }

        [JsonProperty(PropertyName = "holdingType")]
        public string HoldingType { get; set; }

        [JsonProperty(PropertyName = "lotHandling")]
        public string LotHandling { get; set; }

        [JsonProperty(PropertyName = "userTransactionId")]
        public long UserTransactionId { get; set; }

        [JsonProperty(PropertyName = "currency")]
        public string Currency { get; set; }

        [JsonProperty(PropertyName = "isDuplicate")]
        public bool IsDuplicate { get; set; }

        [JsonProperty(PropertyName = "resultType")]
        public string ResultType { get; set; }

        [JsonProperty(PropertyName = "originalDescription")]
        public string OriginalDescription { get; set; }

        [JsonProperty(PropertyName = "isSpending")]
        public bool IsSpending { get; set; }

        [JsonProperty(PropertyName = "amount")]
        public decimal Amount { get; set; }

        [JsonProperty(PropertyName = "checkNumber")]
        public string CheckNumber { get; set; }

        [JsonProperty(PropertyName = "isIncome")]
        public bool IsIncome { get; set; }

        [JsonProperty(PropertyName = "includeInCashManager")]
        public bool IncludeInCashManager { get; set; }

        [JsonProperty(PropertyName = "merchant")]
        public string Merchant { get; set; }

        [JsonProperty(PropertyName = "isNew")] public bool IsNew { get; set; }

        [JsonProperty(PropertyName = "isCashIn")]
        public bool IsCashIn { get; set; }

        [JsonProperty(PropertyName = "transactionDate")]
        public DateTime TransactionDate { get; set; }

        [JsonProperty(PropertyName = "transactionType")]
        public string TransactionType { get; set; }

        [JsonProperty(PropertyName = "accountId")]
        public string AccountId { get; set; }

        [JsonProperty(PropertyName = "originalAmount")]
        public string OriginalAmountString
        {
            get => _originalAmountString;
            set
            {
                _originalAmountString = value;
                // Parse to decimal as originalAmount, otherwise set to null
                if (decimal.TryParse(value, out var decimalValue))
                    _originalAmount = decimalValue;
                else
                    _originalAmount = null;
            }
        }

        public decimal? OriginalAmount
        {
            get => _originalAmount;
            set
            {
                _originalAmount = value;
                _originalAmountString = value?.ToString();
            }
        }

        [JsonProperty(PropertyName = "isCost")]
        public bool IsCost { get; set; }

        [JsonProperty(PropertyName = "userAccountId")]
        public long UserAccountId { get; set; }

        [JsonProperty(PropertyName = "simpleDescription")]
        public string SimpleDescription { get; set; }

        [JsonProperty(PropertyName = "catKeyword")]
        public string CatKeyword { get; set; }

        [JsonProperty(PropertyName = "runningBalance")]
        public decimal RunningBalance { get; set; }

        [JsonProperty(PropertyName = "hasViewed")]
        public bool HasViewed { get; set; }

        [JsonProperty(PropertyName = "categoryId")]
        public int CategoryId { get; set; }

        [JsonProperty(PropertyName = "status")]
        public string Status { get; set; }
    }
}