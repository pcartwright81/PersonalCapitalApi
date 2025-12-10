using System.Collections.Generic;
using System.Net.Http;
using System.Threading.Tasks;

namespace PersonalCapital.Extensions;

public static class HttpClientExtensions
{
    /// <summary>
    /// URL encodes the data object and posts to the supplied url.
    /// </summary>
    public static Task<HttpResponseMessage> PostHttpEncodedData(this HttpClient client, string url, object? data)
    {
        var keyValueData = data?.ToKeyValue() ?? new Dictionary<string, string>();
        return client.PostAsync(url, new FormUrlEncodedContent(keyValueData));
    }

    /// <summary>
    /// Posts the data object to the supplied url as Multipart Form Data.
    /// </summary>
    public static Task<HttpResponseMessage> PostMultipartData(this HttpClient client, string url, object? data)
    {
        var content = new MultipartFormDataContent();
        var dictionary = data?.ToKeyValue();

        if (dictionary != null)
        {
            foreach (var (key, value) in dictionary)
            {
                content.Add(new StringContent(value), key);
            }
        }

        return client.PostAsync(url, content);
    }
}
