using Newtonsoft.Json;
using SellerCloud.Net.Http.Models;
using System;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;

namespace SellerCloud.Net.Http.Extensions
{
    public static class HttpHelper
    {
        private const string ApplicationJson = "application/json";

        public static HttpRequestMessage ConstructHttpRequestMessage(string endpoint, HttpMethod method, AuthToken token)
        {
            HttpRequestMessage request = new HttpRequestMessage
            {
                RequestUri = new Uri(endpoint),
                Method = method
            };

            if (token != null)
            {
                request.Headers.Authorization = new AuthenticationHeaderValue(token.Scheme, token.Parameter);
            }

            return request;
        }

        public static HttpRequestMessage ConstructHttpRequestMessageWithContent<T>(string endpoint, HttpMethod method, T data, AuthToken token)
        {
            HttpRequestMessage request = new HttpRequestMessage
            {
                RequestUri = new Uri(endpoint),
                Method = method,
                Content = data is byte[] bytes
                    ? ConstructByteArrayContent(bytes)
                    : ConstructJsonContent(data)
            };

            if (token != null)
            {
                request.Headers.Authorization = new AuthenticationHeaderValue(token.Scheme, token.Parameter);
            }

            return request;
        }

        public static HttpContent ConstructJsonContent(object data)
        {
            return new StringContent(SerializeAsJson(data), Encoding.UTF8, ApplicationJson);
        }

        public static HttpContent ConstructByteArrayContent(byte[] data)
        {
            return new ByteArrayContent(data);
        }

        private static string SerializeAsJson<T>(T data)
        {
            return JsonConvert.SerializeObject(data);
        }
    }
}
