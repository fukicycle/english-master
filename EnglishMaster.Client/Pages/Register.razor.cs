using EnglishMaster.Client.Forms;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Authorization;
using Microsoft.AspNetCore.Components.WebAssembly.Authentication;
using Fukicycle.Tool.AppBase.Components.Dialog;
using EnglishMaster.Client.Authentication;

namespace EnglishMaster.Client.Pages
{
    public partial class Register
    {
        public UserRegisterForm Form { get; set; } = new UserRegisterForm();

        [SupplyParameterFromQuery]
        [Parameter]
        public string? Picture { get; set; }

        [SupplyParameterFromQuery]
        [Parameter]
        public string? GivenName { get; set; }

        [SupplyParameterFromQuery]
        [Parameter]
        public string? FamilyName { get; set; }

        [SupplyParameterFromQuery]
        [Parameter]
        public string Name { get; set; } = null!;

        [SupplyParameterFromQuery]
        [Parameter]
        public string? Id { get; set; }

        [SupplyParameterFromQuery]
        [Parameter]
        public string? Email { get; set; }

        protected override void OnInitialized()
        {
            Form.FirstName = GivenName ?? string.Empty;
            Form.LastName = FamilyName ?? string.Empty;
            StateHasChanged();
        }

        private async Task RegisterAccount(AuthenticationState context)
        {
            StateContainer.IsLoading = true;
            if (Id == null || Email == null)
            {
                StateContainer.DialogContent = new DialogContent("Can't get email information. Please re-login your google account.", DialogType.Error);
            }
            else
            {
                bool isSuccess = await UserRegisterService.RegisterAsync(Email, Id, Form.FirstName, Form.LastName, Form.Nickname, Picture);
                if (isSuccess)
                {
                    await AuthenticationStateProvider.UserRegisteredAsync(Email, Id);
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