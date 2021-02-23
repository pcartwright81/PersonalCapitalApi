using System.Collections.Generic;
using Newtonsoft.Json;

namespace PersonalCapital.Response
{
    public class CustomTags
    {
        public List<object> systemTags { get; set; }
        public List<object> userTags { get; set; }
    }

    public class UpdateTransactionResponseData
    {
        public string symbol { get; set; }
        public bool isInterest { get; set; }
        public double netCost { get; set; }
        public string cusipNumber { get; set; }
        public string accountName { get; set; }
        public string description { get; set; }
        public string memo { get; set; }
        public bool isCredit { get; set; }
        public CustomTags customTags { get; set; }
        public bool isEditable { get; set; }
        public bool isCashOut { get; set; }
        public string merchantId { get; set; }
        public double price { get; set; }
        public string holdingType { get; set; }
        public string lotHandling { get; set; }
        public string customReason { get; set; }
        public bool hasSplits { get; set; }
        public long userTransactionId { get; set; }
        public string currency { get; set; }
        public bool isDuplicate { get; set; }
        public string resultType { get; set; }
        public string originalDescription { get; set; }
        public bool isSpending { get; set; }
        public double amount { get; set; }
        public string checkNumber { get; set; }
        public int transactionTypeId { get; set; }
        public bool isIncome { get; set; }
        public bool includeInCashManager { get; set; }
        public string merchant { get; set; }
        public bool isNew { get; set; }
        public bool isCashIn { get; set; }
        public string transactionDate { get; set; }
        public string transactionType { get; set; }
        public string accountId { get; set; }
        public double originalAmount { get; set; }
        public bool isCost { get; set; }
        public int userAccountId { get; set; }
        public string simpleDescription { get; set; }
        public double runningBalance { get; set; }
        public bool hasViewed { get; set; }
        public int categoryId { get; set; }
        public string status { get; set; }
    }

    public class UpdateTransactionResponse
    {
        [JsonProperty(PropertyName = "spHeader")]
        public HeaderResponse Header { get; set; }

        [JsonProperty(PropertyName = "spData")]
        public List<UpdateTransactionResponseData> Data { get; set; }
    }
}
