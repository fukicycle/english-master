using EnglishMaster.Client.Services.Interfaces;
using EnglishMaster.Shared.Dto.Response;
using EnglishMaster.Shared;
using Newtonsoft.Json;

namespace EnglishMaster.Client.Services
{
    public sealed class AchivementClientService
    {
        private readonly IHttpClientService _httpClientService;
        private readonly ILogger<AchivementClientService> _logger;

        public AchivementClientService(IHttpClientService httpClientService, ILogger<AchivementClientService> logger)
        {
            _httpClientService = httpClientService;
            _logger = logger;
        }

        public async Task<List<AchievementResponseDto>> GetAchievementAsync()
        {
            HttpResponseResult httpResponseResult = await _httpClientService.SendWithJWTTokenAsync(HttpMethod.Get, ApiEndPoint.ACHIEVEMENT);
            if (httpResponseResult.StatusCode != System.Net.HttpStatusCode.OK)
            {
                throw new Exception(httpResponseResult.Message);
            }
            List<AchievementResponseDto>? achievements = JsonConvert.DeserializeObject<List<AchievementResponseDto>>(httpResponseResult.Json);
            if (achievements == null)
            {
                throw new Exception($"Can not desirialize object for: {typeof(List<AchievementResponseDto>)}");
            }
            return achievements;
        }

        public async Task<List<AchievementGraphResponseDto>> GetAchievementGraphByWeekAsync()
        {
            HttpResponseResult httpResponseResult = await _httpClientService.SendWithJWTTokenAsync(HttpMethod.Get, ApiEndPoint.ACHIEVEMENT + "/car/week");
            if (httpResponseResult.StatusCode != System.Net.HttpStatusCode.OK)
            {
                throw new Exception(httpResponseResult.Message);
            }
            List<AchievementGraphResponseDto>? achievements = JsonConvert.DeserializeObject<List<AchievementGraphResponseDto>>(httpResponseResult.Json);
            if (achievements == null)
            {
                throw new Exception($"Can not desirialize object for: {typeof(List<AchievementGraphResponseDto>)}");
            }
            return achievements;
        }

        public async Task<List<AchievementGraphResponseDto>> GetAchievementGraphByPartOfSpeechAsync()
        {
            HttpResponseResult httpResponseResult = await _httpClientService.SendWithJWTTokenAsync(HttpMethod.Get, ApiEndPoint.ACHIEVEMENT + "/car/part-of-speech");
            if (httpResponseResult.StatusCode != System.Net.HttpStatusCode.OK)
            {
                throw new Exception(httpResponseResult.Message);
            }
            List<AchievementGraphResponseDto>? achievements = JsonConvert.DeserializeObject<List<AchievementGraphResponseDto>>(httpResponseResult.Json);
            if (achievements == null)
            {
                throw new Exception($"Can not desirialize object for: {typeof(List<AchievementGraphResponseDto>)}");
            }
            return achievements;
        }
    }
}
