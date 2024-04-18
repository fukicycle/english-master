using EnglishMaster.Client.Entities;
using EnglishMaster.Client.Services.Interfaces;
using EnglishMaster.Shared;
using EnglishMaster.Shared.Dto.Request;
using Newtonsoft.Json;

namespace EnglishMaster.Client.Services
{
    public sealed class ResultClientService
    {
        private readonly IHttpClientService _httpClientService;
        private readonly ILogger<ResultClientService> _logger;
        private List<UserAnswer> _userAnswers = new List<UserAnswer>();

        public ResultClientService(IHttpClientService httpClientService, ILogger<ResultClientService> logger)
        {
            _httpClientService = httpClientService;
            _logger = logger;
        }

        public void AddResult(UserAnswer userAnswer)
        {
            _userAnswers.Add(userAnswer);
        }

        public async Task Submit()
        {
            List<ResultRequestDto> resultRequestDtos = _userAnswers.Select(a => new ResultRequestDto(a.QuestionMeaningOfWordId, a.AnswerMeaningOfWordId)).ToList();
            HttpResponseResult resultResponse = await _httpClientService.SendWithJWTTokenAsync(HttpMethod.Post, ApiEndPoint.RESULT, JsonConvert.SerializeObject(_userAnswers));
            if (resultResponse.StatusCode != System.Net.HttpStatusCode.OK)
            {
                throw new Exception(resultResponse.Message);
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
                int numberOfCorrect = _userAnswers.Count(a => a.QuestionMeaningOfWordId == a.AnswerMeaningOfWordId);
                int totalCount = _userAnswers.Count;
                return Math.Round(numberOfCorrect * 100.0 / totalCount, 0).ToString("");
            }
            return "-";
        }
    }
}
