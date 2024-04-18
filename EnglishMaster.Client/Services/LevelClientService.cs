using EnglishMaster.Client.Services.Interfaces;
using EnglishMaster.Shared.Dto.Response;
using EnglishMaster.Shared;
using Newtonsoft.Json;

namespace EnglishMaster.Client.Services
{
    public sealed class LevelClientService
    {
        private readonly IHttpClientService _httpClientService;
        private readonly ILogger<LevelClientService> _logger;

        public LevelClientService(IHttpClientService httpClientService, ILogger<LevelClientService> logger)
        {
            _httpClientService = httpClientService;
            _logger = logger;
        }

        public async Task<List<LevelResponseDto>> GetLevelsAsync()
        {
            HttpResponseResult levelsResponse = await _httpClientService.SendAsync(HttpMethod.Get, ApiEndPoint.LEVEL);
            if (levelsResponse.StatusCode != System.Net.HttpStatusCode.OK)
            {
                throw new Exception(levelsResponse.Message);
            }
            List<LevelResponseDto>? levels = JsonConvert.DeserializeObject<List<LevelResponseDto>>(levelsResponse.Json);
            if (levels == null)
            {
                throw new Exception($"Can not deserialized.{nameof(List<LevelResponseDto>)}");
            }
            return levels;
        }
    }
}
