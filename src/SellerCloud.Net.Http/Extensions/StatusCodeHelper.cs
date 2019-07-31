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
    }
}
