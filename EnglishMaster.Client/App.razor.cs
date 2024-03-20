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
                ApplicationSettings.IsMute = settings.IsMute;
                ApplicationSettings.NumberOfQuestion = settings.NumberOfQuestion;
            }
        }
    }
}
