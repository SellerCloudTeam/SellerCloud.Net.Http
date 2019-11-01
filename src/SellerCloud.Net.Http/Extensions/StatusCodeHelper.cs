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

            message = responseContent ?? defaultMessage;

            return false;
        }

        private static string GetDefaultErrorMessage(HttpStatusCode status)
        {
            switch (status)
            {
                case HttpStatusCode.BadRequest:
                    return "Bad request";

                case HttpStatusCode.Unauthorized:
                    return "Unauthorized";

                case HttpStatusCode.NotFound:
                    return "Requested content not found";

                case HttpStatusCode.NotAcceptable:
                    return "Request could not be processed"; // TODO: Better error message?

                case HttpStatusCode.InternalServerError:
                    return "An internal server error has occurred";
            }

            return null;
        }
    }
}
