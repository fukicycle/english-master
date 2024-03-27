using EnglishMaster.Client.Services.Interfaces;
using EnglishMaster.Shared;
using EnglishMaster.Shared.Dto.Request;
using EnglishMaster.Shared.Dto.Response;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Authorization;
using Newtonsoft.Json;

namespace EnglishMaster.Client.Services
{
    public sealed class HttpClientService : IHttpClientService
    {
        private readonly HttpClient _httpClient;
        private readonly ISettingService _settingService;
        private readonly AuthenticationStateProvider _authenticationStateProvider;
        private readonly ILogger<HttpClientService> _logger;
        public HttpClientService(IHttpClientFactory httpClientFactory, ISettingService settingService, AuthenticationStateProvider authenticationStateProvider, ILogger<HttpClientService> logger)
        {
            _settingService = settingService;
            _httpClient = httpClientFactory.CreateClient(ApplicationSettings.Mode.ToString());
            _authenticationStateProvider = authenticationStateProvider;
            _logger = logger;
        }
        public async Task<HttpResponseResult> SendAsync(HttpMethod method, string uri, string? json = null)
        {
            try
            {
                if (!string.IsNullOrEmpty(_settingService.JWTToken))
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
                if (httpResponseMessage.StatusCode == System.Net.HttpStatusCode.Unauthorized)
                {
                    _logger.LogInformation($"Token[{typeof(HttpClientService)}]:" + _settingService.JWTToken);
                    return new HttpResponseResult("Token is missing or invalid token.", System.Net.HttpStatusCode.Unauthorized);
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

        public async Task<HttpResponseResult> SendWithJWTTokenAsync(HttpMethod method, string uri, string? json = null)
        {
            if (string.IsNullOrEmpty(_settingService.JWTToken))
            {
                await ApiAuthentication();
            }
            _httpClient.DefaultRequestHeaders.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", _settingService.JWTToken);
            return await SendAsync(method, uri, json);
        }

        private async Task ApiAuthentication()
        {
            AuthenticationState authenticationState = await _authenticationStateProvider.GetAuthenticationStateAsync();
            string email = authenticationState.User.Claims.FirstOrDefault(a => a.Type == "email")?.Value ?? "";
            string subAsPassword = authenticationState.User.Claims.FirstOrDefault(a => a.Type == "sub")?.Value ?? "";
            LoginRequestDto loginRequestDto = new LoginRequestDto(email, subAsPassword);
            HttpResponseResult result = await SendAsync(HttpMethod.Post, ApiEndPoint.LOGIN, JsonConvert.SerializeObject(loginRequestDto));
            if (result.StatusCode != System.Net.HttpStatusCode.OK)
            {
                throw new Exception(result.Message ?? "Unexpected error has occured.");
            }
            LoginResponseDto? loginResponseDto = JsonConvert.DeserializeObject<LoginResponseDto>(result.Json);
            if (loginResponseDto == null)
            {
                throw new Exception($"Can not deserialize {typeof(LoginResponseDto)}");
            }
            _logger.LogInformation($"Token[{nameof(ApiAuthentication)}]:" + loginResponseDto.Token);
            _settingService.JWTToken = loginResponseDto.Token;
        }
    }
}
