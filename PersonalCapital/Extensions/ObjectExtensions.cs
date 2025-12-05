using Newtonsoft.Json.Linq;
using System.Collections.Generic;
using System.ComponentModel;
using System.Dynamic;
using System.Globalization;
using System.Linq;
using JToken = Newtonsoft.Json.Linq.JToken;

namespace PersonalCapital.Extensions;

public static class ObjectExtensions
{
    /// <summary>
    ///     Helper method to convert the given data object to a Dictionary using the property names as keys.
    ///     The keys are compliant for posting as form data.
    /// </summary>
    /// <param name="data"></param>
    /// <returns></returns>
    public static IDictionary<string, string> ToKeyValue(this object data)
    {
        if (data == null) return null;

        var token = data as JToken ?? JObject.FromObject(data);

        if (token.HasValues)
            return token.Children()
                .Select(child => child.ToKeyValue())
                .Where(childContent => childContent != null)
                .Aggregate(new Dictionary<string, string>(),
                    (current, childContent) => current.Concat(childContent).ToDictionary(k => k.Key, v => v.Value));

        var jValue = token as JValue;
        if (jValue?.Value == null) return null;

        var value = jValue.Type == JTokenType.Date
            ? jValue.ToString("o", CultureInfo.InvariantCulture)
            : jValue.ToString(CultureInfo.InvariantCulture);

        return new Dictionary<string, string> { { token.Path, value } };
    }

    /// <summary>
    ///     Converts the supplied object to an ExpandoObject to be used as a dynamic
    /// </summary>
    /// <param name="value"></param>
    /// <returns></returns>
    public static dynamic ToDynamic(this object value)
    {
        IDictionary<string, object> expando = new ExpandoObject();

        foreach (PropertyDescriptor property in TypeDescriptor.GetProperties(value.GetType()))
            expando.Add(property.Name, property.GetValue(value));

        return (ExpandoObject)expando;
    }
}