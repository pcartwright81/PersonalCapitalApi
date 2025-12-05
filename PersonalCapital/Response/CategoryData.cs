﻿﻿﻿using Newtonsoft.Json;

namespace PersonalCapital.Response;

public record CategoryData(
    [property: JsonProperty(PropertyName = "isEditable")] bool IsEditable,
    [property: JsonProperty(PropertyName = "name")] string Name,
    [property: JsonProperty(PropertyName = "isCustom")] bool IsCustom,
    [property: JsonProperty(PropertyName = "isOverride")] bool IsOverride,
    [property: JsonProperty(PropertyName = "transactionCategoryId")] int TransactionCategoryId,
    [property: JsonProperty(PropertyName = "shortDescription")] string ShortDescription,
    [property: JsonProperty(PropertyName = "type")] string Type
)
{
    public string DataProvider { get; init; } = "Personal Capital";

    public override string ToString()
    {
        return Name;
    }
}