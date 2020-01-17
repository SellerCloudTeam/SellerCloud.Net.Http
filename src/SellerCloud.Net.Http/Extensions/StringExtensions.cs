namespace SellerCloud.Net.Http.Extensions
{
    public static class StringExtensions
    {
        public static string WithRoute(this string baseUri, string? route)
            => string.IsNullOrWhiteSpace(route)
                ? baseUri
                : $"{baseUri?.TrimEnd('/')}/{route?.TrimStart('/')}";
    }
}
