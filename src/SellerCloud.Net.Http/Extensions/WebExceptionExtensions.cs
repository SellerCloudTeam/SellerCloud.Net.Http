using SellerCloud.Net.Http.ResponseModels;
using System.Net;

namespace SellerCloud.Net.Http.Extensions
{
    public static class WebExceptionExtensions
    {
        public static HttpStatusCode? StatusCodeOrDefault(this WebException exception)
        {
            if (exception.Response is HttpWebResponse httpResponse)
            {
                return httpResponse.StatusCode;
            }

            return default;
        }

        public static bool TryExtractErrorFromBody(this WebException exception, out string? message)
        {
            message = null;

            if (exception.Response == null)
            {
                return false;
            }

            if (!exception.Response.TryReadBody(out string? body))
            {
                return false;
            }

            if (exception.Response.ContentType == Constants.ApplicationJson || exception.Response.ContentType?.StartsWith(Constants.ApplicationJson) == true)
            {
                message = ExtractErrorFromJsonBody(exception.Response as HttpWebResponse, body);

                return true;
            }
            else if (!string.IsNullOrWhiteSpace(body))
            {
                message = body;
            }
            else
            {
                HttpWebResponse? httpWebResponse = exception.Response as HttpWebResponse;

                message = StatusCodeHelper.GetStatusMessage(httpWebResponse?.StatusCode ?? HttpStatusCode.NotImplemented);
            }

            return true;
        }

        private static string ExtractErrorFromJsonBody(HttpWebResponse? response, string? body)
        {
            GenericErrorResponse? error = JsonHelper.TryDeserialize<GenericErrorResponse>(body);

            string? errorMessage = error?.ErrorMessage ?? error?.ExceptionMessage ?? error?.Message;
            string? statusMessage = response == null
                ? null
                : StatusCodeHelper.GetStatusMessage(response.StatusCode);

            return errorMessage ?? statusMessage ?? Constants.UnknownError;
        }
    }
}
