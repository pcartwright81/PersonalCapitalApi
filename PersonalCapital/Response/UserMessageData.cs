using System;
using System.Collections.Generic;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;

namespace PersonalCapital.Response;



// 4. User Message (Your specific record, corrected)
public record UserMessageData(
    [property: JsonProperty("template")] string Template,
    [property: JsonProperty("summary")] string Summary,
    [property: JsonProperty("title")] string Title,

    // CRITICAL FIX: Must be Long (Int64) to handle 1000051150982
    [property: JsonProperty("userMessageId")] long UserMessageId,

    [property: JsonProperty("priority")] string Priority,
    [property: JsonProperty("component")] string Component,
    [property: JsonProperty("viewTemplate")] string ViewTemplate,
    [property: JsonProperty("rank")] int Rank,
    [property: JsonProperty("isValid")] bool IsValid,
    [property: JsonProperty("isStale")] bool IsStale,

    // Lists
    [property: JsonProperty("displayLocations")] List<string> DisplayLocations,
    [property: JsonProperty("resources")] List<ResourceData> Resources,
    [property: JsonProperty("action")] List<ActionData> Action,

    // Dates
    // Note: UnixDateTimeConverter is standard in Newtonsoft. 
    // If "UnixEpochConverter" is a custom class of yours, change the type inside typeof().
    [property: JsonConverter(typeof(UnixDateTimeConverter))]
    [property: JsonProperty("updatedTime", NullValueHandling = NullValueHandling.Ignore)]
    DateTime? UpdatedTime,

    [property: JsonConverter(typeof(UnixDateTimeConverter))]
    [property: JsonProperty("lastViewedTime", NullValueHandling = NullValueHandling.Ignore)]
    DateTime? LastViewedTime,

    [property: JsonConverter(typeof(UnixDateTimeConverter))]
    [property: JsonProperty("createdTime", NullValueHandling = NullValueHandling.Ignore)]
    DateTime? CreatedTime
);
