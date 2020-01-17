namespace SellerCloud.Net.Http.Api
{
    public interface IHttpApiClient
    {
        HttpRequestBuilder HttpGet(string route);

        HttpRequestBuilder HttpPost(string route);

        HttpRequestBuilder HttpPost<TBody>(string route, TBody body)
            where TBody : notnull;

        HttpRequestBuilder HttpPut(string route);

        HttpRequestBuilder HttpPut<TBody>(string route, TBody body)
            where TBody : notnull;

        HttpRequestBuilder HttpDelete(string route);

        HttpRequestBuilder HttpDelete<TBody>(string route, TBody body)
            where TBody : notnull;

        HttpRequestBuilder HttpPatch(string route);

        HttpRequestBuilder HttpPatch<TBody>(string route, TBody body)
            where TBody : notnull;
    }
}