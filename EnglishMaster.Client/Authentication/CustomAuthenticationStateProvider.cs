using Blazored.LocalStorage;
using EnglishMaster.Shared;
using EnglishMaster.Shared.Dto.Request;
using EnglishMaster.Shared.Dto.Response;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Authorization;
using Microsoft.AspNetCore.Components.WebAssembly.Authentication;
using Microsoft.AspNetCore.WebUtilities;
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
        private AuthenticationState _currentAuthenticationState;
        public UserResponseDto? LoginUser { get; private set; }

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
            _currentAuthenticationState = Create(AccessRole.Anonymouse);
        }

        public async Task<bool> SignInWithGoogleAsync(string accessToken)
        {
            _logger.LogInformation(accessToken);
            GoogleUser? googleUser = await _httpClientWithGoogle.GetFromJsonAsync<GoogleUser>($"https://www.googleapis.com/userinfo/v2/me?access_token={accessToken}");
            if (googleUser != null)
            {
                _logger.LogInformation("Api authentication.");
                _logger.LogInformation($"Google User {googleUser.Id},{googleUser.Email}");
                //Api authentication
                HttpResponseMessage responseMesage =
                    await _httpClient.PostAsJsonAsync(
                                        ApiEndPoint.LOGIN,
                                        new LoginRequestDto(googleUser.Email, googleUser.Id));
                if (responseMesage.StatusCode == System.Net.HttpStatusCode.TemporaryRedirect)
                {
                    string redirectPath = await responseMesage.Content.ReadAsStringAsync();
                    _logger.LogInformation("Redirect to register page.");
                    List<KeyValuePair<string, string?>> queries = new List<KeyValuePair<string, string?>>();
                    queries.Add(new KeyValuePair<string, string?>(nameof(GoogleUser.GivenName), googleUser.GivenName));
                    queries.Add(new KeyValuePair<string, string?>(nameof(GoogleUser.FamilyName), googleUser.FamilyName));
                    queries.Add(new KeyValuePair<string, string?>(nameof(GoogleUser.Picture), googleUser.Picture));
                    queries.Add(new KeyValuePair<string, string?>(nameof(GoogleUser.Name), googleUser.Name));
                    queries.Add(new KeyValuePair<string, string?>(nameof(GoogleUser.Email), googleUser.Email));
                    queries.Add(new KeyValuePair<string, string?>(nameof(GoogleUser.Id), googleUser.Id));
                    string uri = QueryHelpers.AddQueryString(redirectPath, queries);
                    _currentAuthenticationState = Create(AccessRole.Unregistered);
                    NotifyAuthenticationStateChanged(Task.FromResult(_currentAuthenticationState));
                    _logger.LogInformation($"Access as unregistered. {_currentAuthenticationState.User.IsInRole(nameof(AccessRole.Unregistered))}");
                    _navigationManager.NavigateTo(uri);
                    return true;
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
            return false;
        }

        public async Task UserRegisteredAsync(string email, string password)
        {
            _logger.LogInformation("Api authentication.");
            //Api authentication
            HttpResponseMessage responseMesage =
                await _httpClient.PostAsJsonAsync(
                                    ApiEndPoint.LOGIN,
                                    new LoginRequestDto(email, password));
            if (responseMesage.StatusCode == System.Net.HttpStatusCode.TemporaryRedirect)
            {
                throw new Exception("Unexpected error has occured. Please contact administrator.");
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
            _currentAuthenticationState = Create(AccessRole.General);
            await SetAuthenticationStateAsync();
        }

        private async Task SetAuthenticationStateAsync()
        {
            if (_httpClient.DefaultRequestHeaders.Any(a => a.Key == HttpHeaders.ACCESS_TOKEN_HEADER))
            {
                _logger.LogInformation("Remove access token header.");
                _httpClient.DefaultRequestHeaders.Remove(HttpHeaders.ACCESS_TOKEN_HEADER);
            }
            if (_currentAuthenticationState.User.IsInRole(nameof(AccessRole.Unregistered)))
            {
                _logger.LogInformation("Unregisterd user.");
                return;
            }
            if (await _localStorageService.ContainKeyAsync(LocalStorageKeyConst.ACCESS_TOKEN_KEY))
            {
                StoredAccessToken? accessToken =
                    await _localStorageService.GetItemAsync<StoredAccessToken>(LocalStorageKeyConst.ACCESS_TOKEN_KEY);
                _logger.LogInformation("Verify stored access token.");
                if (accessToken!.Expires.Date <= DateTime.UtcNow.Date)
                {
                    _logger.LogInformation("Expired token.");
                    _currentAuthenticationState = Create(AccessRole.Anonymouse);
                }
                else
                {
                    _httpClient.DefaultRequestHeaders.Add(HttpHeaders.ACCESS_TOKEN_HEADER, accessToken!.Token);
                    _currentAuthenticationState = Create(AccessRole.General);
                    await SetLoginUserInfoAsync();
                }
            }
            else
            {
                _logger.LogInformation("Unauthorized. Access as anonymouse role.");
                _currentAuthenticationState = Create(AccessRole.Anonymouse);
            }
            NotifyAuthenticationStateChanged(Task.FromResult(_currentAuthenticationState));
        }


        public override async Task<AuthenticationState> GetAuthenticationStateAsync()
        {
            await SetAuthenticationStateAsync();
            return _currentAuthenticationState;
        }

        private async Task SetLoginUserInfoAsync()
        {
            LoginUser = await _httpClient.GetFromJsonAsync<UserResponseDto>(ApiEndPoint.USER);

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
                case AccessRole.Unregistered:
                    IEnumerable<Claim> unregisteredRoleClaims = new List<Claim>
                    {
                        new Claim(ClaimTypes.Role,nameof(AccessRole.Unregistered))
                    };
                    return new AuthenticationState(
                        new ClaimsPrincipal(
                            new ClaimsIdentity(unregisteredRoleClaims, nameof(CustomAuthenticationStateProvider))));
                default:
                    return new AuthenticationState(
                        new ClaimsPrincipal(
                            new ClaimsIdentity(Array.Empty<Claim>(), string.Empty)));
            }
        }
    }
}
