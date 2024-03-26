using EnglishMaster.Shared.Dto.Response;
using Microsoft.AspNetCore.Components.WebAssembly.Authentication;
using Newtonsoft.Json;

namespace EnglishMaster.Client.Pages
{
    public partial class Home : PageBase
    {
        private UserResponseDto? _loginUserInfo = null;
        protected override async Task OnInitializedAsync()
        {
            try
            {
                StateContainer.IsLoading = true;
                await GetUserInformation();
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

        private async Task GetUserInformation()
        {
            HttpResponseResult httpResponseResult = await HttpClientService.SendAsync(HttpMethod.Get, "api/v1/users");
            if (httpResponseResult.StatusCode != System.Net.HttpStatusCode.OK)
            {
                throw new Exception(httpResponseResult.Message);
            }
            UserResponseDto? userResponseDto = JsonConvert.DeserializeObject<UserResponseDto>(httpResponseResult.Json);
            if (userResponseDto == null)
            {
                throw new Exception($"Can not desirialize object for: {typeof(UserResponseDto)}");
            }
            _loginUserInfo = userResponseDto;
        }
        private void LoginButtonOnClick()
        {
            NavigationManager.NavigateToLogin($"authentication/login?returnUrl={Uri.EscapeDataString(NavigationManager.Uri)}");
        }
    }
}
