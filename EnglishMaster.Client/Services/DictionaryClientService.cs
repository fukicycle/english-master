using EnglishMaster.Client.Services.Interfaces;
using EnglishMaster.Shared;
using EnglishMaster.Shared.Dto.Response;
using Newtonsoft.Json;
using System.Net.Http.Json;

namespace EnglishMaster.Client.Services
{
    public sealed class DictionaryClientService
    {
        private readonly HttpClient _httpClient;
        private readonly ILogger<DictionaryClientService> _logger;
        private readonly List<DictionaryWordResponseDto> _dictionaries = new List<DictionaryWordResponseDto>();

        public DictionaryClientService(HttpClient httpClientService, ILogger<DictionaryClientService> logger)
        {
            _httpClient = httpClientService;
            _logger = logger;
        }

        public async Task<List<DictionaryWordResponseDto>> GetDictionariesAsync()
        {
            _dictionaries.Clear();
            HttpResponseMessage httpResponseMessage = await _httpClient.GetAsync(ApiEndPoint.DICTIONARY);
            if (!httpResponseMessage.IsSuccessStatusCode)
            {
                throw new Exception(await httpResponseMessage.Content.ReadAsStringAsync());
            }
            List<DictionaryWordResponseDto>? dictionaries =
                await httpResponseMessage.Content.ReadFromJsonAsync<List<DictionaryWordResponseDto>>();
            if (dictionaries == null)
            {
                _logger.LogError("Can not desirialized.");
                throw new Exception($"Can not deserialized.{nameof(List<DictionaryWordResponseDto>)}");
            }
            _dictionaries.AddRange(dictionaries);
            return dictionaries.OrderBy(a => a.Word).ToList();
        }

        public List<DictionaryWordResponseDto> Filter(string searchString)
        {
            string searchLowerString = searchString.ToLower();
            return _dictionaries
                        .Where(a => a.Word.ToLower().StartsWith(searchLowerString))
                        .OrderBy(a => a.Word)
                        .ToList();
        }

    }
}