
using EnglishMaster.Shared;
using EnglishMaster.Shared.Dto.Response;
using EnglishMaster.Shared.Models;
using Newtonsoft.Json;

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

        protected override async Task OnInitializedAsync()
        {
            try
            {
                StateContainer.IsLoading = true;
                await GetPartOfSpeechesAsync();
                await GetLevelsAsync();
                await RefreshQuestionAsync();
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

        private async Task RefreshQuestionAsync()
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
                await RefreshQuestionAsync();
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
    }
}
