﻿using SellerCloud.Net.Http.Api;
using SellerCloud.Net.Http.Extensions;
using SellerCloud.Net.Http.Models;
using SellerCloud.Results;
using SellerCloud.Results.Http;
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
        private readonly object? body;

        private Action<HttpWebRequest>? configureRequest;

        private AuthToken? authToken;
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

        public HttpResult<T> HttpResult<T>()
        {
            try
            {
                HttpWebResponse response = this.Response();
                HttpResult<T> result = response.GetHttpResult<T>();

                return result;
            }
            catch (WebException wex)
            {
                if (wex.TryExtractErrorFromBody(out string? message))
                {
                    return HttpResultFactory.Error<T>(message ?? Constants.UnknownError);
                }
                else
                {
                    return wex.AsHttpResult<T>();
                }
            }
            catch (Exception ex)
            {
                return ex.AsHttpResult<T>(Constants.UnknownHttpStatusCode);
            }
        }

        public HttpResult HttpResult()
        {
            try
            {
                HttpWebResponse response = this.Response();
                HttpResult result = response.GetHttpResult();

                return result;
            }
            catch (WebException wex)
            {
                if (wex.TryExtractErrorFromBody(out string? message))
                {
                    return HttpResultFactory.Error(message ?? Constants.UnknownError);
                }
                else
                {
                    return wex.AsHttpResult(Constants.UnknownHttpStatusCode);
                }
            }
            catch (Exception ex)
            {
                return ex.AsHttpResult();
            }
        }

        // TODO
        // public HttpResult<FileAttachment> FileAttachment()
        // {
        //     try
        //     {
        //         HttpWebResponse response = this.Response();
        //         HttpResult<FileAttachment> result = response.GetFileAttachmentHttpResult();
        // 
        //         return result;
        //     }
        //     catch (Exception ex)
        //     {
        //         return ex.AsHttpResult<FileAttachment>();
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