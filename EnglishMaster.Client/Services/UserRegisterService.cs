using EnglishMaster.Client.Services.Interfaces;
using EnglishMaster.Shared;
using EnglishMaster.Shared.Dto.Request;
using Newtonsoft.Json;

namespace EnglishMaster.Client.Services
{
    public sealed class UserRegisterService
    {
        private readonly IHttpClientService _httpClientService;
        private readonly ILogger<UserRegisterService> _logger;

        public UserRegisterService(IHttpClientService httpClientService, ILogger<UserRegisterService> logger)
        {
            _httpClientService = httpClientService;
            _logger = logger;
        }

        public async Task<bool> RegisterAsync(string email, string sub, string firstName, string lastName, string? nickname, string? iconUrl)
        {
            UserReqestDto userReqestDto = new UserReqestDto(email, sub, firstName, lastName, nickname, iconUrl);
            HttpResponseResult httpResponseResult = await _httpClientService.SendAsync(HttpMethod.Post, ApiEndPoint.USER, JsonConvert.SerializeObject(userReqestDto));
            return httpResponseResult.StatusCode == System.Net.HttpStatusCode.Created;
        }
    }
}
