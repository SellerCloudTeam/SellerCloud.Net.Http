using System.Net;

namespace SellerCloud.Net.Http.Extensions
{
    public static class StatusCodeHelper
    {
        public static string GetStatusMessage(HttpStatusCode status)
        {
            IsSuccessStatus(status, out string message);

            return message;
        }
        public static bool IsSuccessStatus(HttpStatusCode status, out string message)
        {
            return IsSuccessStatus(status, responseContent: null, message: out message);
        }

        public static bool IsSuccessStatus(HttpStatusCode status, string responseContent, out string message)
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
            }

            string defaultMessage = $"Unexpected HTTP response status {GetDefaultErrorMessage(status)}!";

            message = string.IsNullOrWhiteSpace(responseContent)
                ? defaultMessage
                : responseContent;

            return false;
        }

        private static string GetDefaultErrorMessage(HttpStatusCode status)
        {
            switch (status)
            {
                case HttpStatusCode.BadRequest:
                    return $"{nameof(HttpStatusCode.BadRequest)}";

                case HttpStatusCode.Unauthorized:
                    return $"{nameof(HttpStatusCode.Unauthorized)}";

                case HttpStatusCode.NotFound:
                    return $"The requested content {nameof(HttpStatusCode.NotFound)}";

                case HttpStatusCode.NotAcceptable:
                    return "The request could not be processed"; // TODO: Better error message?

                case HttpStatusCode.InternalServerError:
                    return "An internal server error has occurred";
            }

            return null;
        }
    }
}
