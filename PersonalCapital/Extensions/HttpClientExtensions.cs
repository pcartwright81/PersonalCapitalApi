using System.Net.Http;
using System.Threading.Tasks;

namespace PersonalCapital.Extensions;

public static class HttpClientExtensions
{
    /// <summary>
    ///     URL encodes the data object and posts to the supplied url
    /// </summary>
    /// <param name="client"></param>
    /// <param name="url"></param>
    /// <param name="data"></param>
    /// <returns></returns>
    public static Task<HttpResponseMessage> PostHttpEncodedData(this HttpClient client, string url, object data)
    {
        return client.PostAsync(url, new FormUrlEncodedContent(data.ToKeyValue()));
    }

    /// <summary>
    ///     URL encodes the data object and posts to the supplied url as Multipart Form Data
    /// </summary>
    public static Task<HttpResponseMessage> PostMultipartData(this HttpClient client, string url, object data)
    {
        var content = new MultipartFormDataContent();
        var dictionary = data.ToKeyValue();

        if (dictionary != null)
        {
            foreach (var pair in dictionary)
            {
                content.Add(new StringContent(pair.Value), pair.Key);
            }
        }

        return client.PostAsync(url, content);
    }
}