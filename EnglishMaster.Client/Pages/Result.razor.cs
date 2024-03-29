using Microsoft.AspNetCore.Components;

namespace EnglishMaster.Client.Pages
{
    public partial class Result : PageBase
    {
        [Parameter]
        [SupplyParameterFromQuery]
        public int Count { get; set; }

        private void CloseButtonOnClick()
        {
            NavigationManager.NavigateTo("");
        }
    }
}
