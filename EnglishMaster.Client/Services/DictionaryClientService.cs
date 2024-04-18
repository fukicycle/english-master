using EnglishMaster.Client.Services.Interfaces;
using EnglishMaster.Shared;
using EnglishMaster.Shared.Dto.Response;
using Newtonsoft.Json;

namespace EnglishMaster.Client.Services
{
    public sealed class DictionaryClientService
    {
        private readonly IHttpClientService _httpClientService;
        private readonly ILogger<DictionaryClientService> _logger;
        private readonly List<DictionaryWordResponseDto> _dictionaries = new List<DictionaryWordResponseDto>();

        public DictionaryClientService(IHttpClientService httpClientService, ILogger<DictionaryClientService> logger)
        {
            _httpClientService = httpClientService;
            _logger = logger;
        }

        public async Task<List<DictionaryWordResponseDto>> GetDictionariesAsync()
        {
            _dictionaries.Clear();
            HttpResponseResult dictionaryResult = await _httpClientService.SendAsync(HttpMethod.Get, ApiEndPoint.DICTIONARY);
            if (dictionaryResult.StatusCode != System.Net.HttpStatusCode.OK)
            {
                _logger.LogError("Error: {0},Status Code: {1}", dictionaryResult.Message, dictionaryResult.StatusCode);
                throw new Exception(dictionaryResult.Message);
            }
            List<DictionaryWordResponseDto>? dictionaries = JsonConvert.DeserializeObject<List<DictionaryWordResponseDto>>(dictionaryResult.Json);
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
            return _dictionaries.Where(a => a.Word.ToLower().StartsWith(searchLowerString)).OrderBy(a => a.Word).ToList();
        }

    }
}
