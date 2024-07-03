using EnglishMaster.Shared;
using Toolbelt.Blazor.SpeechSynthesis;

namespace EnglishMaster.Client.Pages
{
    public partial class Settings
    {
        private string? _voiceIdentity = null;
        private int _numberOfQuestion = 10;
        private long _mode = StudyMode.Choice;
        private string _buttonContent = "Save changes";

        private IEnumerable<SpeechSynthesisVoice> _voices = Enumerable.Empty<SpeechSynthesisVoice>();

        protected override async Task OnInitializedAsync()
        {
            StateContainer.IsLoading = true;
            _voices = await SpeakService.GetVoicesAsync();
            UserSettings userSettings = await SettingService.LoadAsync();
            _voiceIdentity = userSettings.VoiceIdentity;
            _numberOfQuestion = userSettings.NumberOfQuestion;
            _mode = userSettings.Mode;
            StateContainer.IsLoading = false;
        }

        private async Task SaveButtonOnClick()
        {
            UserSettings userSettings = new UserSettings(_voiceIdentity, _numberOfQuestion, _mode);
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
                _buttonContent = "Saved!";
                StateHasChanged();
                await Task.Delay(1000);
                _buttonContent = "Save changes";
                StateHasChanged();
            }
        }
    }
}
