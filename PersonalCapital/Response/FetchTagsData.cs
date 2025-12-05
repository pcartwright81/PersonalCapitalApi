﻿namespace PersonalCapital.Response;

using Newtonsoft.Json;

public record FetchTagsData(
    [property: JsonProperty("tagId")] long TagId,
    [property: JsonProperty("tagName")] string TagName
)
{
    public string DataProvider { get; set; } = "Personal Capital";
}