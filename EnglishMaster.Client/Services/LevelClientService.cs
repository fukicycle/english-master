using EnglishMaster.Client.Services.Interfaces;
using EnglishMaster.Shared.Dto.Response;
using EnglishMaster.Shared;
using Newtonsoft.Json;
using System.Net.Http.Json;

namespace EnglishMaster.Client.Services
{
    public sealed class LevelClientService
    {
        private readonly HttpClient _httpClient;
        private readonly ILogger<LevelClientService> _logger;

        public LevelClientService(HttpClient httpClient, ILogger<LevelClientService> logger)
        {
            _httpClient = httpClient;
            _logger = logger;
        }

        public async Task<List<LevelResponseDto>> GetLevelsAsync()
        {
            HttpResponseMessage httpResponseMessage = await _httpClient.GetAsync(ApiEndPoint.LEVEL);
            if (!httpResponseMessage.IsSuccessStatusCode)
            {
                throw new Exception(await httpResponseMessage.Content.ReadAsStringAsync());
            }
            List<LevelResponseDto>? levels = 
                await httpResponseMessage.Content.ReadFromJsonAsync<List<LevelResponseDto>>();
            if (levels == null)
            {
                throw new Exception($"Can not deserialized.{nameof(List<LevelResponseDto>)}");
            }
            return levels;
        }
    }
}
