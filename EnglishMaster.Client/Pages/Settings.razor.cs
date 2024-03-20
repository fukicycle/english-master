using EnglishMaster.Shared;

namespace EnglishMaster.Client.Pages
{
    public partial class Settings : PageBase
    {
        private bool _isMute = ApplicationSettings.IsMute;
        private int _numberOfQuestion = ApplicationSettings.NumberOfQuestion;

        private async Task SaveButtonOnClick()
        {
            try
            {
                StateContainer.IsLoading = true;
                ApplicationSettings.IsMute = _isMute;
                ApplicationSettings.NumberOfQuestion = _numberOfQuestion;
                EnglishMaster.Shared.Settings settings = new EnglishMaster.Shared.Settings
                {
                    IsMute = ApplicationSettings.IsMute,
                    NumberOfQuestion = ApplicationSettings.NumberOfQuestion
                };
                await LocalStorageService.SetItemAsync(ApplicationSettings.APPLICATION_ID, settings);
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
    }
}
