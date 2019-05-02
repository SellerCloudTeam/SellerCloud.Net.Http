namespace SellerCloud.Net.Http.Models
{
    public class AuthToken
    {
        public AuthToken(string scheme)
        {
            this.Scheme = scheme;
        }

        public AuthToken(string scheme, string parameter)
        {
            this.Scheme = scheme;
            this.Parameter = parameter;
        }

        public string Scheme { get; }
        public string Parameter { get; }

        public static class Schemes
        {
            public const string Bearer = "Bearer";
        }
    }
}
