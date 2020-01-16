using SellerCloud.Net.Http.Models;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;

namespace SellerCloud.Net.Http.Extensions
{
    public static class HttpClientExtensions
    {
        public static async Task<HttpResponseMessage> GetAsync(this HttpClient client, string endpoint, AuthToken token, CancellationToken cancellationToken)
        {
            HttpMethod method = HttpMethod.Get;
            HttpRequestMessage request = HttpHelper.ConstructHttpRequestMessage(endpoint, method, token);
            HttpResponseMessage response = await client.SendAsync(request, cancellationToken);

            return response;
        }

        public static async Task<HttpResponseMessage> PostAsJsonAsync(this HttpClient client, string endpoint, object data, CancellationToken cancellationToken)
        {
            HttpMethod method = HttpMethod.Post;
            HttpRequestMessage request = HttpHelper.ConstructHttpRequestMessageWithContent(endpoint, method, data, token: null);
            HttpResponseMessage response = await client.SendAsync(request, cancellationToken);

            return response;
        }

        public static async Task<HttpResponseMessage> PostAsJsonAsync(this HttpClient client, string endpoint, object data, AuthToken token, CancellationToken cancellationToken)
        {
            HttpMethod method = HttpMethod.Post;
            HttpRequestMessage request = HttpHelper.ConstructHttpRequestMessageWithContent(endpoint, method, data, token);
            HttpResponseMessage response = await client.SendAsync(request, cancellationToken);

            return response;
        }
    }
}
