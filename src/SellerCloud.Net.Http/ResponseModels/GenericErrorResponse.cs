using Newtonsoft.Json.Linq;
using System.Collections.Generic;
using System.Linq;

namespace SellerCloud.Net.Http.ResponseModels
{
    internal class GenericErrorResponse
    {
        public string? ErrorMessage { get; set; }
        public string? ExceptionMessage { get; set; }
        public string? Message { get; set; }

        public string? ErrorSource { get; set; }
        public string? StackTrace { get; set; }

        public string? Title { get; set; }
        public string? TraceId { get; set; }

        public IDictionary<string, object>? Errors { get; set; }

        public string? RfcError => this.RfcErrors().FirstOrDefault();

        public IEnumerable<string> RfcErrors()
        {
            if (this.Errors is null)
            {
                return Enumerable.Empty<string>();
            }

            bool stringArrays = this.Errors.Values.All(value => value is IEnumerable<string>);
            bool jTokenArrays = this.Errors.Values.All(value => value is IJEnumerable<JToken>);

            if (stringArrays)
            {
                IEnumerable<string> errorMessages = this.Errors.Values
                    .Cast<IEnumerable<string>>()
                    .SelectMany(c => c)
                    .ToList();

                return errorMessages;
            }

            if (jTokenArrays)
            {
                IEnumerable<string> errorMessages = this.Errors.Values
                    .Cast<IJEnumerable<JToken>>()
                    .SelectMany(c => c)
                    .Select(t => t.ToString())
                    .ToList();

                return errorMessages;
            }

            return Enumerable.Empty<string>();

        }
    }
}
