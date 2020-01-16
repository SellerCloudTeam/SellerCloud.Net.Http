using SellerCloud.Net.Http.Extensions;
using System.Net;
using System.Net.Http;

namespace SellerCloud.Net.Http.Api
{
    public abstract class LegacyApiClientBase
    {
        protected WebRequestBuilder HttpGet(string baseUri, string route)
        {
            string endpoint = baseUri.WithRoute(route);

            return new WebRequestBuilder(endpoint, HttpMethod.Get);
        }

        protected WebRequestBuilder HttpPost(string baseUri, string route)
        {
            string endpoint = baseUri.WithRoute(route);

            return new WebRequestBuilder(endpoint, HttpMethod.Post);
        }

        protected WebRequestBuilder HttpPost<TBody>(string baseUri, string route, TBody body)
        {
            string endpoint = baseUri.WithRoute(route);

            return new WebRequestBuilder(endpoint, HttpMethod.Post, body);
        }

        protected WebRequestBuilder HttpPut(string baseUri, string route)
        {
            string endpoint = baseUri.WithRoute(route);

            return new WebRequestBuilder(endpoint, HttpMethod.Put);
        }

        protected WebRequestBuilder HttpPut<TBody>(string baseUri, string route, TBody body)
        {
            string endpoint = baseUri.WithRoute(route);

            return new WebRequestBuilder(endpoint, HttpMethod.Put, body);
        }

        protected WebRequestBuilder HttpDelete(string baseUri, string route)
        {
            string endpoint = baseUri.WithRoute(route);

            return new WebRequestBuilder(endpoint, HttpMethod.Delete);
        }

        protected WebRequestBuilder HttpDelete<TBody>(string baseUri, string route, TBody body)
        {
            string endpoint = baseUri.WithRoute(route);

            return new WebRequestBuilder(endpoint, HttpMethod.Delete, body);
        }

        protected WebRequestBuilder HttpPatch<TBody>(string baseUri, string route, TBody body)
        {
            string endpoint = baseUri.WithRoute(route);

            return new WebRequestBuilder(endpoint, new HttpMethod("PATCH"), body);
        }
    }
}