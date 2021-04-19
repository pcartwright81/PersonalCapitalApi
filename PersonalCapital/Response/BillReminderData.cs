using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using PersonalCapital.Extensions;
using System;

namespace PersonalCapital.Response
{
    public class BillReminderData
    {
        [JsonProperty("payee")]
        public string Payee { get; set; }

        [JsonProperty("amountDue")]
        public double AmountDue { get; set; }

        [JsonProperty("statementBalance")]
        public double StatementBalance { get; set; }

        [JsonProperty("minAmountDue")]
        public double MinAmountDue { get; set; }

        [JsonProperty("accountName")]
        public string AccountName { get; set; }

        [JsonProperty("userAccountId")]
        public int UserAccountId { get; set; }

        [JsonConverter(typeof(UnixEpochConverter))]
        [JsonProperty("dueDate")]       
        public DateTime DueDate { get; set; }

        [JsonConverter(typeof(UnixEpochConverter))]
        [JsonProperty("lastPaymentDate")]
        public DateTime LastPaymentDate { get; set; }

        [JsonProperty("lastPaymentAmount")]
        public object LastPaymentAmount { get; set; }

        [JsonProperty("billPaymentId")]
        public int BillPaymentId { get; set; }

        [JsonProperty("status")]
        public string Status { get; set; }
    }
}
