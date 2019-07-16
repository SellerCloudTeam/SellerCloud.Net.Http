using Newtonsoft.Json;
using SellerCloud.Net.Http.Models;
using SellerCloud.Net.Http.ResponseModels;
using SellerCloud.Results;
using System.Net;
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

                error = TryDeserialize<GenericErrorResponse>(body);
            }

            string errorMessage = error?.ErrorMessage ?? error?.ExceptionMessage;

            if (!IsSuccessStatus(response.StatusCode, out string message))
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

            GenericErrorResponse error = TryDeserialize<GenericErrorResponse>(body);

            string errorMessage = error?.ErrorMessage ?? error?.ExceptionMessage;
            string errorSource = error?.ErrorSource ?? error?.StackTrace;

            if (!IsSuccessStatus(response.StatusCode, out string message))
            {
                return ResultFactory.Error<T>(errorMessage ?? message, errorSource);
            }

            T data = Deserialize<T>(body);

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

                error = TryDeserialize<GenericErrorResponse>(body);
                content = await response.Content.ReadAsByteArrayAsync();

                name = response.Content.Headers.ContentDisposition?.FileName;
                contentType = response.Content.Headers?.ContentType?.MediaType;
            }

            string errorMessage = error?.ErrorMessage ?? error?.ExceptionMessage;

            if (!IsSuccessStatus(response.StatusCode, out string message))
            {
                return ResultFactory.Error<FileAttachment>(errorMessage ?? message);
            }

            FileAttachment file = new FileAttachment(name ?? UntitledFileName, content, contentType);

            return ResultFactory.Success(file);
        }

        private static bool IsSuccessStatus(HttpStatusCode status, out string message)
        {
            message = null;

            switch (status)
            {
                case HttpStatusCode.OK:
                case HttpStatusCode.NoContent:
                case HttpStatusCode.Continue:

                case HttpStatusCode.Found:
                case HttpStatusCode.MovedPermanently:
                case HttpStatusCode.TemporaryRedirect:

                case HttpStatusCode.Created:
                    return true;

                case HttpStatusCode.BadRequest:
                    message = "Bad request";

                    return false;

                case HttpStatusCode.Unauthorized:
                    message = "Unauthorized";

                    return false;

                case HttpStatusCode.NotFound:
                    message = "Requested content not found";

                    return false;

                case HttpStatusCode.NotAcceptable:
                    message = "Request could not be processed"; // TODO: Better error message?

                    return false;

                case HttpStatusCode.InternalServerError:
                    message = "An internal server error has occurred";

                    return false;
            }

            message = $"Unexpected HTTP response status {status}!";

            return false;
        }

        private static bool IsJsonString(string input) => input.TrimStart()?.StartsWith(@"""") ?? false;
        private static bool IsJsonObject(string input) => input.TrimStart()?.StartsWith("{") ?? false;

        private static T Deserialize<T>(string input) => JsonConvert.DeserializeObject<T>(input);

        private static T TryDeserialize<T>(string input)
        {
            T result;

            bool isStringExpected = typeof(T) == typeof(string);
            bool isStringProvided = IsJsonString(input);

            // Special cases to avoid exception throwing
            if (isStringExpected && !isStringProvided)
            {
                result = default(T);
            }
            else if (isStringProvided && !isStringExpected)
            {
                result = default(T);
            }
            else
            {
                try
                {
                    result = Deserialize<T>(input);
                }
                catch (JsonReaderException)
                {
                    result = default(T);
                }
                catch (JsonSerializationException)
                {
                    result = default(T);
                }
            }

            return result;
        }
    }
}
