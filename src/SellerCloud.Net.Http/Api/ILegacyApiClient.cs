namespace SellerCloud.Net.Http.Api
{
    public interface ILegacyApiClient
    {
        WebRequestBuilder HttpGet(string route);

        WebRequestBuilder HttpPost(string route);

        WebRequestBuilder HttpPost<TBody>(string route, TBody body);

        WebRequestBuilder HttpPut(string route);

        WebRequestBuilder HttpPut<TBody>(string route, TBody body);

        WebRequestBuilder HttpDelete(string route);
    }
}