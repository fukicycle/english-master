﻿using EnglishMaster.Shared.Models;
using Microsoft.AspNetCore.Authentication;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using System.Security.Claims;
using System.Text.Encodings.Web;

namespace EnglishMaster.Server.Authentication
{
    public sealed class AccessTokenAuthenticationHandler : AuthenticationHandler<AccessTokenAuthenticationOptions>
    {
        private readonly DB _db;
        public AccessTokenAuthenticationHandler(
                IOptionsMonitor<AccessTokenAuthenticationOptions> options,
                ILoggerFactory logger,
                UrlEncoder encoder,
                DB db) : base(options, logger, encoder)
        {
            _db = db;
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
                return AuthenticateResult.Fail("Invalid token.");
            }
            if (accessTokenObj.Expires < DateTime.Now)
            {
                return AuthenticateResult.Fail("Expired token.");
            }

            var claims = new List<Claim>()
            {
                new Claim(ClaimTypes.NameIdentifier,accessTokenObj.UserId.ToString())
            };

            var claimsIdentity = new ClaimsIdentity(claims, Scheme.Name);
            var claimsPrincipal = new ClaimsPrincipal(claimsIdentity);

            return AuthenticateResult.Success(
                new AuthenticationTicket(claimsPrincipal, Scheme.Name));
        }
    }
}
