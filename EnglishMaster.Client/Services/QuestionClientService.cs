using EnglishMaster.Shared.Dto.Response;
using EnglishMaster.Shared;
using EnglishMaster.Client.Entities;
using System.Net.Http.Json;

namespace EnglishMaster.Client.Services
{
    public sealed class QuestionClientService
    {
        private readonly HttpClient _httpClient;
        private readonly ILogger<QuestionClientService> _logger;
        private long _partOfSpeechId = 0;
        private long _levelId = 0;
        private List<QuestionResponseDto> _questions = new List<QuestionResponseDto>();
        private int _questionIndex = 0;
        private QuestionResponseDto? _currentQuestion;
        private ResultClientService _resultClientService;

        public QuestionClientService(
            HttpClient httpClient, ILogger<QuestionClientService> logger, ResultClientService resultClientService)
        {
            _httpClient = httpClient;
            _logger = logger;
            _resultClientService = resultClientService;
        }

        public async Task InitializeAsync()
        {
            _questionIndex = 0;
            _questions = await GetQuestionsAsync();
        }

        private async Task<List<QuestionResponseDto>> GetQuestionsAsync()
        {
            HttpResponseMessage httpResponseMessage =
                await _httpClient.GetAsync($"{ApiEndPoint.QUESTION}/part-of-speeches/{_partOfSpeechId}/levels/{_levelId}");
            if (!httpResponseMessage.IsSuccessStatusCode)
            {
                throw new Exception(await httpResponseMessage.Content.ReadAsStringAsync());
            }
            List<QuestionResponseDto>? questions =
                await httpResponseMessage.Content.ReadFromJsonAsync<List<QuestionResponseDto>>();
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
                string questionMeaning =
                    _currentQuestion.AnswerResponseDtos
                        .First(a => a.WordId == _currentQuestion.MeaningOfWordId).Meaning;
                _resultClientService.AddResult(
                    new UserAnswer(
                        _currentQuestion.Word,
                        answerMeaning,
                        questionMeaning,
                        answerMeaingOfWordId,
                        _currentQuestion.MeaningOfWordId));
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
