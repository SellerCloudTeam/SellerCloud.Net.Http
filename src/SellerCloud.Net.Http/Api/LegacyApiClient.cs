using SellerCloud.Net.Http.Extensions;
using System.Net;
using System.Net.Http;

namespace SellerCloud.Net.Http.Api
{
    public class LegacyApiClient : ILegacyApiClient
    {
        private readonly string baseUri;

        public LegacyApiClient(string baseUri)
        {
            this.baseUri = baseUri;
        }

        public WebRequestBuilder HttpGet(string route)
        {
            string endpoint = this.baseUri.WithRoute(route);

            return new WebRequestBuilder(endpoint, HttpMethod.Get);
        }

        public WebRequestBuilder HttpPost(string route)
        {
            string endpoint = this.baseUri.WithRoute(route);

            return new WebRequestBuilder(endpoint, HttpMethod.Post);
        }

        public WebRequestBuilder HttpPost<TBody>(string route, TBody body)
            where TBody : notnull
        {
            string endpoint = this.baseUri.WithRoute(route);

            return new WebRequestBuilder(endpoint, HttpMethod.Post, body);
        }

        public WebRequestBuilder HttpPut(string route)
        {
            string endpoint = this.baseUri.WithRoute(route);

            return new WebRequestBuilder(endpoint, HttpMethod.Put);
        }

        public WebRequestBuilder HttpPut<TBody>(string route, TBody body)
            where TBody : notnull
        {
            string endpoint = this.baseUri.WithRoute(route);

            return new WebRequestBuilder(endpoint, HttpMethod.Put, body);
        }

        public WebRequestBuilder HttpDelete(string route)
        {
            string endpoint = this.baseUri.WithRoute(route);

            return new WebRequestBuilder(endpoint, HttpMethod.Delete);
        }

        public WebRequestBuilder HttpDelete<TBody>(string route, TBody body)
            where TBody : notnull
        {
            string endpoint = this.baseUri.WithRoute(route);

            return new WebRequestBuilder(endpoint, HttpMethod.Delete, body);
        }

        public WebRequestBuilder HttpPatch(string route)
        {
            string endpoint = this.baseUri.WithRoute(route);

            return new WebRequestBuilder(endpoint, new HttpMethod("PATCH"));
        }

        public WebRequestBuilder HttpPatch<TBody>(string route, TBody body)
            where TBody : notnull
        {
            string endpoint = this.baseUri.WithRoute(route);

            return new WebRequestBuilder(endpoint, new HttpMethod("PATCH"), body);
        }
    }
}