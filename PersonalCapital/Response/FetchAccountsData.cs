using Newtonsoft.Json;
using System.Collections.Generic;

namespace PersonalCapital.Response {
    public class FetchAccountsData {
        [JsonProperty(PropertyName = "creditCardAccountsTotal")]
        public decimal CreditCardAccountsTotal { get; set; }

        [JsonProperty(PropertyName = "assets")]
        public decimal Aassets { get; set; }

        [JsonProperty(PropertyName = "otherLiabilitiesAccountsTotal")]
        public decimal OtherLiabilitiesAccountsTotal { get; set; }

        [JsonProperty(PropertyName = "cashAccountsTotal")]
        public decimal CashAccountsTotal { get; set; }

        [JsonProperty(PropertyName = "liabilities")]
        public decimal Liabilities { get; set; }

        [JsonProperty(PropertyName = "networth")]
        public decimal Networth { get; set; }

        [JsonProperty(PropertyName = "investmentAccountsTotal")]
        public decimal InvestmentAccountsTotal { get; set; }

        [JsonProperty(PropertyName = "mortgageAccountsTotal")]
        public decimal MortgageAccountsTotal { get; set; }

        [JsonProperty(PropertyName = "loanAccountsTotal")]
        public decimal LoanAccountsTotal { get; set; }

        [JsonProperty(PropertyName = "otherAssetAccountsTotal")]
        public decimal OtherAssetAccountsTotal { get; set; }

        [JsonProperty(PropertyName = "accounts")]
        public List<Account> Accounts { get; set; }
    }
}
