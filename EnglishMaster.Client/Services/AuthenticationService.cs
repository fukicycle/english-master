using EnglishMaster.Client.Services.Interfaces;
using EnglishMaster.Shared.Dto.Response;
using EnglishMaster.Shared;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Authorization;
using Newtonsoft.Json;
using EnglishMaster.Client.Entities;

namespace EnglishMaster.Client.Services
{
    public sealed class AuthenticationService : IAuthenticationService
    {
        private readonly ILogger<AuthenticationService> _logger;
        private readonly AuthenticationStateProvider _authenticationStateProvider;
        private readonly IHttpClientService _httpClientService;

        public AuthenticationService(ILogger<AuthenticationService> logger, AuthenticationStateProvider authenticationStateProvider, IHttpClientService httpClientService)
        {
            _logger = logger;
            _authenticationStateProvider = authenticationStateProvider;
            _httpClientService = httpClientService;
        }

        public async Task<bool> IsAuthenticatedAsync()
        {
            _logger.LogInformation("Checking authentication state.");
            AuthenticationState authenticationState = await _authenticationStateProvider.GetAuthenticationStateAsync();
            return authenticationState.User.Identity?.IsAuthenticated == true;
        }

        public async Task<LoginUser?> GetLoginUserAsync()
        {
            HttpResponseResult httpResponseResult = await _httpClientService.SendWithJWTTokenAsync(HttpMethod.Get, ApiEndPoint.USER);
            if (httpResponseResult.StatusCode == System.Net.HttpStatusCode.NotFound)
            {
                _logger.LogInformation("Not registered user. Redirect to register page.");
                return null;
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
            string? icon = await GetIconUrlAsync();
            return new LoginUser(userResponseDto.FirstName, userResponseDto.LastName, icon);
        }

        public async Task<string?> GetIconUrlAsync()
        {
            AuthenticationState authenticationState = await _authenticationStateProvider.GetAuthenticationStateAsync();
            return authenticationState.User.Claims.FirstOrDefault(a => a.Type == "picture")?.Value;
        }

        public async Task<string?> GetFirstNameAsync()
        {
            AuthenticationState authenticationState = await _authenticationStateProvider.GetAuthenticationStateAsync();
            return authenticationState.User.Claims.FirstOrDefault(a => a.Type == "given_name")?.Value;
        }

        public async Task<string?> GetLastNameAsync()
        {
            AuthenticationState authenticationState = await _authenticationStateProvider.GetAuthenticationStateAsync();
            return authenticationState.User.Claims.FirstOrDefault(a => a.Type == "family_name")?.Value;
        }

        public async Task<string?> GetEmailAsync()
        {
            AuthenticationState authenticationState = await _authenticationStateProvider.GetAuthenticationStateAsync();
            return authenticationState.User.Claims.FirstOrDefault(a => a.Type == "email")?.Value;
        }

        public async Task<string?> GetSubAsPasswordAsync()
        {
            AuthenticationState authenticationState = await _authenticationStateProvider.GetAuthenticationStateAsync();
            return authenticationState.User.Claims.FirstOrDefault(a => a.Type == "sub")?.Value;
        }
    }
}
