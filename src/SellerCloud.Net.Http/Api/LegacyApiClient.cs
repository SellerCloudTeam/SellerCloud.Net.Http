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
            string endpoint = this.baseUri + route;

            return new WebRequestBuilder(endpoint, HttpMethod.Get);
        }

        public WebRequestBuilder HttpPost(string route)
        {
            string endpoint = this.baseUri + route;

            return new WebRequestBuilder(endpoint, HttpMethod.Post);
        }

        public WebRequestBuilder HttpPost<TBody>(string route, TBody body)
        {
            string endpoint = this.baseUri + route;

            return new WebRequestBuilder(endpoint, HttpMethod.Post, body);
        }

        public WebRequestBuilder HttpPut<TBody>(string route, TBody body)
        {
            string endpoint = this.baseUri + route;

            return new WebRequestBuilder(endpoint, HttpMethod.Put, body);
        }

        public WebRequestBuilder HttpDelete(string route)
        {
            string endpoint = this.baseUri + route;

            return new WebRequestBuilder(endpoint, HttpMethod.Delete);
        }
    }
}