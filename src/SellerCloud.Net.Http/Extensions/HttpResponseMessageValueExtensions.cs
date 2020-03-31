using SellerCloud.Net.Http.ResponseModels;
using SellerCloud.Results.Http;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;

namespace SellerCloud.Net.Http.Extensions
{
    public static class HttpResponseMessageValueExtensions
    {
        public static async Task<HttpValueResult<T>> GetHttpValueResultAsync<T>(this HttpResponseMessage response)
            where T : struct
        {
            HttpStatusCode statusCode = response.StatusCode;

            HttpContent content = response.Content;
            MediaTypeHeaderValue? contentType = content?.Headers?.ContentType;

            if (content == null)
            {
                return HttpValueResultFactory.Error<T>(statusCode, Constants.UnknownError);
            }

            if (contentType == null)
            {
                return HandleNoBody<T>(response.StatusCode);
            }

            string body = await content.ReadAsStringAsync();
            string mediaType = contentType.MediaType;

            return mediaType switch
            {
                Constants.TextPlain => HandleText<T>(statusCode, body),
                Constants.TextHtml => HandleHtml<T>(response.StatusCode, body),
                Constants.ApplicationJson => HandleJson<T>(response.StatusCode, body),
                Constants.ApplicationProblemJson => HandleJson<T>(response.StatusCode, body),
                _ => HttpValueResultFactory.Error<T>(statusCode, $"{Constants.UnknownError}, media type {mediaType}"),
            };
        }

        private static HttpValueResult<T> HandleText<T>(HttpStatusCode statusCode, string body)
            where T : struct
        {
            return HttpValueResultFactory.Error<T>(statusCode, body ?? Constants.UnknownError);
        }

        private static HttpValueResult<T> HandleNoBody<T>(HttpStatusCode statusCode)
            where T : struct
        {
            HttpValueResult<T> result = HttpValueResultFactory.Error<T>(statusCode, Constants.UnknownError);

            if (!StatusCodeHelper.IsSuccessStatus(statusCode, out string? message))
            {
                result = HttpValueResultFactory.Error<T>(statusCode, message ?? Constants.UnknownError);
            }

            return result;
        }

        private static HttpValueResult<T> HandleHtml<T>(HttpStatusCode statusCode, string body)
            where T : struct
        {
            HttpValueResult<T> result = HttpValueResultFactory.Error<T>(statusCode, Constants.UnknownError);

            if (!StatusCodeHelper.IsSuccessStatus(statusCode, body, out string? message))
            {
                result = HttpValueResultFactory.Error<T>(statusCode, message ?? body);
            }

            return result;
        }

        private static HttpValueResult<T> HandleJson<T>(HttpStatusCode statusCode, string body)
            where T : struct
        {
            GenericErrorResponse? error = JsonHelper.TryDeserialize<GenericErrorResponse>(body);

            string? errorMessage = error?.ErrorMessage ?? error?.ExceptionMessage ?? error?.Message ?? error?.RfcError ?? error?.Title ?? error?.ErrorDescription;
            string? errorSource = error?.ErrorSource ?? error?.StackTrace ?? error?.TraceId;

            if (!StatusCodeHelper.IsSuccessStatus(statusCode, body, out string? message))
            {
                return HttpValueResultFactory.Error<T>(statusCode, errorMessage ?? message ?? Constants.UnknownError, errorSource);
            }

            T data = JsonHelper.Deserialize<T>(body);

            return HttpValueResultFactory.Success(statusCode, data);
        }
    }
}
