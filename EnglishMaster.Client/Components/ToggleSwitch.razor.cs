using Microsoft.AspNetCore.Components;

namespace EnglishMaster.Client.Components
{
    public partial class ToggleSwitch
    {
        [Parameter]
        public bool IsSelected { get; set; }

        private string _identifier = Guid.NewGuid().ToString();
    }
}
