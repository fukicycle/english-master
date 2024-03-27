using EnglishMaster.Client.Services.Interfaces;
using EnglishMaster.Shared;
using Microsoft.AspNetCore.Components;

namespace EnglishMaster.Client.Services
{
    public sealed class HttpClientService : IHttpClientService
    {
        private readonly HttpClient _httpClient;
        private readonly ISettingService _settingService;
        private readonly ILogger<HttpClientService> _logger;
        private readonly NavigationManager _navigationManager;
        public HttpClientService(IHttpClientFactory httpClientFactory, ISettingService settingService, ILogger<HttpClientService> logger, NavigationManager navigationManager)
        {
            _settingService = settingService;
            _httpClient = httpClientFactory.CreateClient(ApplicationSettings.Mode.ToString());
            _logger = logger;
            _navigationManager = navigationManager;
        }
        public async Task<HttpResponseResult> SendAsync(HttpMethod method, string uri, string? json = null)
        {
            try
            {
                if (_settingService.JWTToken != null)
                {
                    _httpClient.DefaultRequestHeaders.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", _settingService.JWTToken);
                }
                HttpRequestMessage httpRequestMessage = new HttpRequestMessage(method, uri);
                if (json != null)
                {
                    StringContent content = new StringContent(json, System.Text.Encoding.UTF8, "application/json");
                    httpRequestMessage.Content = content;
                }
                HttpResponseMessage httpResponseMessage = await _httpClient.SendAsync(httpRequestMessage);
                string responseContent = await httpResponseMessage.Content.ReadAsStringAsync();
                if (httpResponseMessage.StatusCode == System.Net.HttpStatusCode.Forbidden)
                {
                    return new HttpResponseResult(string.Empty, System.Net.HttpStatusCode.Forbidden, "ユーザが登録されていません。利用申請してください。");
                }
                if (httpResponseMessage.StatusCode == System.Net.HttpStatusCode.Unauthorized)
                {
                    _navigationManager.NavigateTo($"?redirect={_navigationManager.Uri}");
                    //再度認証をかけるのでOKで返す。
                    return new HttpResponseResult(string.Empty, System.Net.HttpStatusCode.OK);
                }
                if (httpResponseMessage.StatusCode == System.Net.HttpStatusCode.NotFound)
                {
                    return new HttpResponseResult(string.Empty, System.Net.HttpStatusCode.NotFound, responseContent);
                }
                if (httpResponseMessage.StatusCode == System.Net.HttpStatusCode.Created)
                {
                    return new HttpResponseResult(string.Empty, System.Net.HttpStatusCode.Created, responseContent);
                }
                if (httpResponseMessage.IsSuccessStatusCode)
                {
                    return new HttpResponseResult(responseContent, System.Net.HttpStatusCode.OK);
                }
                throw new Exception(responseContent);
            }
            catch (Exception ex)
            {
                return new HttpResponseResult(string.Empty, System.Net.HttpStatusCode.InternalServerError, ex.Message);
            }
        }
    }
}
