using Microsoft.AspNetCore.Components.WebAssembly.Authentication;

namespace EnglishMaster.Client.Pages
{
    public partial class Home : PageBase
    {
        private void LoginButtonOnClick()
        {
            NavigationManager.NavigateToLogin($"authentication/login?returnUrl={Uri.EscapeDataString(NavigationManager.Uri)}");
        }
    }
}
