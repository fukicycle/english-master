namespace EnglishMaster.Client.Services.Interfaces
{
    public interface IHttpClientService
    {
        Task<HttpResponseResult> SendAsync(HttpMethod method, string uri, string? json = null);
        Task<HttpResponseResult> SendWithJWTTokenAsync(HttpMethod method, string uri, string? json = null);
    }
}
