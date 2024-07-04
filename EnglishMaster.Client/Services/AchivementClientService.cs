using EnglishMaster.Client.Services.Interfaces;
using EnglishMaster.Shared.Dto.Response;
using EnglishMaster.Shared;
using Newtonsoft.Json;
using System.Net.Http.Json;

namespace EnglishMaster.Client.Services
{
    public sealed class AchivementClientService
    {
        private readonly HttpClient _httpClient;
        private readonly ILogger<AchivementClientService> _logger;
        private readonly ISettingService _settingService;

        public AchivementClientService(HttpClient httpClient, ISettingService settingService, ILogger<AchivementClientService> logger)
        {
            _httpClient = httpClient;
            _settingService = settingService;
            _logger = logger;
        }

        public async Task<List<AchievementResponseDto>> GetAchievementAsync()
        {
            UserSettings userSettings = await _settingService.LoadAsync();
            HttpResponseMessage httpResponseMessage = await _httpClient.GetAsync($"{ApiEndPoint.ACHIEVEMENT}/{userSettings.Mode}");
            if (!httpResponseMessage.IsSuccessStatusCode)
            {
                throw new Exception(await httpResponseMessage.Content.ReadAsStringAsync());
            }
            List<AchievementResponseDto>? achievements =
                await httpResponseMessage.Content.ReadFromJsonAsync<List<AchievementResponseDto>>();
            if (achievements == null)
            {
                throw new Exception($"Can not desirialize object for: {typeof(List<AchievementResponseDto>)}");
            }
            return achievements;
        }

        public async Task<List<AchievementGraphResponseDto>> GetAchievementGraphByWeekAsync()
        {
            UserSettings userSettings = await _settingService.LoadAsync();
            HttpResponseMessage httpResponseMessage =
                await _httpClient.GetAsync(ApiEndPoint.ACHIEVEMENT + $"/car/week/{userSettings.Mode}");
            if (!httpResponseMessage.IsSuccessStatusCode)
            {
                throw new Exception(await httpResponseMessage.Content.ReadAsStringAsync());
            }
            List<AchievementGraphResponseDto>? achievements =
                await httpResponseMessage.Content.ReadFromJsonAsync<List<AchievementGraphResponseDto>>();
            if (achievements == null)
            {
                throw new Exception($"Can not desirialize object for: {typeof(List<AchievementGraphResponseDto>)}");
            }
            return achievements;
        }

        public async Task<List<AchievementGraphResponseDto>> GetAchievementGraphByPartOfSpeechAsync()
        {
            UserSettings userSettings = await _settingService.LoadAsync();
            HttpResponseMessage httpResponseMessage =
                await _httpClient.GetAsync(ApiEndPoint.ACHIEVEMENT + $"/car/part-of-speech/{userSettings.Mode}");
            if (!httpResponseMessage.IsSuccessStatusCode)
            {
                throw new Exception(await httpResponseMessage.Content.ReadAsStringAsync());
            }
            List<AchievementGraphResponseDto>? achievements =
                await httpResponseMessage.Content.ReadFromJsonAsync<List<AchievementGraphResponseDto>>();
            if (achievements == null)
            {
                throw new Exception($"Can not desirialize object for: {typeof(List<AchievementGraphResponseDto>)}");
            }
            return achievements;
        }
    }
}