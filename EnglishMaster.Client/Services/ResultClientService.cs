using EnglishMaster.Client.Entities;
using EnglishMaster.Client.Services.Interfaces;
using EnglishMaster.Shared;
using EnglishMaster.Shared.Dto.Request;
using Newtonsoft.Json;
using System.Net.Http.Json;

namespace EnglishMaster.Client.Services
{
    public sealed class ResultClientService
    {
        private readonly HttpClient _httpClient;
        private readonly ILogger<ResultClientService> _logger;
        private List<UserAnswer> _userAnswers = new List<UserAnswer>();
        private readonly ISettingService _settingService;

        public ResultClientService(HttpClient httpClient, ISettingService settingService, ILogger<ResultClientService> logger)
        {
            _httpClient = httpClient;
            _settingService = settingService;
            _logger = logger;
        }

        public void AddResult(UserAnswer userAnswer)
        {
            _userAnswers.Add(userAnswer);
        }

        public async Task SubmitAsync()
        {
            UserSettings userSettings = await _settingService.LoadAsync();
            List<ResultRequestDto> resultRequestDtos =
                _userAnswers.Select(a => new ResultRequestDto(a.QuestionMeaningOfWordId, a.QuestionMeaning == a.AnswerMeaning, userSettings.Mode))
                            .ToList();
            HttpResponseMessage httpResponseMessage =
                await _httpClient.PostAsJsonAsync(ApiEndPoint.RESULT, _userAnswers);
            if (!httpResponseMessage.IsSuccessStatusCode)
            {
                throw new Exception(await httpResponseMessage.Content.ReadAsStringAsync());
            }
        }

        public List<UserAnswer> GetResults()
        {
            return _userAnswers;
        }

        public string GetScore()
        {
            if (_userAnswers.Any())
            {
                int numberOfCorrect = _userAnswers.Count(a => a.AnswerMeaning == a.QuestionMeaning);
                int totalCount = _userAnswers.Count;
                return Math.Round(numberOfCorrect * 100.0 / totalCount, 0).ToString("");
            }
            return "-";
        }

        public void Reset()
        {
            _userAnswers = new List<UserAnswer>();
        }
    }
}
