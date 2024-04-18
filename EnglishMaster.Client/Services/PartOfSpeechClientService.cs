using EnglishMaster.Client.Services.Interfaces;
using EnglishMaster.Shared.Dto.Response;
using EnglishMaster.Shared;
using Newtonsoft.Json;

namespace EnglishMaster.Client.Services
{
    public sealed class PartOfSpeechClientService
    {
        private readonly IHttpClientService _httpClientService;
        private readonly ILogger<PartOfSpeechClientService> _logger;

        public PartOfSpeechClientService(IHttpClientService httpClientService, ILogger<PartOfSpeechClientService> logger)
        {
            _httpClientService = httpClientService;
            _logger = logger;
        }

        public async Task<List<PartOfSpeechResponseDto>> GetPartOfSpeechesAsync()
        {
            HttpResponseResult partOfSpeechesResponse = await _httpClientService.SendAsync(HttpMethod.Get, ApiEndPoint.PART_OF_SPEECH);
            if (partOfSpeechesResponse.StatusCode != System.Net.HttpStatusCode.OK)
            {
                throw new Exception(partOfSpeechesResponse.Message);
            }
            List<PartOfSpeechResponseDto>? partOfSpeeches = JsonConvert.DeserializeObject<List<PartOfSpeechResponseDto>>(partOfSpeechesResponse.Json);
            if (partOfSpeeches == null)
            {
                throw new Exception($"Can not deserialized.{nameof(List<PartOfSpeechResponseDto>)}");
            }
            return partOfSpeeches;
        }
    }
}
