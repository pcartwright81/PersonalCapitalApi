using Newtonsoft.Json.Linq;
using System.Collections.Generic;
using System.ComponentModel;
using System.Dynamic;
using System.Globalization;
using System.Linq;

namespace PersonalCapital.Extensions;

public static class ObjectExtensions
{
    /// <summary>
    /// Converts the given data object to a Dictionary using property names as keys.
    /// The keys are compliant for posting as form data.
    /// </summary>
    public static IDictionary<string, string>? ToKeyValue(this object? data)
    {
        if (data == null) return null;

        var token = data as JToken ?? JToken.FromObject(data);

        if (token.HasValues)
        {
            return token.Children()
                .Select(child => child.ToKeyValue())
                .OfType<IDictionary<string, string>>()
                .SelectMany(childContent => childContent)
                .ToDictionary(kvp => kvp.Key, kvp => kvp.Value);
        }

        if (token is not JValue jValue || jValue.Value == null)
            return null;

        var value = jValue.Type == JTokenType.Date
            ? jValue.ToString("o", CultureInfo.InvariantCulture)
            : jValue.ToString(CultureInfo.InvariantCulture);

        return new Dictionary<string, string> { { token.Path, value } };
    }

    /// <summary>
    /// Converts the supplied object to an ExpandoObject to be used as a dynamic.
    /// </summary>
    public static dynamic ToDynamic(this object value)
    {
        IDictionary<string, object?> expando = new ExpandoObject();

        foreach (PropertyDescriptor property in TypeDescriptor.GetProperties(value.GetType()))
        {
            expando.Add(property.Name, property.GetValue(value));
        }

        return expando;
    }
}
