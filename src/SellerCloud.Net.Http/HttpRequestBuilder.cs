using SellerCloud.Net.Http.Extensions;
using SellerCloud.Net.Http.Models;
using SellerCloud.Results;
using SellerCloud.Results.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;

namespace SellerCloud.Net.Http
{
    public class HttpRequestBuilder
    {
        private readonly HttpClient client;
        private readonly HttpMethod method;

        private readonly string endpoint;
        private readonly object? body;

        private Action<HttpRequestMessage>? configureRequest;

        private AuthToken? authToken;
        private CancellationToken cancellationToken;

        private readonly IEnumerable<HttpMethod> HttpMethodsThatNeedBody = new[] { HttpMethod.Post, HttpMethod.Put };

        public HttpRequestBuilder(HttpClient client, string endpoint, HttpMethod method, object body)
            : this(client, endpoint, method)
        {
            this.body = body;
        }

        public HttpRequestBuilder(HttpClient client, string endpoint, HttpMethod method)
        {
            this.client = client;
            this.endpoint = endpoint;
            this.method = method;
        }

        public HttpRequestBuilder AuthorizeWith(AuthToken authToken)
        {
            this.authToken = authToken;

            return this;
        }

        public HttpRequestBuilder CancelWith(CancellationToken cancellationToken)
        {
            this.cancellationToken = cancellationToken;

            return this;
        }

        public HttpRequestBuilder Request(Action<HttpRequestMessage> configure)
        {
            this.configureRequest = configure;

            return this;
        }

        public async Task<HttpResult<T>> HttpResult<T>()
        {
            try
            {
                HttpResponseMessage response = await this.Response();
                HttpResult<T> result = await response.GetHttpResultAsync<T>();

                return result;
            }
            catch (Exception ex)
            {
                return ex.AsHttpResult<T>(Constants.UnknownHttpStatusCode);
            }
        }

        public async Task<HttpResult> HttpResult()
        {
            try
            {
                HttpResponseMessage response = await this.Response();
                HttpResult result = await response.GetHttpResultAsync();

                return result;
            }
            catch (Exception ex)
            {
                return ex.AsHttpResult(Constants.UnknownHttpStatusCode);
            }
        }

        public async Task<HttpResult<FileAttachment>> FileAttachment()
        {
            try
            {
                HttpResponseMessage response = await this.Response();
                HttpResult<FileAttachment> result = await response.GetFileAttachmentHttpResultAsync();

                return result;
            }
            catch (Exception ex)
            {
                return ex.AsHttpResult<FileAttachment>(Constants.UnknownHttpStatusCode);
            }
        }

        public async Task<HttpResponseMessage> Response()
        {
            bool requestMustHaveContent = this.body != null || HttpMethodsThatNeedBody.Contains(this.method);

            HttpRequestMessage request = requestMustHaveContent
                ? HttpHelper.ConstructHttpRequestMessageWithContent(this.endpoint, this.method, this.body, this.authToken)
                : HttpHelper.ConstructHttpRequestMessage(this.endpoint, this.method, this.authToken);

            this.configureRequest?.Invoke(request);

            HttpResponseMessage response = await this.client.SendAsync(request, this.cancellationToken);

            return response;
        }
    }
}
