using Blazored.LocalStorage;
using EnglishMaster.Shared;
using EnglishMaster.Shared.Dto.Request;
using EnglishMaster.Shared.Dto.Response;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Authorization;
using Microsoft.AspNetCore.Components.WebAssembly.Authentication;
using System.Net.Http.Json;
using System.Security.Claims;

namespace EnglishMaster.Client.Authentication
{
    public sealed class CustomAuthenticationStateProvider : AuthenticationStateProvider
    {
        private readonly ILocalStorageService _localStorageService;
        private readonly NavigationManager _navigationManager;
        private readonly HttpClient _httpClientWithGoogle;
        private readonly HttpClient _httpClient;

        public CustomAuthenticationStateProvider(
                ILocalStorageService localStorageService,
                IHttpClientFactory httpClientFactory,
                NavigationManager navigationManager,
                HttpClient httpClient)
        {
            _localStorageService = localStorageService;
            _httpClientWithGoogle = httpClientFactory.CreateClient();
            _httpClient = httpClient;
            _navigationManager = navigationManager;
        }

        public async Task SignInWithGoogleAsync(string accessToken)
        {
            GoogleUser? googleUser = await _httpClientWithGoogle.GetFromJsonAsync<GoogleUser>($"https://www.googleapis.com/userinfo/v2/me?access_token={accessToken}");
            if (googleUser != null)
            {
                //Api authentication
                HttpResponseMessage responseMesage =
                    await _httpClient.PostAsJsonAsync(
                                        ApiEndPoint.LOGIN,
                                        new LoginRequestDto(googleUser.Email, googleUser.Id));
                if (responseMesage.StatusCode == System.Net.HttpStatusCode.Redirect)
                {
                    string redirectPath = await responseMesage.Content.ReadAsStringAsync();
                    _navigationManager.NavigateToLogin(redirectPath);
                }
                else
                {
                    LoginResponseDto? loginResponseDto =
                        await responseMesage.Content.ReadFromJsonAsync<LoginResponseDto>();
                    await _localStorageService.SetItemAsync(
                        LocalStorageKeyConst.ACCESS_TOKEN_KEY,
                        loginResponseDto?.Token);
                }
            }
            NotifyAuthenticationStateChanged(GetAuthenticationStateAsync());
        }


        public override async Task<AuthenticationState> GetAuthenticationStateAsync()
        {
            if (_httpClient.DefaultRequestHeaders.Any(a => a.Key == HttpHeaders.ACCESS_TOKEN_HEADER))
            {
                _httpClient.DefaultRequestHeaders.Remove(HttpHeaders.ACCESS_TOKEN_HEADER);
            }
            if (await _localStorageService.ContainKeyAsync(LocalStorageKeyConst.ACCESS_TOKEN_KEY))
            {
                string? accessTokenKey =
                    await _localStorageService.GetItemAsync<string>(LocalStorageKeyConst.ACCESS_TOKEN_KEY);
                _httpClient.DefaultRequestHeaders.Add(HttpHeaders.ACCESS_TOKEN_HEADER, accessTokenKey);
                IEnumerable<Claim> generalRoleClaims = new List<Claim>
                {
                    new Claim(ClaimTypes.Role,AccessRole.General)
                };
                return new AuthenticationState(
                    new ClaimsPrincipal(
                        new ClaimsIdentity(generalRoleClaims, nameof(CustomAuthenticationStateProvider))));
            }
            IEnumerable<Claim> anonymouseRoleClaims = new List<Claim>
            {
                new Claim(ClaimTypes.Role, AccessRole.Anonymouse)
            };
            return new AuthenticationState(
                new ClaimsPrincipal(
                    new ClaimsIdentity(anonymouseRoleClaims, nameof(CustomAuthenticationStateProvider))));
        }

        public async Task<bool> IsAuthenticatedAsync()
        {
            AuthenticationState state = await GetAuthenticationStateAsync();
            if (state.User.Identity == null)
            {
                return false;
            }
            return state.User.Identity.IsAuthenticated;
        }
    }
}
