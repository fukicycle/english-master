using Microsoft.AspNetCore.Components.WebAssembly.Authentication;

namespace EnglishMaster.Client.Pages;

public partial class Register : PageBase
{
    private void CancelButtonOnClick()
    {
        NavigationManager.NavigateToLogout("authentication/logout", "");
    }
}
