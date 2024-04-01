using Microsoft.AspNetCore.Components;

namespace EnglishMaster.Client.Components
{
    public abstract class ButtonBase
    {
        [Parameter]
        public string Content { get; set; } = null!;

        [Parameter]
        public EventCallback OnClick { get; set; }
    }
}
