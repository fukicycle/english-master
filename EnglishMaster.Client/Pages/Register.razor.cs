using EnglishMaster.Client.Forms;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Authorization;
using Microsoft.AspNetCore.Components.WebAssembly.Authentication;
using Fukicycle.Tool.AppBase.Components.Dialog;

namespace EnglishMaster.Client.Pages
{
    public partial class Register
    {
        public UserRegisterForm Form { get; set; } = new UserRegisterForm();

        protected override async Task OnInitializedAsync()
        {
            StateContainer.IsLoading = true;
            string? firstName = await ExecuteAsync(AuthenticationService.GetFirstNameAsync);
            string? lastName = await ExecuteAsync(AuthenticationService.GetLastNameAsync);
            Form.FirstName = firstName ?? string.Empty;
            Form.LastName = lastName ?? string.Empty;
            StateHasChanged();
            StateContainer.IsLoading = false;
        }

        private async Task RegisterAccount(AuthenticationState context)
        {
            StateContainer.IsLoading = true;
            string? email = await ExecuteAsync(AuthenticationService.GetEmailAsync);
            string? sub = await ExecuteAsync(AuthenticationService.GetSubAsPasswordAsync);
            if (email == null || sub == null)
            {
                StateContainer.DialogContent = new DialogContent("Can't get email information. Please re-login your google account.", DialogType.Error);
            }
            else
            {
                bool isSuccess = await UserRegisterService.RegisterAsync(email, sub, Form.FirstName, Form.LastName, Form.Nickname);
                if (isSuccess)
                {
                    NavigationManager.NavigateTo("", true);
                }
                else
                {
                    StateContainer.DialogContent = new DialogContent("Sorry, registration failed. Please try again later.", DialogType.Error);
                }
            }
            StateContainer.IsLoading = false;
        }

        private void CancelButtonOnClick()
        {
            NavigationManager.NavigateToLogout("authentication/logout", "");
        }
    }
}