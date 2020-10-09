using Newtonsoft.Json;

namespace PersonalCapital.Response
{
    public class Account
    {
        [JsonProperty(PropertyName = "priorBalance")]
        public decimal PriorBalance { get; set; }

        [JsonProperty(PropertyName = "originalName")]
        public string OriginalName { get; set; }

        [JsonProperty(PropertyName = "balance")]
        public decimal Balance { get; set; }

        [JsonProperty(PropertyName = "accountId")]
        public string AccountId { get; set; }

        [JsonProperty(PropertyName = "userAccountId")]
        public long UserAccountId { get; set; }

        [JsonProperty(PropertyName = "name")] public string Name { get; set; }

        [JsonProperty(PropertyName = "firmName")]
        public string FirmName { get; set; }

        [JsonProperty(PropertyName = "productType")]
        public string ProductType { get; set; }

        [JsonProperty(PropertyName = "accountType")]
        public string AccountType { get; set; }

        [JsonProperty(PropertyName = "accountTypeGroup")]
        public string AccountTypeGroup { get; set; }

        [JsonProperty(PropertyName = "fundFees")]
        public decimal FundFees { get; set; }

        [JsonProperty(PropertyName = "feesPerYear")]
        public string FeesPerYear { get; set; } // TODO: Convert to decimal

        [JsonProperty(PropertyName = "totalFee")]
        public decimal TotalFee { get; set; }

        [JsonProperty(PropertyName = "originalFirmName")]
        public string OriginalFirmName { get; set; }

        #region CREDIT

        [JsonProperty(PropertyName = "lastPayment")]
        public decimal LastPayment { get; set; }

        [JsonProperty(PropertyName = "runningBalance")]
        public decimal RunningBalance { get; set; }

        [JsonProperty(PropertyName = "availableCredit")]
        public decimal AvailableCredit { get; set; }

        [JsonProperty(PropertyName = "creditLimit")]
        public decimal CreditLimit { get; set; }

        #endregion
    }
}