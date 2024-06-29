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
        private readonly ModeClientService _modeClientService;

        public AchivementClientService(HttpClient httpClient, ModeClientService modeClientService, ILogger<AchivementClientService> logger)
        {
            _httpClient = httpClient;
            _modeClientService = modeClientService;
            _logger = logger;
        }

        public async Task<List<AchievementResponseDto>> GetAchievementAsync()
        {
            HttpResponseMessage httpResponseMessage = await _httpClient.GetAsync($"{ApiEndPoint.ACHIEVEMENT}/{_modeClientService.CurrentModeId}");
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
            HttpResponseMessage httpResponseMessage =
                await _httpClient.GetAsync(ApiEndPoint.ACHIEVEMENT + $"/car/week/{_modeClientService.CurrentModeId}");
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
            HttpResponseMessage httpResponseMessage =
                await _httpClient.GetAsync(ApiEndPoint.ACHIEVEMENT + $"/car/part-of-speech/{_modeClientService.CurrentModeId}");
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