using EnglishMaster.Client.Authentication;
using EnglishMaster.Client.Entities;
using EnglishMaster.Shared.Dto.Response;
using Microsoft.AspNetCore.Components.Authorization;
using Microsoft.AspNetCore.Components.WebAssembly.Authentication;

namespace EnglishMaster.Client.Pages
{
    public partial class Home
    {
        private List<AchievementResponseDto> _achievements = new List<AchievementResponseDto>();
        private string _treeImagePath = "process/tree_01.png";
        private int _level = 1;
        private string _displayName = "";

        protected override async Task OnInitializedAsync()
        {
            StateContainer.IsLoading = true;
            AuthenticationState authState = await ExecuteAsync(AuthenticationStateProvider.GetAuthenticationStateAsync);
            if (authState.User.IsInRole(nameof(AccessRole.General)))
            {
                string fullName = AuthenticationStateProvider.LoginUser!.FirstName + " " + AuthenticationStateProvider.LoginUser!.LastName;
                string? nickName = AuthenticationStateProvider.LoginUser!.Nickname;
                _displayName = nickName ?? fullName;
                _achievements = await ExecuteAsync(AchivementClientService.GetAchievementAsync);
                await ExecuteAsync(GetTreeImagePathAsync);
            }
            StateContainer.IsLoading = false;
        }

        private void LoginButtonOnClick()
        {
            NavigationManager.NavigateToLogin("authentication/login");
        }

        private async Task GetTreeImagePathAsync()
        {
            if (!await TreeFarmService.IsEnabledTreeFarmAsync())
            {
                await TreeFarmService.SetStartDateAsync();
            }
            _level = await TreeFarmService.GetTreeLevelAsync();
            _treeImagePath = TreeFarmService.GenerateTreeImagePath(_level);
            StateHasChanged();
        }
    }
}
