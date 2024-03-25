using EnglishMaster.Client.Forms;
using EnglishMaster.Client.Services;
using Microsoft.AspNetCore.Components.Authorization;
using Microsoft.AspNetCore.Components.WebAssembly.Authentication;
using Newtonsoft.Json;

namespace EnglishMaster.Client.Pages;

public partial class Register : PageBase
{
    public RegisterAccountForm Form { get; set; } = new RegisterAccountForm();
    private void CancelButtonOnClick()
    {
        NavigationManager.NavigateToLogout("authentication/logout", "");
    }

    private async Task RegisterAccount(AuthenticationState context)
    {
        try
        {
            StateContainer.IsLoading = true;
            var email = context.User.Claims.FirstOrDefault(a => a.Type == "email")?.Value;
            var aud = context.User.Claims.FirstOrDefault(a => a.Type == "aud")?.Value;
            if (email == null || aud == null)
            {
                throw new Exception("Can not get email or aud information. Please re-login your google account.");
            }
            Form.Email = email;
            Form.Password = aud;
            HttpResponseResult httpResponseResult = await HttpClientService.SendAsync(HttpMethod.Post, "api/v1/users", JsonConvert.SerializeObject(Form));
            if (httpResponseResult.StatusCode != System.Net.HttpStatusCode.Created)
            {
                throw new Exception(httpResponseResult.Message);
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
