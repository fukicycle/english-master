
using ChartJs.Blazor.LineChart;
using ChartJs.Blazor.RadarChart;
using EnglishMaster.Client.Entities;
using EnglishMaster.Shared.Dto.Response;
using Microsoft.AspNetCore.Components.WebAssembly.Authentication;

namespace EnglishMaster.Client.Pages;
public partial class Achievement
{

    private RadarConfig? _config = null;
    private LineConfig? _config1 = null;
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
            //TODO implementation
            //await ExecuteAsync(GenerateUserDataAsync);
            GenerateSampleData();
        }
        else
        {
            GenerateSampleData();
        }
        StateContainer.IsLoading = false;
    }
    private void LoginButtonOnClick()
    {
        NavigationManager.NavigateToLogin($"authentication/login?returnUrl={Uri.EscapeDataString(NavigationManager.Uri)}");
    }

    private void GenerateSampleData()
    {
        Dictionary<string, decimal> data = new Dictionary<string, decimal>
        {
            { "動詞", GenerateRandomValue() },
            { "名詞", GenerateRandomValue()},
            { "副詞", GenerateRandomValue() },
            { "形容詞", GenerateRandomValue() },
            { "助詞", GenerateRandomValue() }
        };
        _config = RadarChartClientService.Create(data, "Correct answer rate by part of speech", "Correct answer rate");
        Dictionary<DateTime, decimal> data1 = new Dictionary<DateTime, decimal>();
        Enumerable.Range(-8, 7).ToList().ForEach(a => data1.Add(DateTime.Today.AddDays(a), GenerateRandomValue()));
        _config1 = LineChartClientService.Create(data1, "Correct answer rate by last week", "Correct answer rate");
    }

    private decimal GenerateRandomValue()
    {
        int randomValue = Random.Shared.Next(0, 100);
        decimal rate = decimal.Parse(Random.Shared.NextDouble().ToString());
        return randomValue * rate;
    }

    private async Task GenerateUserDataAsync()
    {
        Dictionary<string, decimal?> data = new Dictionary<string, decimal?>();
        List<AchievementGraphResponseDto> partOfSpeechData = await AchivementClientService.GetAchievementGraphByPartOfSpeechAsync();
        foreach (AchievementGraphResponseDto node in partOfSpeechData)
        {
            data.Add(node.Label, node.CorrectAnswerRate);
        }
        _config = RadarChartClientService.Create(data, "Correct answer rate by part of speech", "Correct answer rate");

        Dictionary<string, decimal?> data1 = new Dictionary<string, decimal?>();
        List<AchievementGraphResponseDto> weekData = await AchivementClientService.GetAchievementGraphByWeekAsync();
        foreach (AchievementGraphResponseDto node in weekData)
        {
            data1.Add(node.Label, node.CorrectAnswerRate);
        }
        _config1 = LineChartClientService.Create(data1, "Correct answer rate by last week", "Correct answer rate");
    }

}