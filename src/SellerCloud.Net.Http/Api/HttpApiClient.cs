﻿using SellerCloud.Net.Http.Helpers;
using System.Net.Http;

namespace SellerCloud.Net.Http.Api
{
    public class HttpApiClient : IHttpApiClient
    {
        private readonly HttpClient client;
        private readonly string baseUri;

        public HttpApiClient(string baseUri)
            : this(Singletons.DefaultHttpClient, baseUri)
        {
        }

        public HttpApiClient(HttpClient client, string baseUri)
        {
            this.client = client;
            this.baseUri = baseUri;
        }

        public HttpRequestBuilder HttpGet(string route)
        {
            string endpoint = this.baseUri + route;

            return new HttpRequestBuilder(this.client, endpoint, HttpMethod.Get);
        }

        public HttpRequestBuilder HttpPost(string route)
        {
            string endpoint = this.baseUri + route;

            return new HttpRequestBuilder(this.client, endpoint, HttpMethod.Post);
        }

        public HttpRequestBuilder HttpPost<TBody>(string route, TBody body)
        {
            string endpoint = this.baseUri + route;

            return new HttpRequestBuilder(this.client, endpoint, HttpMethod.Post, body);
        }

        public HttpRequestBuilder HttpPut<TBody>(string route, TBody body)
        {
            string endpoint = this.baseUri + route;

            return new HttpRequestBuilder(this.client, endpoint, HttpMethod.Put, body);
        }

        public HttpRequestBuilder HttpDelete(string route)
        {
            string endpoint = this.baseUri + route;

            return new HttpRequestBuilder(this.client, endpoint, HttpMethod.Delete);
        }
    }
}
