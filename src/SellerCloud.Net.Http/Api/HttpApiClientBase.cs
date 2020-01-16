using SellerCloud.Net.Http.Extensions;
using System.Net.Http;

namespace SellerCloud.Net.Http.Api
{
    public abstract class HttpApiClientBase
    {
        private readonly HttpClient client;

        protected HttpApiClientBase(HttpClient client)
        {
            this.client = client;
        }

        protected HttpRequestBuilder HttpGet(string baseUri, string route)
        {
            string endpoint = baseUri.WithRoute(route);

            return new HttpRequestBuilder(this.client, endpoint, HttpMethod.Get);
        }

        protected HttpRequestBuilder HttpPost(string baseUri, string route)
        {
            string endpoint = baseUri.WithRoute(route);

            return new HttpRequestBuilder(this.client, endpoint, HttpMethod.Post);
        }

        protected HttpRequestBuilder HttpPost<TBody>(string baseUri, string route, TBody body)
        {
            string endpoint = baseUri.WithRoute(route);

            return new HttpRequestBuilder(this.client, endpoint, HttpMethod.Post, body);
        }

        protected HttpRequestBuilder HttpPut(string baseUri, string route)
        {
            string endpoint = baseUri.WithRoute(route);

            return new HttpRequestBuilder(this.client, endpoint, HttpMethod.Put);
        }

        protected HttpRequestBuilder HttpPut<TBody>(string baseUri, string route, TBody body)
        {
            string endpoint = baseUri.WithRoute(route);

            return new HttpRequestBuilder(this.client, endpoint, HttpMethod.Put, body);
        }

        protected HttpRequestBuilder HttpDelete(string baseUri, string route)
        {
            string endpoint = baseUri.WithRoute(route);

            return new HttpRequestBuilder(this.client, endpoint, HttpMethod.Delete);
        }

        protected HttpRequestBuilder HttpDelete<TBody>(string baseUri, string route, TBody body)
        {
            string endpoint = baseUri.WithRoute(route);

            return new HttpRequestBuilder(this.client, endpoint, HttpMethod.Delete, body);
        }

        protected HttpRequestBuilder HttpPatch<TBody>(string baseUri, string route, TBody body)
        {
            string endpoint = baseUri.WithRoute(route);

            return new HttpRequestBuilder(this.client, endpoint, new HttpMethod("PATCH"), body);
        }
    }
}
