using EnglishMaster.Client.Entities;
using EnglishMaster.Shared.Dto.Response;
using Microsoft.AspNetCore.Components.WebAssembly.Authentication;

namespace EnglishMaster.Client.Pages
{
    public partial class Home
    {
        private LoginUser? _loginUser = null;
        private List<AchievementResponseDto> _achievements = new List<AchievementResponseDto>();

        protected override async Task OnInitializedAsync()
        {
            StateContainer.IsLoading = true;
            bool isAuthenticated = await ExecuteAsync(AuthenticationService.IsAuthenticatedAsync);
            if (isAuthenticated)
            {
                _loginUser = await ExecuteAsync(AuthenticationService.GetLoginUserAsync);
                if (_loginUser == null)
                {
                    NavigationManager.NavigateTo("register");
                    return;
                }
                _achievements = await ExecuteAsync(AchivementClientService.GetAchievementAsync);
            }
            StateContainer.IsLoading = false;
        }

        private void LoginButtonOnClick()
        {
            NavigationManager.NavigateToLogin($"authentication/login?returnUrl={Uri.EscapeDataString(NavigationManager.Uri)}");
        }
    }
}
