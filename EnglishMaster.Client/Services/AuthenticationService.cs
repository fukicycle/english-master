using EnglishMaster.Client.Services.Interfaces;
using Microsoft.AspNetCore.Components.Authorization;

namespace EnglishMaster.Client.Services
{
    public sealed class AuthenticationService : IAuthenticationService
    {
        private readonly ILogger<AuthenticationService> _logger;
        private readonly AuthenticationStateProvider _authenticationStateProvider;
        public AuthenticationService(ILogger<AuthenticationService> logger, AuthenticationStateProvider authenticationStateProvider)
        {
            _logger = logger;
            _authenticationStateProvider = authenticationStateProvider;
        }

        public async Task<bool> IsAuthenticatedAsync()
        {
            _logger.LogInformation("Checking authentication state.");
            AuthenticationState authenticationState = await _authenticationStateProvider.GetAuthenticationStateAsync();
            return authenticationState.User.Identity?.IsAuthenticated == true;
        }
    }
}
