using Microsoft.AspNetCore.Components;
using EnglishMaster.Client.Entities;
using EnglishMaster.Client.Authentication;
using Microsoft.AspNetCore.Components.Authorization;

namespace EnglishMaster.Client.Pages
{
    public partial class Result
    {
        [Parameter]
        [SupplyParameterFromQuery(Name = "level")]
        public long LevelId { get; set; }

        [Parameter]
        [SupplyParameterFromQuery(Name = "part-of-speech")]
        public long PartOfSpeechId { get; set; }

        private List<UserAnswer> _userAnswers = new List<UserAnswer>();

        protected override void OnInitialized()
        {
            _userAnswers = Execute(ResultClientService.GetResults);
        }

        private async Task CloseButtonOnClick()
        {
            await SaveAsync();
            ResultClientService.Reset();
            NavigationManager.NavigateTo("");
        }

        private async Task NextButtonOnClick()
        {
            await SaveAsync();
            NavigationManager.NavigateTo($"study?level={LevelId}&part-of-speech={PartOfSpeechId}&auto-start=true");
        }

        private async Task SaveAsync()
        {
            StateContainer.IsLoading = true;
            AuthenticationState authState = await ExecuteAsync(AuthenticationStateProvider.GetAuthenticationStateAsync);
            if (authState.User.IsInRole(nameof(AccessRole.General)))
            {
                await ExecuteAsync(ResultClientService.SubmitAsync);
            }
            ResultClientService.Reset();
            StateContainer.IsLoading = false;
        }
    }
}
