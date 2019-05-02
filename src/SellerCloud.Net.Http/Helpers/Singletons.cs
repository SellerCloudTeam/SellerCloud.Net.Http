using System.Net.Http;

namespace SellerCloud.Net.Http.Helpers
{
    internal static class Singletons
    {
        public static readonly HttpClient DefaultHttpClient = new HttpClient();
    }
}
