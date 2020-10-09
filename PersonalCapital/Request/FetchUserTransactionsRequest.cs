using System;
using System.Diagnostics.CodeAnalysis;

namespace PersonalCapital.Request
{
    public class FetchUserTransactionsRequest
    {
        public FetchUserTransactionsRequest(DateTime startDate, DateTime endDate)
        {
            this.startDate = startDate.ToString("yyyy-MM-dd");
            this.endDate = endDate.ToString("yyyy-MM-dd");
        }

        /* Serialized Values */
        [SuppressMessage("ReSharper", "InconsistentNaming")]
        public string startDate { get; }

        [SuppressMessage("ReSharper", "InconsistentNaming")]
        public string endDate { get; }
    }
}