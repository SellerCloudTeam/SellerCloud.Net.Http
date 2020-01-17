namespace SellerCloud.Net.Http.Api
{
    public interface ILegacyApiClient
    {
        WebRequestBuilder HttpGet(string route);

        WebRequestBuilder HttpPost(string route);

        WebRequestBuilder HttpPost<TBody>(string route, TBody body)
            where TBody : notnull;

        WebRequestBuilder HttpPut(string route);

        WebRequestBuilder HttpPut<TBody>(string route, TBody body)
            where TBody : notnull;

        WebRequestBuilder HttpDelete(string route);

        WebRequestBuilder HttpDelete<TBody>(string route, TBody body)
            where TBody : notnull;

        WebRequestBuilder HttpPatch<TBody>(string route, TBody body)
            where TBody : notnull;
    }
}