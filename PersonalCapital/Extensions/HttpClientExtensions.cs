using System.Net.Http;
using System.Threading.Tasks;

namespace PersonalCapital.Extensions
{
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
    }
}