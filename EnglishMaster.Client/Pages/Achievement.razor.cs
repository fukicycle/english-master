
using EnglishMaster.Client.Entities;
using Microsoft.AspNetCore.Components.WebAssembly.Authentication;

namespace EnglishMaster.Client.Pages;
public partial class Achievement
{
    private LoginUser? _loginUser = null;
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
            }
            // _achievements = await ExecuteAsync(AchivementClientService.GetAchievementAsync);
        }
        StateContainer.IsLoading = false;
    }
    private void LoginButtonOnClick()
    {
        NavigationManager.NavigateToLogin($"authentication/login?returnUrl={Uri.EscapeDataString(NavigationManager.Uri)}");
    }
}