using EnglishMaster.Client.Services.Interfaces;
using EnglishMaster.Shared.Dto.Response;
using EnglishMaster.Shared;
using Newtonsoft.Json;
using EnglishMaster.Shared.Dto.Request;
using EnglishMaster.Client.Entities;

namespace EnglishMaster.Client.Services
{
    public sealed class QuestionClientService
    {
        private readonly IHttpClientService _httpClientService;
        private readonly ILogger<QuestionClientService> _logger;
        private readonly IAuthenticationService _authenticationService;
        private long _partOfSpeechId = 0;
        private long _levelId = 0;
        private List<QuestionResponseDto> _questions = new List<QuestionResponseDto>();
        private int _questionIndex = 0;
        private QuestionResponseDto? _currentQuestion;
        private ResultClientService _resultClientService;

        public QuestionClientService(IHttpClientService httpClientService, ILogger<QuestionClientService> logger, IAuthenticationService authenticationService, ResultClientService resultClientService)
        {
            _httpClientService = httpClientService;
            _logger = logger;
            _authenticationService = authenticationService;
            _resultClientService = resultClientService;
        }

        public async Task InitializeAsync()
        {
            _questionIndex = 0;
            _questions = await GetQuestionsAsync();
        }

        private async Task<List<QuestionResponseDto>> GetQuestionsAsync()
        {
            HttpResponseResult questionResponse;
            if (await _authenticationService.IsAuthenticatedAsync())
            {
                questionResponse = await _httpClientService.SendWithJWTTokenAsync(HttpMethod.Get, $"{ApiEndPoint.QUESTION}/part-of-speeches/{_partOfSpeechId}/levels/{_levelId}");
            }
            else
            {
                questionResponse = await _httpClientService.SendAsync(HttpMethod.Get, $"{ApiEndPoint.QUESTION}/part-of-speeches/{_partOfSpeechId}/levels/{_levelId}");
            }
            if (questionResponse.StatusCode != System.Net.HttpStatusCode.OK)
            {
                throw new Exception(questionResponse.Message);
            }
            List<QuestionResponseDto>? questions = JsonConvert.DeserializeObject<List<QuestionResponseDto>>(questionResponse.Json);
            if (questions == null)
            {
                throw new Exception($"Can not deserialized.{nameof(List<QuestionResponseDto>)}");
            }
            if (questions.Count == 0)
            {
                throw new Exception("Sorry, This combination does not have enough questions. Try other combinations.");
            }
            return questions;
        }

        public bool Verify(long answerMeaingOfWordId, string answerMeaning)
        {
            bool isCorrect = _currentQuestion?.MeaningOfWordId == answerMeaingOfWordId;
            if (_currentQuestion != null)
            {
                string questionMeaning = _currentQuestion.AnswerResponseDtos.First(a => a.WordId == _currentQuestion.MeaningOfWordId).Meaning;
                _resultClientService.AddResult(new UserAnswer(_currentQuestion.Word, answerMeaning, questionMeaning, answerMeaingOfWordId, _currentQuestion.MeaningOfWordId));
            }
            return isCorrect;
        }

        public void SetPartOfSpeechId(long id)
        {
            _partOfSpeechId = id;
        }

        public void SetLevelId(long id)
        {
            _levelId = id;
        }

        public QuestionResponseDto? GetQuestion(out int questionIndex)
        {
            if (_questions.Count <= _questionIndex)
            {
                questionIndex = 0;
                return null;
            }
            _currentQuestion = _questions[_questionIndex];
            _questionIndex++;
            questionIndex = _questionIndex;
            return _currentQuestion;
        }
    }
}
