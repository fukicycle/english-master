using EnglishMaster.Shared;

namespace EnglishMaster.Client.Pages
{
    public partial class Settings
    {
        private bool _isMute = false;
        private int _numberOfQuestion = 10;

        protected override async Task OnInitializedAsync()
        {
            //try
            //{
            //    StateContainer.IsLoading = true;
            //    EnglishMaster.Shared.Settings? settings = await LocalStorageService.GetItemAsync<EnglishMaster.Shared.Settings>(ApplicationSettings.APPLICATION_ID);
            //    if (settings != null)
            //    {
            //        _isMute = settings.IsMute;
            //        _numberOfQuestion = settings.NumberOfQuestion;
            //    }
            //}
            //catch (Exception ex)
            //{
            //    StateContainer.Message = ex.Message;
            //}
            //finally
            //{
            //    StateContainer.IsLoading = false;
            //}
        }
        private async Task SaveButtonOnClick()
        {
            //try
            //{
            //    StateContainer.IsLoading = true;
            //    SettingService.IsMute = _isMute;
            //    SettingService.NumberOfQuestion = _numberOfQuestion;
            //    EnglishMaster.Shared.Settings settings = new EnglishMaster.Shared.Settings
            //    {
            //        IsMute = SettingService.IsMute,
            //        NumberOfQuestion = SettingService.NumberOfQuestion
            //    };
            //    await LocalStorageService.SetItemAsync(ApplicationSettings.APPLICATION_ID, settings);
            //}
            //catch (Exception ex)
            //{
            //    StateContainer.Message = ex.Message;
            //}
            //finally
            //{
            //    StateContainer.IsLoading = false;
            //}
        }
    }
}
