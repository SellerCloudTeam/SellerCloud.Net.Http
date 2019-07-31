using SellerCloud.Net.Http.ResponseModels;
using System.Net;

namespace SellerCloud.Net.Http.Extensions
{
    public static class WebExceptionExtensions
    {
        private const string ApplicationJson = "application/json";

        public static bool TryExtractErrorFromBody(this WebException exception, out string message)
        {
            message = null;

            if (exception.Response == null)
            {
                return false;
            }

            if (!exception.Response.TryReadBody(out string body))
            {
                return false;
            }

            if (exception.Response.ContentType == ApplicationJson)
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
                HttpWebResponse httpWebResponse = exception.Response as HttpWebResponse;

                message = StatusCodeHelper.GetStatusMessage(httpWebResponse?.StatusCode ?? HttpStatusCode.NotImplemented);
            }

            return true;
        }

        private static string ExtractErrorFromJsonBody(HttpWebResponse response, string body)
        {
            GenericErrorResponse error = JsonHelper.TryDeserialize<GenericErrorResponse>(body);

            string errorMessage = error?.ErrorMessage ?? error?.ExceptionMessage ?? error?.Message;
            string statusMessage = StatusCodeHelper.GetStatusMessage(response.StatusCode);

            return errorMessage ?? statusMessage;
        }
    }
}
