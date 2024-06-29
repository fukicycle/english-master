using EnglishMaster.Shared.Models;
using Microsoft.AspNetCore.Authentication;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using System.Security.Claims;
using System.Text.Encodings.Web;

namespace EnglishMaster.Server.Security.Authentication
{
    public sealed class AccessTokenAuthenticationHandler : AuthenticationHandler<AccessTokenAuthenticationOptions>
    {
        private readonly DB _db;
        private readonly ILogger<AccessTokenAuthenticationHandler> _logger;
        public AccessTokenAuthenticationHandler(
                IOptionsMonitor<AccessTokenAuthenticationOptions> options,
                ILoggerFactory logger,
                UrlEncoder encoder,
                DB db) : base(options, logger, encoder)
        {
            _db = db;
            _logger = logger.CreateLogger<AccessTokenAuthenticationHandler>();
        }

        protected override async Task<AuthenticateResult> HandleAuthenticateAsync()
        {
            if (!Request.Headers.ContainsKey(Options.TokenHeaderName))
            {
                return AuthenticateResult.Fail($"Missing header: {Options.TokenHeaderName}");
            }

            string? accessToken = Request.Headers[Options.TokenHeaderName];
            AccessToken? accessTokenObj =
                await _db.AccessTokens.FirstOrDefaultAsync(a => a.Token == accessToken);
            if (accessTokenObj == null)
            {
                _logger.LogInformation("Invalid token.");
                return AuthenticateResult.Fail("Invalid token.");
            }
            if (accessTokenObj.Expires < DateTime.UtcNow)
            {
                _logger.LogInformation($"Expired token. Expires:{accessTokenObj.Expires:yyyy-MM-dd HH:mm:ss}, Current: {DateTime.UtcNow:yyyy-MM-dd HH:mm:ss}");
                return AuthenticateResult.Fail("Expired token.");
            }

            IEnumerable<Claim> claims = new List<Claim>()
            {
                new Claim(ClaimTypes.NameIdentifier,accessTokenObj.UserId.ToString())
            };

            ClaimsIdentity claimsIdentity = new ClaimsIdentity(claims, Scheme.Name);
            ClaimsPrincipal claimsPrincipal = new ClaimsPrincipal(claimsIdentity);

            return AuthenticateResult.Success(
                new AuthenticationTicket(claimsPrincipal, Scheme.Name));
        }
    }
}
