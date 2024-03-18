
using System.Net.NetworkInformation;
using EnglishMaster.Shared;
using EnglishMaster.Shared.Dto.Response;
using EnglishMaster.Shared.Models;
using Newtonsoft.Json;
using Toolbelt.Blazor.SpeechSynthesis;

namespace EnglishMaster.Client.Pages
{
    public partial class Study : PageBase
    {
        private long _partOfSpeechId = 0;
        private long _levelId = 0;
        private QuestionResponseDto? _question = null;
        private List<QuestionResponseDto> _questions = new List<QuestionResponseDto>();
        private List<PartOfSpeechResponseDto> _partOfSpeeches = new List<PartOfSpeechResponseDto>();
        private List<LevelResponseDto> _levles = new List<LevelResponseDto>();
        private int _questionIndex = 0;
        private bool _isAnswered = false;
        private bool _isCorrect = false;

        protected override async Task OnInitializedAsync()
        {
            try
            {
                StateContainer.IsLoading = true;
                await GetPartOfSpeechesAsync();
                await GetLevelsAsync();
            }
            catch (Exception ex)
            {
                StateContainer.Message = ex.Message;
            }
            finally
            {
                StateContainer.IsLoading = false;
            }
        }

        private async Task GetLevelsAsync()
        {
            HttpResponseResult levelsResponse = await HttpClientService.SendAsync(HttpMethod.Get, "/api/v1/levels");
            if (levelsResponse.StatusCode != System.Net.HttpStatusCode.OK)
            {
                throw new Exception(levelsResponse.Message);
            }
            List<LevelResponseDto>? levels = JsonConvert.DeserializeObject<List<LevelResponseDto>>(levelsResponse.Json);
            if (levels == null)
            {
                throw new Exception($"Can not deserialized.{nameof(List<LevelResponseDto>)}");
            }
            _levles = levels;
        }

        private async Task GetPartOfSpeechesAsync()
        {
            HttpResponseResult partOfSpeechesResponse = await HttpClientService.SendAsync(HttpMethod.Get, "/api/v1/part-of-speeches");
            if (partOfSpeechesResponse.StatusCode != System.Net.HttpStatusCode.OK)
            {
                throw new Exception(partOfSpeechesResponse.Message);
            }
            List<PartOfSpeechResponseDto>? partOfSpeeches = JsonConvert.DeserializeObject<List<PartOfSpeechResponseDto>>(partOfSpeechesResponse.Json);
            if (partOfSpeeches == null)
            {
                throw new Exception($"Can not deserialized.{nameof(List<PartOfSpeechResponseDto>)}");
            }
            _partOfSpeeches = partOfSpeeches;
        }

        private async Task GetQuestionsAsync()
        {
            HttpResponseResult questionResponse = await HttpClientService.SendAsync(HttpMethod.Get, $"/api/v1/questions/part-of-speeches/{_partOfSpeechId}/levels/{_levelId}");
            if (questionResponse.StatusCode != System.Net.HttpStatusCode.OK)
            {
                throw new Exception(questionResponse.Message);
            }
            List<QuestionResponseDto>? questions = JsonConvert.DeserializeObject<List<QuestionResponseDto>>(questionResponse.Json);
            if (questions == null)
            {
                throw new Exception($"Can not deserialized.{nameof(List<QuestionResponseDto>)}");
            }
            _questions = questions;
        }

        private async Task StartButtonOnClick()
        {
            try
            {
                StateContainer.IsLoading = true;
                _questionIndex = 0;
                await GetQuestionsAsync();
                _question = _questions[_questionIndex];
            }
            catch (Exception ex)
            {
                StateContainer.Message = ex.Message;
            }
            finally
            {
                StateContainer.IsLoading = false;
            }
        }

        private void OptionButtonOnClick(long wordId)
        {
            if (_question == null)
            {
                StateContainer.Message = "Unexpected error has occured.";
                return;
            }
            _isCorrect = _question.MeaningOfWordId == wordId;
            _isAnswered = true;
        }

        private void NextButtonOnClick()
        {
            _isAnswered = false;
            _questionIndex++;
            if (_questionIndex >= _questions.Count)
            {
                _questionIndex = 0;
                _question = null;
            }
            else
            {
                _question = _questions[_questionIndex];
            }
        }

        private async Task SoundButtonOnClick()
        {
            if (_question == null)
            {
                StateContainer.Message = "Unexpected error has occured.";
                return;
            }
            var utterancet = new SpeechSynthesisUtterance
            {
                Text = _question.Word,
                Lang = "en-US", // BCP 47 language tag
                Pitch = 1.0, // 0.0 ~ 2.0 (Default 1.0)
                Rate = 1.0, // 0.1 ~ 10.0 (Default 1.0)
                Volume = 1.0 // 0.0 ~ 1.0 (Default 1.0)
            };
            await SpeechSynthesis.SpeakAsync(utterancet);
        }
    }
}
