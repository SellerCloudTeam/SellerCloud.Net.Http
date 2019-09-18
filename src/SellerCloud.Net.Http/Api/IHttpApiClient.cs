namespace SellerCloud.Net.Http.Api
{
    public interface IHttpApiClient
    {
        HttpRequestBuilder HttpGet(string route);

        HttpRequestBuilder HttpPost(string route);

        HttpRequestBuilder HttpPost<TBody>(string route, TBody body);

        HttpRequestBuilder HttpPut(string route);

        HttpRequestBuilder HttpPut<TBody>(string route, TBody body);

        HttpRequestBuilder HttpDelete(string route);
    }
}