namespace SellerCloud.Net.Http.Extensions
{
    public static class StringExtensions
    {
        public static string WithRoute(this string baseUri, string? route)
        {
            if (string.IsNullOrWhiteSpace(route))
            {
                return baseUri;
            }

            return $"{baseUri?.TrimEnd('/')}/{route?.TrimStart('/')}";
        }
    }
}
