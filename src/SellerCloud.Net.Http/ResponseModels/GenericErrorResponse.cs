namespace SellerCloud.Net.Http.ResponseModels
{
    internal class GenericErrorResponse
    {
        public string? ErrorMessage { get; set; }
        public string? ExceptionMessage { get; set; }
        public string? Message { get; set; }

        public string? ErrorSource { get; set; }
        public string? StackTrace { get; set; }

        public int Status { get; set; }
        public string? Title { get; set; }
        public string? TraceId { get; set; }
        public Errors? Errors { get; set; }
    }

    internal class Errors
    {
        public string[]? OrderId { get; set; }
    }
}
