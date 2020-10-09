using System.Collections.Generic;
using Newtonsoft.Json;

namespace PersonalCapital.Response
{
    public class FetchUserTransactionsData
    {
        [JsonProperty(PropertyName = "intervalType")]
        public string IntervalType { get; set; }

        [JsonProperty(PropertyName = "endDate")]
        public string EndDate { get; set; }

        [JsonProperty(PropertyName = "startDate")]
        public string StartDate { get; set; }

        [JsonProperty(PropertyName = "moneyIn")]
        public decimal MoneyIn { get; set; }

        [JsonProperty(PropertyName = "netCashflow")]
        public decimal NetCashflow { get; set; }

        [JsonProperty(PropertyName = "averageOut")]
        public decimal AverageOut { get; set; }

        [JsonProperty(PropertyName = "moneyOut")]
        public decimal MoneyOut { get; set; }

        [JsonProperty(PropertyName = "averageIn")]
        public decimal AverageIn { get; set; }

        [JsonProperty(PropertyName = "transactions")]
        public List<Transaction> Transactions { get; set; }
    }
}