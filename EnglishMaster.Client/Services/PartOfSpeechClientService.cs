using EnglishMaster.Client.Services.Interfaces;
using EnglishMaster.Shared.Dto.Response;
using EnglishMaster.Shared;
using Newtonsoft.Json;
using System.Net.Http.Json;

namespace EnglishMaster.Client.Services
{
    public sealed class PartOfSpeechClientService
    {
        private readonly HttpClient _httpClient;
        private readonly ILogger<PartOfSpeechClientService> _logger;

        public PartOfSpeechClientService(HttpClient httpClient, ILogger<PartOfSpeechClientService> logger)
        {
            _httpClient = httpClient;
            _logger = logger;
        }

        public async Task<List<PartOfSpeechResponseDto>> GetPartOfSpeechesAsync()
        {
            HttpResponseMessage httpResponseMessage = await _httpClient.GetAsync(ApiEndPoint.PART_OF_SPEECH);
            if (!httpResponseMessage.IsSuccessStatusCode)
            {
                throw new Exception(await httpResponseMessage.Content.ReadAsStringAsync());
            }
            List<PartOfSpeechResponseDto>? partOfSpeeches =
                await httpResponseMessage.Content.ReadFromJsonAsync<List<PartOfSpeechResponseDto>>();
            if (partOfSpeeches == null)
            {
                throw new Exception($"Can not deserialized.{nameof(List<PartOfSpeechResponseDto>)}");
            }
            return partOfSpeeches;
        }
    }
}
