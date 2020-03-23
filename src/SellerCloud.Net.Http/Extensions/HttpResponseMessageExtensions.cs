using SellerCloud.Net.Http.Models;
using SellerCloud.Net.Http.ResponseModels;
using SellerCloud.Results;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;

namespace SellerCloud.Net.Http.Extensions
{
    public static class HttpResponseMessageExtensions
    {
        public static async Task<Result> GetResultAsync(this HttpResponseMessage response)
        {
            HttpContent content = response.Content;

            if (content == null)
            {
                if (!StatusCodeHelper.IsSuccessStatus(response.StatusCode, out string? standardErrorMessage))
                {
                    return ResultFactory.Error(standardErrorMessage ?? Constants.UnknownError);
                }

                return ResultFactory.Success();
            }

            string body = await content.ReadAsStringAsync();

            GenericErrorResponse? error = JsonHelper.TryDeserialize<GenericErrorResponse>(body);

            string? errorMessage = error?.ErrorMessage ?? error?.ExceptionMessage ?? error?.Message ?? error?.Title;

            if (!StatusCodeHelper.IsSuccessStatus(response.StatusCode, body, out string? message))
            {
                return ResultFactory.Error(errorMessage ?? message ?? Constants.UnknownError);
            }

            return ResultFactory.Success();
        }

        public static async Task<Result<T>> GetResultAsync<T>(this HttpResponseMessage response)
        {
            HttpContent content = response.Content;
            MediaTypeHeaderValue? contentType = content?.Headers?.ContentType;

            if (content == null)
            {
                return ResultFactory.Error<T>(Constants.UnknownError);
            }

            if (contentType == null)
            {
                return HandleNoBody<T>(response.StatusCode);
            }

            string body = await content.ReadAsStringAsync();
            string mediaType = contentType.MediaType;

            return mediaType switch
            {
                Constants.TextPlain => HandleText<T>(body),
                Constants.TextHtml => HandleHtml<T>(response.StatusCode, body),
                Constants.ApplicationJson => HandleJson<T>(response.StatusCode, body),
                _ => ResultFactory.Error<T>($"{Constants.UnknownError}, media type {mediaType}"),
            };
        }

        public static async Task<Result<FileAttachment>> GetFileAttachmentResultAsync(this HttpResponseMessage response)
        {
            const string UntitledFileName = "Untitled.dat";

            string? name = null;
            string? contentType = null;
            byte[]? content = null;

            GenericErrorResponse? error = null;
            string? body = null;

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
                return ResultFactory.Error<FileAttachment>(errorMessage ?? message ?? Constants.UnknownError);
            }

            if (content == null)
            {
                return ResultFactory.Error<FileAttachment>(Constants.NoContentError);
            }

            FileAttachment file = new FileAttachment(name ?? UntitledFileName, content, contentType ?? Constants.ApplicationBinary);

            return ResultFactory.Success(file);
        }

        private static Result<T> HandleText<T>(string body) =>
            ResultFactory.Error<T>(body ?? Constants.UnknownError);

        private static Result<T> HandleNoBody<T>(HttpStatusCode statusCode)
        {
            Result<T> result = ResultFactory.Error<T>(Constants.UnknownError);
            if (!StatusCodeHelper.IsSuccessStatus(statusCode, out string? message))
            {
                result = ResultFactory.Error<T>(message ?? Constants.UnknownError);
            }
            return result;
        }

        private static Result<T> HandleHtml<T>(HttpStatusCode statusCode, string body)
        {
            Result<T> result = ResultFactory.Error<T>(Constants.UnknownError);
            if (!StatusCodeHelper.IsSuccessStatus(statusCode, body, out string? message))
            {
                result = ResultFactory.Error<T>(message ?? body);
            }
            return result;
        }

        private static Result<T> HandleJson<T>(HttpStatusCode statusCode, string body)
        {
            GenericErrorResponse? error = JsonHelper.TryDeserialize<GenericErrorResponse>(body);

            string? errorMessage = error?.ErrorMessage ?? error?.ExceptionMessage ?? error?.Message ?? error?.Title;
            string? errorSource = error?.ErrorSource ?? error?.StackTrace ?? error?.TraceId;

            if (!StatusCodeHelper.IsSuccessStatus(statusCode, body, out string? message))
            {
                return ResultFactory.Error<T>(errorMessage ?? message ?? Constants.UnknownError, errorSource);
            }
            T data = JsonHelper.Deserialize<T>(body);
            return ResultFactory.Success(data);
        }
    }
}
