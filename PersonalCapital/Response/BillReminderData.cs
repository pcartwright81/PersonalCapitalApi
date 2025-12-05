﻿using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using PersonalCapital.Extensions;
using System;

namespace PersonalCapital.Response;

public record BillReminderData(
    [property: JsonProperty("payee")] string Payee,
    [property: JsonProperty("amountDue")] double AmountDue,
    [property: JsonProperty("statementBalance")] double StatementBalance,
    [property: JsonProperty("minAmountDue")] double MinAmountDue,
    [property: JsonProperty("accountName")] string AccountName,
    [property: JsonProperty("userAccountId")] int UserAccountId,
    [property: JsonConverter(typeof(UnixDateTimeConverter))]
    [property: JsonProperty("dueDate")]
    DateTime DueDate,
    [property: JsonConverter(typeof(UnixDateTimeConverter))]
    [property: JsonProperty("lastPaymentDate")]
    DateTime LastPaymentDate,
    [property: JsonProperty("lastPaymentAmount")] object LastPaymentAmount,
    [property: JsonProperty("billPaymentId")] int BillPaymentId,
    [property: JsonProperty("status")] string Status
);
