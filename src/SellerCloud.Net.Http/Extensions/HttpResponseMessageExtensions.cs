using SellerCloud.Net.Http.Models;
using SellerCloud.Net.Http.ResponseModels;
using SellerCloud.Results;
using System.Net.Http;
using System.Threading.Tasks;

namespace SellerCloud.Net.Http.Extensions
{
    public static class HttpResponseMessageExtensions
    {
        public static async Task<Result> GetResultAsync(this HttpResponseMessage response)
        {
            string? body = response.Content == null
                ? null
                : await response.Content.ReadAsStringAsync();

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
            string? body = response.Content == null
                ? null
                : await response.Content.ReadAsStringAsync();

            GenericErrorResponse? error = JsonHelper.TryDeserialize<GenericErrorResponse>(body);

            string? errorMessage = error?.ErrorMessage ?? error?.ExceptionMessage ?? error?.Message ?? error?.Title;
            string? errorSource = error?.ErrorSource ?? error?.StackTrace ?? error?.TraceId;

            if (!StatusCodeHelper.IsSuccessStatus(response.StatusCode, body, out string? message))
            {
                return ResultFactory.Error<T>(errorMessage ?? message ?? Constants.UnknownError, errorSource);
            }

            T data = JsonHelper.Deserialize<T>(body);

            return ResultFactory.Success(data);
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
    }
}
