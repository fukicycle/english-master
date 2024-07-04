using Microsoft.AspNetCore.Components;

namespace EnglishMaster.Client.Pages.Study
{
    public partial class Study
    {
        [Parameter]
        [SupplyParameterFromQuery(Name = "level")]
        public long LevelId { get; set; } = 0;

        [Parameter]
        [SupplyParameterFromQuery(Name = "part-of-speech")]
        public long PartOfSpeechId { get; set; } = 0;

        [Parameter]
        [SupplyParameterFromQuery(Name = "auto-start")]
        public bool AutoStart { get; set; } = false;

        [Parameter]
        [SupplyParameterFromQuery(Name = "mode")]
        public long Mode { get; set; }

        protected override async Task OnInitializedAsync()
        {
            UserSettings userSettings = await SettingService.LoadAsync();
            Mode = userSettings.Mode;
        }

        protected override void OnParametersSet()
        {
            base.OnParametersSet();
            StateHasChanged();
        }
    }
}
