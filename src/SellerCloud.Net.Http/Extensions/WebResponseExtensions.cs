using SellerCloud.Net.Http.ResponseModels;
using SellerCloud.Results;
using System.IO;
using System.Net;

namespace SellerCloud.Net.Http.Extensions
{
    public static class WebResponseExtensions
    {
        public static bool TryReadBody(this WebResponse response, out string body)
        {
            body = null;

            try
            {
                body = response.ReadBody();

                return true;
            }
            catch
            {
                return false;
            }
        }

        private static string ReadBody(this WebResponse response)
        {
            using (Stream stream = response.GetResponseStream())
            using (StreamReader reader = new StreamReader(stream))
            {
                return reader.ReadToEnd();
            }
        }

        public static Result GetResult(this HttpWebResponse response)
        {
            GenericErrorResponse error = null;

            if (response.TryReadBody(out string body))
            {
                error = JsonHelper.TryDeserialize<GenericErrorResponse>(body);
            }

            string errorMessage = error?.ErrorMessage ?? error?.ExceptionMessage ?? error?.Message;

            if (!StatusCodeHelper.IsSuccessStatus(response.StatusCode, out string message))
            {
                return ResultFactory.Error(errorMessage ?? message);
            }

            return ResultFactory.Success();
        }

        public static Result<T> GetResult<T>(this HttpWebResponse response)
        {
            if (!response.TryReadBody(out string body))
            {
                return ResultFactory.Error<T>("Could not read web response!");
            }

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
    }
}
