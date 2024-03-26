using EnglishMaster.Client.Forms;
using EnglishMaster.Shared.Dto.Request;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Authorization;
using Microsoft.AspNetCore.Components.WebAssembly.Authentication;
using Newtonsoft.Json;

namespace EnglishMaster.Client.Pages
{
    public partial class Register : PageBase
    {
        public RegisterAccountForm Form { get; set; } = new RegisterAccountForm();


        private void CancelButtonOnClick()
        {
            NavigationManager.NavigateToLogout("authentication/logout", "");
        }

        protected override async Task OnInitializedAsync()
        {
            try
            {
                StateContainer.IsLoading = true;
                AuthenticationState context = await AuthenticationStateProvider.GetAuthenticationStateAsync();
                var firstName = context.User.Claims.FirstOrDefault(a => a.Type == "given_name")?.Value;
                var lastName = context.User.Claims.FirstOrDefault(a => a.Type == "family_name")?.Value;
                if (firstName == null || lastName == null)
                {
                    throw new Exception("Can not get first name or last name. Please re-login your google account.");
                }
                Form.FirstName = firstName;
                Form.LastName = lastName;
                StateHasChanged();
            }
            catch (Exception ex)
            {
                StateContainer.Message = ex.Message;
            }
            finally
            {
                StateContainer.IsLoading = false;
            }
        }

        private async Task RegisterAccount(AuthenticationState context)
        {
            try
            {
                StateContainer.IsLoading = true;
                var email = context.User.Claims.FirstOrDefault(a => a.Type == "email")?.Value;
                var sub = context.User.Claims.FirstOrDefault(a => a.Type == "sub")?.Value;
                if (email == null || sub == null)
                {
                    throw new Exception("Can not get email or aud information. Please re-login your google account.");
                }
                UserReqestDto userReqestDto = new UserReqestDto(email, sub, Form.FirstName, Form.LastName, Form.Nickname);
                HttpResponseResult httpResponseResult = await HttpClientService.SendAsync(HttpMethod.Post, "api/v1/users", JsonConvert.SerializeObject(userReqestDto));
                if (httpResponseResult.StatusCode != System.Net.HttpStatusCode.Created)
                {
                    throw new Exception("Sorry, registration failed. Please try again later.");
                }
                NavigationManager.NavigateTo("", true);
            }
            catch (Exception ex)
            {
                StateContainer.Message = ex.Message;
            }
            finally
            {
                StateContainer.IsLoading = false;
            }

        }
    }
}