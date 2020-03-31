using SellerCloud.Net.Http.ResponseModels;
using SellerCloud.Results.Http;
using System.IO;
using System.Net;

namespace SellerCloud.Net.Http.Extensions
{
    public static class WebResponseExtensions
    {
        public static bool TryReadBody(this WebResponse response, out string? body)
        {
            body = null;

            try
            {
                body = response.ReadBody();

                return true;
            }
            catch
            {
                return false;
            }
        }

        private static string ReadBody(this WebResponse response)
        {
            using (Stream stream = response.GetResponseStream())
            using (StreamReader reader = new StreamReader(stream))
            {
                return reader.ReadToEnd();
            }
        }

        public static HttpResult GetHttpResult(this HttpWebResponse response)
        {
            HttpStatusCode statusCode = response.StatusCode;

            GenericErrorResponse? error = null;

            if (response.TryReadBody(out string? body))
            {
                error = JsonHelper.TryDeserialize<GenericErrorResponse>(body);
            }

            string? errorMessage = error?.ErrorMessage ?? error?.ExceptionMessage ?? error?.Message ?? error?.Title;

            if (!StatusCodeHelper.IsSuccessStatus(response.StatusCode, out string? message))
            {
                return HttpResultFactory.Error(statusCode, errorMessage ?? message ?? Constants.UnknownError);
            }

            return HttpResultFactory.Success(statusCode);
        }

        public static HttpResult<T> GetHttpResult<T>(this HttpWebResponse response)
            where T : class
        {
            HttpStatusCode statusCode = response.StatusCode;

            if (!response.TryReadBody(out string? body))
            {
                return HttpResultFactory.Error<T>(statusCode, "Could not read web response!");
            }

            GenericErrorResponse? error = JsonHelper.TryDeserialize<GenericErrorResponse>(body);

            string? errorMessage = error?.ErrorMessage ?? error?.ExceptionMessage ?? error?.Message ?? error?.Title;
            string? errorSource = error?.ErrorSource ?? error?.StackTrace ?? error?.TraceId;

            if (!StatusCodeHelper.IsSuccessStatus(response.StatusCode, out string? message))
            {
                return HttpResultFactory.Error<T>(statusCode, errorMessage ?? message ?? Constants.UnknownError, errorSource);
            }

            T data = JsonHelper.Deserialize<T>(body);

            return HttpResultFactory.Success(statusCode, data);
        }

        public static HttpValueResult<T> GetHttpValueResult<T>(this HttpWebResponse response)
            where T : struct
        {
            HttpStatusCode statusCode = response.StatusCode;

            if (!response.TryReadBody(out string? body))
            {
                return HttpValueResultFactory.Error<T>(statusCode, "Could not read web response!");
            }

            GenericErrorResponse? error = JsonHelper.TryDeserialize<GenericErrorResponse>(body);

            string? errorMessage = error?.ErrorMessage ?? error?.ExceptionMessage ?? error?.Message ?? error?.Title;
            string? errorSource = error?.ErrorSource ?? error?.StackTrace ?? error?.TraceId;

            if (!StatusCodeHelper.IsSuccessStatus(response.StatusCode, out string? message))
            {
                return HttpValueResultFactory.Error<T>(statusCode, errorMessage ?? message ?? Constants.UnknownError, errorSource);
            }

            T data = JsonHelper.Deserialize<T>(body);

            return HttpValueResultFactory.Success(statusCode, data);
        }
    }
}
