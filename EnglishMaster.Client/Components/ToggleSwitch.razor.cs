using Microsoft.AspNetCore.Components;

namespace EnglishMaster.Client.Components
{
    public partial class ToggleSwitch
    {
        [Parameter]
        public bool IsSelected { get; set; }
        [Parameter]
        public EventCallback<bool> IsSelectedChanged { get; set; }

        private async Task SelectionChanged()
        {
            IsSelected = !IsSelected;
            await IsSelectedChanged.InvokeAsync(IsSelected);
        }


        private string _identifier = Guid.NewGuid().ToString();
    }
}