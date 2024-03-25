using EnglishMaster.Client.Forms;
using Microsoft.AspNetCore.Components.WebAssembly.Authentication;

namespace EnglishMaster.Client.Pages;

public partial class Register : PageBase
{
    public RegisterAccountForm Form { get; set; } = new RegisterAccountForm();
    private void CancelButtonOnClick()
    {
        NavigationManager.NavigateToLogout("authentication/logout", "");
    }

    private async Task RegisterAccount()
    {
        await Task.CompletedTask;
    }
}
