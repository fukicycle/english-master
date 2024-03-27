using EnglishMaster.Shared;
using EnglishMaster.Shared.Dto.Response;
using Microsoft.AspNetCore.Components.Authorization;
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
                AuthenticationState authenticationState = await AuthenticationStateProvider.GetAuthenticationStateAsync();
                if (authenticationState.User.Identity?.IsAuthenticated == true)
                {
                    await SetCurrentLoginUserInformation();
                }
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

        private async Task SetCurrentLoginUserInformation()
        {
            HttpResponseResult httpResponseResult = await HttpClientService.SendWithJWTTokenAsync(HttpMethod.Get, ApiEndPoint.USER);
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
