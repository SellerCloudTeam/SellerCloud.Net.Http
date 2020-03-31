using SellerCloud.Net.Http.Models;
using SellerCloud.Net.Http.ResponseModels;
using SellerCloud.Results.Http;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;

namespace SellerCloud.Net.Http.Extensions
{
    public static class HttpResponseMessageExtensions
    {
        public static async Task<HttpResult> GetHttpResultAsync(this HttpResponseMessage response)
        {
            HttpStatusCode statusCode = response.StatusCode;
            HttpContent content = response.Content;

            if (content == null)
            {
                if (!StatusCodeHelper.IsSuccessStatus(response.StatusCode, out string? standardErrorMessage))
                {
                    return HttpResultFactory.Error(statusCode, standardErrorMessage ?? Constants.UnknownError);
                }

                return HttpResultFactory.Success(statusCode);
            }

            string body = await content.ReadAsStringAsync();

            GenericErrorResponse? error = JsonHelper.TryDeserialize<GenericErrorResponse>(body);

            string? errorMessage = error?.ErrorMessage ?? error?.ExceptionMessage ?? error?.Message ?? error?.RfcError ?? error?.Title ?? error?.ErrorDescription;

            if (!StatusCodeHelper.IsSuccessStatus(response.StatusCode, body, out string? message))
            {
                return HttpResultFactory.Error(statusCode, errorMessage ?? message ?? Constants.UnknownError);
            }

            return HttpResultFactory.Success(statusCode);
        }

        public static async Task<HttpResult<T>> GetHttpResultAsync<T>(this HttpResponseMessage response)
            where T : class
        {
            HttpStatusCode statusCode = response.StatusCode;
            HttpContent content = response.Content;
            MediaTypeHeaderValue? contentType = content?.Headers?.ContentType;

            if (content == null)
            {
                return HttpResultFactory.Error<T>(statusCode, Constants.UnknownError);
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
                _ => HttpResultFactory.Error<T>(statusCode, $"{Constants.UnknownError}, media type {mediaType}"),
            };
        }

        public static async Task<HttpResult<FileAttachment>> GetFileAttachmentHttpResultAsync(this HttpResponseMessage response)
        {
            const string UntitledFileName = "Untitled.dat";

            string? name = null;
            string? contentType = null;
            byte[]? content = null;

            GenericErrorResponse? error = null;
            string? body = null;

            HttpStatusCode statusCode = response.StatusCode;

            if (response.Content != null)
            {
                body = await response.Content.ReadAsStringAsync();

                error = JsonHelper.TryDeserialize<GenericErrorResponse>(body);
                content = await response.Content.ReadAsByteArrayAsync();

                name = response.Content.Headers.ContentDisposition?.FileName;
                contentType = response.Content.Headers?.ContentType?.MediaType;
            }

            string? errorMessage = error?.ErrorMessage ?? error?.ExceptionMessage ?? error?.Message ?? error?.Title;

            if (!StatusCodeHelper.IsSuccessStatus(response.StatusCode, body, out string? message))
            {
                return HttpResultFactory.Error<FileAttachment>(statusCode, errorMessage ?? message ?? Constants.UnknownError);
            }

            if (content == null)
            {
                return HttpResultFactory.Error<FileAttachment>(statusCode, Constants.NoContentError);
            }

            FileAttachment file = new FileAttachment(name ?? UntitledFileName, content, contentType ?? Constants.ApplicationBinary);

            return HttpResultFactory.Success(statusCode, file);
        }

        private static HttpResult<T> HandleText<T>(HttpStatusCode statusCode, string body)
            where T : class
            => HttpResultFactory.Error<T>(statusCode, body ?? Constants.UnknownError);

        private static HttpResult<T> HandleNoBody<T>(HttpStatusCode statusCode)
            where T : class
        {
            HttpResult<T> result = HttpResultFactory.Error<T>(statusCode, Constants.UnknownError);
            if (!StatusCodeHelper.IsSuccessStatus(statusCode, out string? message))
            {
                result = HttpResultFactory.Error<T>(statusCode, message ?? Constants.UnknownError);
            }
            return result;
        }

        private static HttpResult<T> HandleHtml<T>(HttpStatusCode statusCode, string body)
            where T : class
        {
            HttpResult<T> result = HttpResultFactory.Error<T>(statusCode, Constants.UnknownError);
            if (!StatusCodeHelper.IsSuccessStatus(statusCode, body, out string? message))
            {
                result = HttpResultFactory.Error<T>(statusCode, message ?? body);
            }
            return result;
        }

        private static HttpResult<T> HandleJson<T>(HttpStatusCode statusCode, string body)
            where T : class
        {
            GenericErrorResponse? error = JsonHelper.TryDeserialize<GenericErrorResponse>(body);

            string? errorMessage = error?.ErrorMessage ?? error?.ExceptionMessage ?? error?.Message ?? error?.RfcError ?? error?.Title ?? error?.ErrorDescription;
            string? errorSource = error?.ErrorSource ?? error?.StackTrace ?? error?.TraceId;

            if (!StatusCodeHelper.IsSuccessStatus(statusCode, body, out string? message))
            {
                return HttpResultFactory.Error<T>(statusCode, errorMessage ?? message ?? Constants.UnknownError, errorSource);
            }
            T data = JsonHelper.Deserialize<T>(body);
            return HttpResultFactory.Success(statusCode, data);
        }
    }
}
