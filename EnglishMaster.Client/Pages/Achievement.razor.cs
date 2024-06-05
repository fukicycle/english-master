
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





    private List<MyData> Data { get; set; } = new();
    protected override void OnInitialized()
    {
        Data.Add(new MyData { Category = "Jan", NetProfit = 12, Revenue = 33 });
        Data.Add(new MyData { Category = "Feb", NetProfit = 43, Revenue = 42 });
        Data.Add(new MyData { Category = "Mar", NetProfit = 112, Revenue = 23 });
    }

    public class MyData
    {
        public string Category { get; set; }
        public int NetProfit { get; set; }
        public int Revenue { get; set; }
    }
}