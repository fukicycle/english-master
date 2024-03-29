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
        private IList<AchievementResponseDto>? _achievements = null;

        protected override async Task OnInitializedAsync()
        {
            try
            {
                StateContainer.IsLoading = true;
                if (await AuthenticationService.IsAuthenticatedAsync())
                {
                    await SetCurrentLoginUserInformation();
                    _achievements = await GetAchievementAsync();
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
            if (httpResponseResult.StatusCode == System.Net.HttpStatusCode.NotFound)
            {
                NavigationManager.NavigateTo("register");
                return;
            }
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

        private async Task<IList<AchievementResponseDto>> GetAchievementAsync()
        {
            HttpResponseResult httpResponseResult = await HttpClientService.SendWithJWTTokenAsync(HttpMethod.Get, ApiEndPoint.ACHIEVEMENT);
            if (httpResponseResult.StatusCode != System.Net.HttpStatusCode.OK)
            {
                throw new Exception(httpResponseResult.Message);
            }
            IList<AchievementResponseDto>? achievementResponseDto = JsonConvert.DeserializeObject<IList<AchievementResponseDto>>(httpResponseResult.Json);
            if (achievementResponseDto == null)
            {
                throw new Exception($"Can not desirialize object for: {typeof(IList<AchievementResponseDto>)}");
            }
            return achievementResponseDto;
        }

        private double GetTotalProgressValue()
        {
            if (_achievements != null)
            {
                if (_achievements.Any())
                {
                    return Math.Round(_achievements.Average(a => a.Progress), 0);
                }
            }
            return 0;
        }

        private void LoginButtonOnClick()
        {
            NavigationManager.NavigateToLogin($"authentication/login?returnUrl={Uri.EscapeDataString(NavigationManager.Uri)}");
        }
    }
}
