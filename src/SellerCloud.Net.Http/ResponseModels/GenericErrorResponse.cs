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
        
    }
}
