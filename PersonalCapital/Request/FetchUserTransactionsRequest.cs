using System;

namespace PersonalCapital.Request {
    public class FetchUserTransactionsRequest {
        public FetchUserTransactionsRequest(DateTime startDate, DateTime endDate) {
            this.startDate = startDate.ToString("yyyy-MM-dd");
            this.endDate = endDate.ToString("yyyy-MM-dd");
        }

        /* Serialized Values */
        // ReSharper disable once InconsistentNaming
        public string startDate { get; private set; }
        // ReSharper disable once InconsistentNaming
        public string endDate { get; private set; }
    }
}