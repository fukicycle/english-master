
using EnglishMaster.Shared;
using EnglishMaster.Shared.Dto.Request;
using EnglishMaster.Shared.Dto.Response;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Authorization;
using Newtonsoft.Json;
using System.Net.NetworkInformation;
using Toolbelt.Blazor.SpeechSynthesis;

namespace EnglishMaster.Client.Pages
{
    public partial class Study : PageBase
    {
        [Parameter]
        [SupplyParameterFromQuery(Name = "level")]
        public long LevelId { get; set; } = 0;

        [Parameter]
        [SupplyParameterFromQuery(Name = "part-of-speech")]
        public long PartOfSpeechId { get; set; } = 0;

        [Parameter]
        [SupplyParameterFromQuery(Name = "auto-start")]
        public bool AutoStart { get; set; } = false;

        private long _partOfSpeechId = 0;
        private long _levelId = 0;
        private QuestionResponseDto? _question = null;
        private List<QuestionResponseDto> _questions = new List<QuestionResponseDto>();
        private List<PartOfSpeechResponseDto> _partOfSpeeches = new List<PartOfSpeechResponseDto>();
        private List<LevelResponseDto> _levles = new List<LevelResponseDto>();
        private List<ResultRequestDto> _resultRequestDtos = new List<ResultRequestDto>();
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

        protected override async Task OnParametersSetAsync()
        {
            _partOfSpeechId = PartOfSpeechId;
            _levelId = LevelId;
            if (AutoStart)
            {
                await StartButtonOnClick();
            }
        }

        private async Task GetLevelsAsync()
        {
            HttpResponseResult levelsResponse = await HttpClientService.SendAsync(HttpMethod.Get, ApiEndPoint.LEVEL);
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
            HttpResponseResult partOfSpeechesResponse = await HttpClientService.SendAsync(HttpMethod.Get, ApiEndPoint.PART_OF_SPEECH);
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
            HttpResponseResult questionResponse;
            if (await AuthenticationService.IsAuthenticatedAsync())
            {
                questionResponse = await HttpClientService.SendWithJWTTokenAsync(HttpMethod.Get, $"{ApiEndPoint.QUESTION}/part-of-speeches/{_partOfSpeechId}/levels/{_levelId}");
            }
            else
            {
                questionResponse = await HttpClientService.SendAsync(HttpMethod.Get, $"{ApiEndPoint.QUESTION}/part-of-speeches/{_partOfSpeechId}/levels/{_levelId}");
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
            _questions = questions;
        }

        private async Task StartButtonOnClick()
        {
            try
            {
                _resultRequestDtos.Clear();
                StateContainer.IsLoading = true;
                _questionIndex = 0;
                await GetQuestionsAsync();
                if (!_questions.Any())
                {
                    throw new Exception($"Sorry, This combination({_levles.Single(a => a.Id == _levelId).Name},{_partOfSpeeches.Single(a => a.Id == _partOfSpeechId).Name}) does not have enough questions. Try other combinations.");
                }
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

        private async void OptionButtonOnClick(long wordId)
        {
            if (_question == null)
            {
                StateContainer.Message = "Unexpected error has occured.";
                return;
            }
            _isCorrect = _question.MeaningOfWordId == wordId;
            _isAnswered = true;
            if (await AuthenticationService.IsAuthenticatedAsync())
            {
                _resultRequestDtos.Add(new ResultRequestDto(_question.MeaningOfWordId, wordId));
            }
        }

        private async Task NextButtonOnClick()
        {
            try
            {
                _isAnswered = false;
                _questionIndex++;
                if (_questionIndex >= _questions.Count)
                {
                    if (await AuthenticationService.IsAuthenticatedAsync())
                    {
                        StateContainer.IsLoading = true;
                        int numberOfRegistered = await SubmitResult();
                        StateContainer.IsLoading = false;
                        NavigationManager.NavigateTo($"result?count={numberOfRegistered}&level={_levelId}&part-of-speech={_partOfSpeechId}");
                    }
                    else
                    {
                        _question = null;
                        _questionIndex = 0;
                    }
                }
                else
                {
                    _question = _questions[_questionIndex];
                }
            }
            catch (Exception ex)
            {
                StateContainer.Message = ex.Message;
            }
        }

        private async Task<int> SubmitResult()
        {
            HttpResponseResult resultResponse = await HttpClientService.SendWithJWTTokenAsync(HttpMethod.Post, ApiEndPoint.RESULT, JsonConvert.SerializeObject(_resultRequestDtos));
            if (resultResponse.StatusCode != System.Net.HttpStatusCode.OK)
            {
                throw new Exception(resultResponse.Message);
            }
            int? result = JsonConvert.DeserializeObject<int>(resultResponse.Json);
            if (result == null)
            {
                throw new Exception($"Can not deserialized.{nameof(List<PartOfSpeechResponseDto>)}");
            }
            return result.Value;
        }

        private double GetProgressValue()
        {
            return _questionIndex * 100.0 / _questions.Count;
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
