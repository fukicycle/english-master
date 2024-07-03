using EnglishMaster.Shared;

namespace EnglishMaster.Client.Pages
{
    public partial class Settings
    {
        private bool _isMute = false;
        private int _numberOfQuestion = 10;
        private long _mode = StudyMode.Choice;

        protected override async Task OnInitializedAsync()
        {
            StateContainer.IsLoading = true;
            UserSettings userSettings = await SettingService.LoadAsync();
            _isMute = userSettings.IsMute;
            _numberOfQuestion = userSettings.NumberOfQuestion;
            _mode = userSettings.Mode;
            StateContainer.IsLoading = false;
        }

        private async Task SaveButtonOnClick()
        {
            UserSettings userSettings = new UserSettings(_isMute, _numberOfQuestion, _mode);
            if (userSettings.NumberOfQuestion < 10)
            {
                StateContainer.DialogContent =
                    new Fukicycle.Tool.AppBase.Components.Dialog.DialogContent(
                            "The number of questions must be greater than 10.",
                            Fukicycle.Tool.AppBase.Components.Dialog.DialogType.Info);
            }
            else
            {
                await SettingService.SaveAsync(userSettings);
            }
        }
    }
}
