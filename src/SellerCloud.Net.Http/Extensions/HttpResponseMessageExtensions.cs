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
            GenericErrorResponse error = null;

            if (response.Content != null)
            {
                string body = await response.Content.ReadAsStringAsync();

                error = JsonHelper.TryDeserialize<GenericErrorResponse>(body);
            }

            string errorMessage = error?.ErrorMessage ?? error?.ExceptionMessage ?? error?.Message;

            if (!StatusCodeHelper.IsSuccessStatus(response.StatusCode, out string message))
            {
                return ResultFactory.Error(errorMessage ?? message);
            }

            return ResultFactory.Success();
        }

        public static async Task<Result<T>> GetResultAsync<T>(this HttpResponseMessage response)
        {
            string body = response.Content == null
                ? null
                : await response.Content.ReadAsStringAsync();

            GenericErrorResponse error = JsonHelper.TryDeserialize<GenericErrorResponse>(body);

            string errorMessage = error?.ErrorMessage ?? error?.ExceptionMessage ?? error?.Message;
            string errorSource = error?.ErrorSource ?? error?.StackTrace;

            if (!StatusCodeHelper.IsSuccessStatus(response.StatusCode, out string message))
            {
                return ResultFactory.Error<T>(errorMessage ?? message, errorSource);
            }

            T data = JsonHelper.Deserialize<T>(body);

            return ResultFactory.Success(data);
        }

        public static async Task<Result<FileAttachment>> GetFileAttachmentResultAsync(this HttpResponseMessage response)
        {
            const string UntitledFileName = "Untitled.dat";

            string name = null;
            string contentType = null;
            byte[] content = null;

            GenericErrorResponse error = null;

            if (response.Content != null)
            {
                string body = await response.Content.ReadAsStringAsync();

                error = JsonHelper.TryDeserialize<GenericErrorResponse>(body);
                content = await response.Content.ReadAsByteArrayAsync();

                name = response.Content.Headers.ContentDisposition?.FileName;
                contentType = response.Content.Headers?.ContentType?.MediaType;
            }

            string errorMessage = error?.ErrorMessage ?? error?.ExceptionMessage ?? error?.Message;

            if (!StatusCodeHelper.IsSuccessStatus(response.StatusCode, out string message))
            {
                return ResultFactory.Error<FileAttachment>(errorMessage ?? message);
            }

            FileAttachment file = new FileAttachment(name ?? UntitledFileName, content, contentType);

            return ResultFactory.Success(file);
        }
    }
}
