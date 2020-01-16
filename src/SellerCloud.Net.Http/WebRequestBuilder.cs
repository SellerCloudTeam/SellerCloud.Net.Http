using SellerCloud.Net.Http.Api;
using SellerCloud.Net.Http.Extensions;
using SellerCloud.Net.Http.Models;
using SellerCloud.Results;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;

namespace SellerCloud.Net.Http
{
    public class WebRequestBuilder
    {
        private readonly HttpMethod method;

        private readonly string endpoint;
        private readonly object body;

        private Action<HttpWebRequest> configureRequest;

        private AuthToken authToken;
        private TimeSpan? timeout;

        private readonly IEnumerable<HttpMethod> HttpMethodsThatNeedBody = new[] { HttpMethod.Post, HttpMethod.Put };

        public WebRequestBuilder(string endpoint, HttpMethod method, object body)
            : this(endpoint, method)
        {
            this.body = body;
        }

        public WebRequestBuilder(string endpoint, HttpMethod method)
        {
            this.endpoint = endpoint;
            this.method = method;
        }

        public WebRequestBuilder AuthorizeWith(AuthToken authToken)
        {
            this.authToken = authToken;

            return this;
        }

        public WebRequestBuilder TimeoutInSeconds(int seconds)
        {
            this.timeout = TimeSpan.FromSeconds(seconds);

            return this;
        }

        public WebRequestBuilder Timeout(TimeSpan timeout)
        {
            this.timeout = timeout;

            return this;
        }

        public WebRequestBuilder Request(Action<WebRequest> configure)
        {
            this.configureRequest = configure;

            return this;
        }

        public Result<T> Result<T>()
        {
            try
            {
                HttpWebResponse response = this.Response();
                Result<T> result = response.GetResult<T>();

                return result;
            }
            catch (WebException wex)
            {
                if (wex.TryExtractErrorFromBody(out string? message))
                {
                    return ResultFactory.Error<T>(message);
                }
                else
                {
                    return wex.AsResult<T>();
                }
            }
            catch (Exception ex)
            {
                return ex.AsResult<T>();
            }
        }

        public Result Result()
        {
            try
            {
                HttpWebResponse response = this.Response();
                Result result = response.GetResult();

                return result;
            }
            catch (WebException wex)
            {
                if (wex.TryExtractErrorFromBody(out string? message))
                {
                    return ResultFactory.Error(message);
                }
                else
                {
                    return wex.AsResult();
                }
            }
            catch (Exception ex)
            {
                return ex.AsResult();
            }
        }

        // TODO
        // public Result<FileAttachment> FileAttachment()
        // {
        //     try
        //     {
        //         HttpWebResponse response = this.Response();
        //         Result<FileAttachment> result = response.GetFileAttachmentResult();
        // 
        //         return result;
        //     }
        //     catch (Exception ex)
        //     {
        //         return ex.AsResult<FileAttachment>();
        //     }
        // }

        public HttpWebResponse Response()
        {
            bool requestMustHaveContent = this.body != null || HttpMethodsThatNeedBody.Contains(this.method);

            HttpWebRequest request = requestMustHaveContent
                ? WebHelper.ConstructWebRequestMessageWithContent(this.endpoint, this.method, this.body, this.authToken)
                : WebHelper.ConstructWebRequestMessage(this.endpoint, this.method, this.authToken);

            this.configureRequest?.Invoke(request);

            if (this.timeout != null)
            {
                request.Timeout = this.timeout.Value.Milliseconds;
            }

            HttpWebResponse response = (HttpWebResponse)request.GetResponse();

            return response;
        }
    }
}