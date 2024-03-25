using EnglishMaster.Shared;

namespace EnglishMaster.Client
{
    public partial class App
    {
        protected override async Task OnInitializedAsync()
        {
            Settings? settings = await LocalStorageService.GetItemAsync<Settings>(ApplicationSettings.APPLICATION_ID);
            if (settings != null)
            {
                SettingService.IsMute = settings.IsMute;
                SettingService.NumberOfQuestion = settings.NumberOfQuestion;
            }
        }
    }
}
