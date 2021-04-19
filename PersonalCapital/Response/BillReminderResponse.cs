using Newtonsoft.Json;
using System.Collections.Generic;

namespace PersonalCapital.Response
{
    public class BillReminderResponse
    {
        [JsonProperty(PropertyName = "spHeader")]
        public HeaderResponse Header { get; set; }

        [JsonProperty(PropertyName = "spData")]
        public List<BillReminderData> Data { get; set; }
    }
}
