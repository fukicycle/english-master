using EnglishMaster.Client.Services;
using EnglishMaster.Shared;

namespace EnglishMaster.Client.Pages
{
    public partial class Settings : PageBase
    {
        private bool _isMute = false;
        private int _numberOfQuestion = 10;
        private async Task SaveButtonOnClick()
        {
            try
            {
                StateContainer.IsLoading = true;
                SettingService.IsMute = _isMute;
                SettingService.NumberOfQuestion = _numberOfQuestion;
                EnglishMaster.Shared.Settings settings = new EnglishMaster.Shared.Settings
                {
                    IsMute = SettingService.IsMute,
                    NumberOfQuestion = SettingService.NumberOfQuestion
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
