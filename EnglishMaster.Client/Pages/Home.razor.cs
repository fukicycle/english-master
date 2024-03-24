using EnglishMaster.Client.Components;

namespace EnglishMaster.Client.Pages
{
    public partial class Home : PageBase
    {
        private void LoginButtonOnClick()
        {
            NavigationManager.NavigateTo($"authentication/login?returnUrl={Uri.EscapeDataString(NavigationManager.Uri)}");
        }
    }
}
