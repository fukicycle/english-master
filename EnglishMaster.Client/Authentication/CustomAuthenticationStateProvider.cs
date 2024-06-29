using Blazored.LocalStorage;
using EnglishMaster.Shared;
using EnglishMaster.Shared.Dto.Request;
using EnglishMaster.Shared.Dto.Response;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Authorization;
using Microsoft.AspNetCore.Components.WebAssembly.Authentication;
using System.Net.Http.Json;
using System.Net.NetworkInformation;
using System.Security.Claims;

namespace EnglishMaster.Client.Authentication
{
    public sealed class CustomAuthenticationStateProvider : AuthenticationStateProvider
    {
        private readonly ILocalStorageService _localStorageService;
        private readonly NavigationManager _navigationManager;
        private readonly HttpClient _httpClientWithGoogle;
        private readonly HttpClient _httpClient;
        private readonly ILogger<CustomAuthenticationStateProvider> _logger;
        public AuthenticationState CurrentAuthenticationState { get; private set; }

        public CustomAuthenticationStateProvider(
                ILocalStorageService localStorageService,
                IHttpClientFactory httpClientFactory,
                NavigationManager navigationManager,
                HttpClient httpClient,
                ILogger<CustomAuthenticationStateProvider> logger)
        {
            _localStorageService = localStorageService;
            _httpClientWithGoogle = httpClientFactory.CreateClient();
            _httpClient = httpClient;
            _navigationManager = navigationManager;
            _logger = logger;
            CurrentAuthenticationState = Create(AccessRole.Anonymouse);
        }

        public async Task SignInWithGoogleAsync(string accessToken)
        {
            GoogleUser? googleUser = await _httpClientWithGoogle.GetFromJsonAsync<GoogleUser>($"https://www.googleapis.com/userinfo/v2/me?access_token={accessToken}");
            if (googleUser != null)
            {
                _logger.LogInformation("Api authentication.");
                //Api authentication
                HttpResponseMessage responseMesage =
                    await _httpClient.PostAsJsonAsync(
                                        ApiEndPoint.LOGIN,
                                        new LoginRequestDto(googleUser.Email, googleUser.Id));
                if (responseMesage.StatusCode == System.Net.HttpStatusCode.Redirect)
                {
                    string redirectPath = await responseMesage.Content.ReadAsStringAsync();
                    _logger.LogInformation("Redirect to register page.");
                    _navigationManager.NavigateToLogin(redirectPath);
                }
                else
                {
                    LoginResponseDto? loginResponseDto =
                        await responseMesage.Content.ReadFromJsonAsync<LoginResponseDto>();
                    if (loginResponseDto == null)
                    {
                        throw new Exception("Unexpected error has occured. Please contact administrator.");
                    }
                    StoredAccessToken storedAccessToken = new StoredAccessToken(loginResponseDto.Token, loginResponseDto.Expires);
                    await _localStorageService.SetItemAsync(
                        LocalStorageKeyConst.ACCESS_TOKEN_KEY,
                        storedAccessToken);
                }
            }
            await SetAuthenticationStateAsync();
        }

        private async Task SetAuthenticationStateAsync()
        {
            if (_httpClient.DefaultRequestHeaders.Any(a => a.Key == HttpHeaders.ACCESS_TOKEN_HEADER))
            {
                _logger.LogInformation("Remove access token header.");
                _httpClient.DefaultRequestHeaders.Remove(HttpHeaders.ACCESS_TOKEN_HEADER);
            }
            if (await _localStorageService.ContainKeyAsync(LocalStorageKeyConst.ACCESS_TOKEN_KEY))
            {
                StoredAccessToken? accessToken =
                    await _localStorageService.GetItemAsync<StoredAccessToken>(LocalStorageKeyConst.ACCESS_TOKEN_KEY);
                _logger.LogInformation("Verify stored access token.");
                if (accessToken!.Expires.Date <= DateTime.UtcNow.Date)
                {
                    _logger.LogInformation("Expired token.");
                    CurrentAuthenticationState = Create(AccessRole.Anonymouse);
                }
                else
                {
                    _httpClient.DefaultRequestHeaders.Add(HttpHeaders.ACCESS_TOKEN_HEADER, accessToken!.Token);
                    CurrentAuthenticationState = Create(AccessRole.General);
                }
            }
            else
            {
                _logger.LogInformation("Unauthorized. Access as anonymouse role.");
                CurrentAuthenticationState = Create(AccessRole.Anonymouse);
            }
            NotifyAuthenticationStateChanged(Task.FromResult(CurrentAuthenticationState));
        }


        public override async Task<AuthenticationState> GetAuthenticationStateAsync()
        {
            await SetAuthenticationStateAsync();
            return CurrentAuthenticationState;
        }

        private AuthenticationState Create(AccessRole accessRole)
        {
            switch (accessRole)
            {
                case AccessRole.Anonymouse:
                    IEnumerable<Claim> anonymouseRoleClaims = new List<Claim>
                    {
                        new Claim(ClaimTypes.Role, nameof(AccessRole.Anonymouse))
                    };
                    return new AuthenticationState(
                        new ClaimsPrincipal(
                            new ClaimsIdentity(anonymouseRoleClaims, nameof(CustomAuthenticationStateProvider))));
                case AccessRole.General:
                    IEnumerable<Claim> generalRoleClaims = new List<Claim>
                    {
                        new Claim(ClaimTypes.Role,nameof(AccessRole.General))
                    };
                    return new AuthenticationState(
                        new ClaimsPrincipal(
                            new ClaimsIdentity(generalRoleClaims, nameof(CustomAuthenticationStateProvider))));
                default:
                    return new AuthenticationState(
                        new ClaimsPrincipal(
                            new ClaimsIdentity(Array.Empty<Claim>(), string.Empty)));
            }
        }
    }
}
