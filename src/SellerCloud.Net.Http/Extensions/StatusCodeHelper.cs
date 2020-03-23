using System.Net;

namespace SellerCloud.Net.Http.Extensions
{
    public static class StatusCodeHelper
    {
        public static string? GetStatusMessage(HttpStatusCode status)
        {
            IsSuccessStatus(status, out string? message);

            return message;
        }

        public static bool IsSuccessStatus(HttpStatusCode status, out string? message)
        {
            return IsSuccessStatus(status, responseContent: null, message: out message);
        }

        public static bool IsSuccessStatus(HttpStatusCode status, string? responseContent, out string? message)
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

            var errorMessage = GetDefaultErrorMessage(status);

            message = string.IsNullOrWhiteSpace(errorMessage)
                ? responseContent
                : errorMessage;

            return false;
        }

        private static string? GetDefaultErrorMessage(HttpStatusCode status)
        {
            return status switch
            {
                HttpStatusCode.BadRequest => $"Bad request",
                HttpStatusCode.Unauthorized => $"Unauthorized",
                HttpStatusCode.NotFound => $"The requested content was not found",
                HttpStatusCode.NotAcceptable => "The request could not be processed", // TODO: Better error message?
                HttpStatusCode.InternalServerError => "An internal server error has occurred",

                _ => null,
            };
        }
    }
}
