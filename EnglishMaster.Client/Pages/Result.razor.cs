using EnglishMaster.Shared.Dto.Response;
using EnglishMaster.Shared;
using Microsoft.AspNetCore.Components;
using Newtonsoft.Json;

namespace EnglishMaster.Client.Pages
{
    public partial class Result : PageBase
    {
        [Parameter]
        [SupplyParameterFromQuery]
        public int Count { get; set; }

        [Parameter]
        [SupplyParameterFromQuery(Name = "level")]
        public long LevelId { get; set; }

        [Parameter]
        [SupplyParameterFromQuery(Name = "part-of-speech")]
        public long PartOfSpeechId { get; set; }

        private List<ResultResponseDto> _results = new List<ResultResponseDto>();

        protected override async Task OnInitializedAsync()
        {
            try
            {
                StateContainer.IsLoading = true;
                await GetResultAsync();
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

        private void CloseButtonOnClick()
        {
            NavigationManager.NavigateTo("");
        }

        private void NextButtonOnClick()
        {
            NavigationManager.NavigateTo($"study?level={LevelId}&part-of-speech={PartOfSpeechId}&auto-start=true");
        }

        private async Task GetResultAsync()
        {
            HttpResponseResult resultsResponse = await HttpClientService.SendWithJWTTokenAsync(HttpMethod.Get, ApiEndPoint.RESULT + "?count=" + Count);
            if (resultsResponse.StatusCode != System.Net.HttpStatusCode.OK)
            {
                throw new Exception(resultsResponse.Message);
            }
            List<ResultResponseDto>? resultResponse = JsonConvert.DeserializeObject<List<ResultResponseDto>>(resultsResponse.Json);
            if (resultResponse == null)
            {
                throw new Exception($"Can not deserialized.{nameof(List<LevelResponseDto>)}");
            }
            _results = resultResponse;
        }

        private string GetScore()
        {
            if (_results.Any())
            {
                int numberOfCorrect = _results.Count(a => a.UserAnswer == a.CorrectAnswer);
                int totalCount = _results.Count;
                return Math.Round(numberOfCorrect * 100.0 / totalCount, 0).ToString("");
            }
            return "-";
        }
    }
}
