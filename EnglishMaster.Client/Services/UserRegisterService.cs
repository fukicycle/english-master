using EnglishMaster.Client.Services.Interfaces;
using EnglishMaster.Shared;
using EnglishMaster.Shared.Dto.Request;
using Newtonsoft.Json;
using System.Net.Http.Json;

namespace EnglishMaster.Client.Services
{
    public sealed class UserRegisterService
    {
        private readonly HttpClient _httpClient;
        private readonly ILogger<UserRegisterService> _logger;

        public UserRegisterService(HttpClient httpClient, ILogger<UserRegisterService> logger)
        {
            _httpClient = httpClient;
            _logger = logger;
        }

        public async Task<bool> RegisterAsync(string email, string sub, string firstName, string lastName, string? nickname, string? iconUrl)
        {
            UserReqestDto userReqestDto = new UserReqestDto(email, sub, firstName, lastName, nickname, iconUrl);
            HttpResponseMessage httpResponseMessage =
                await _httpClient.PostAsJsonAsync(ApiEndPoint.USER, userReqestDto);
            return httpResponseMessage.StatusCode == System.Net.HttpStatusCode.Created;
        }
    }
}
