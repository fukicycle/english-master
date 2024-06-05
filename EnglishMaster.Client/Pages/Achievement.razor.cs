
using ChartJs.Blazor.LineChart;
using EnglishMaster.Client.Entities;
using Microsoft.AspNetCore.Components.WebAssembly.Authentication;

namespace EnglishMaster.Client.Pages;
public partial class Achievement
{

    private LineConfig? _config = null;
    private LineConfig? _config1 = null;
    private LoginUser? _loginUser = null;

    protected override void OnInitialized()
    {
        Dictionary<string, int> data = new Dictionary<string, int>();
        data.Add("動詞", 85);
        data.Add("名詞", 55);
        data.Add("副詞", 25);
        data.Add("形容詞", 50);
        _config = LineChartClientService.Create(data, "品詞別正答率", "正答率");
        Dictionary<DateTime, int> data1 = new Dictionary<DateTime, int>();
        Enumerable.Range(1, 7).ToList().ForEach(a => data1.Add(DateTime.Today.AddDays(a), Random.Shared.Next(0, 100)));
        _config1 = LineChartClientService.Create(data1, "週間正答率", "正答率");
    }

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
        }
        StateContainer.IsLoading = false;
    }
    private void LoginButtonOnClick()
    {
        NavigationManager.NavigateToLogin($"authentication/login?returnUrl={Uri.EscapeDataString(NavigationManager.Uri)}");
    }
}