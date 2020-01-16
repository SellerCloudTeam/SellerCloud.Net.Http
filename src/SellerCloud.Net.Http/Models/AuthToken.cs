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
        public string? Parameter { get; }

        public override string ToString()
        {
            return this.Parameter == null
                ? $"{this.Scheme}"
                : $"{this.Scheme} {this.Parameter}";
        }

        public static class Schemes
        {
            public const string Bearer = "Bearer";
        }
    }
}
