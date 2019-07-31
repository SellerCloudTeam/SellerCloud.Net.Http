using Newtonsoft.Json;
using SellerCloud.Net.Http.Models;
using System.IO;
using System.Net;
using System.Net.Http;
using System.Text;

namespace SellerCloud.Net.Http.Api
{
    public static class WebHelper
    {
        private const string ApplicationJson = "application/json";

        public static HttpWebRequest ConstructWebRequestMessage(string endpoint, HttpMethod method, AuthToken token)
        {
            HttpWebRequest request = WebRequest.CreateHttp(endpoint);

            request.Method = method.Method;

            if (token != null)
            {
                request.Headers[HttpRequestHeader.Authorization] = $"{token.Scheme} {token.Parameter}";
            }

            return request;
        }

        public static HttpWebRequest ConstructWebRequestMessageWithContent<T>(string endpoint, HttpMethod method, T data, AuthToken token)
        {
            HttpWebRequest request = WebRequest.CreateHttp(endpoint);

            request.Method = method.Method;
            request.ContentType = ApplicationJson;

            string dataJson = SerializeAsJson(data);
            byte[] dataBytes = Encoding.UTF8.GetBytes(dataJson);

            using (Stream stream = request.GetRequestStream())
            {
                stream.Write(dataBytes, 0, dataBytes.Length);
            }

            if (token != null)
            {
                request.Headers[HttpRequestHeader.Authorization] = $"{token.Scheme} {token.Parameter}";
            }

            return request;
        }

        private static string SerializeAsJson<T>(T data)
        {
            return JsonConvert.SerializeObject(data);
        }
    }
}