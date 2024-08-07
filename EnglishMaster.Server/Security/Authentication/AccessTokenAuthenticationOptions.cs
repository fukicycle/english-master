﻿using EnglishMaster.Shared;
using Microsoft.AspNetCore.Authentication;

namespace EnglishMaster.Server.Security.Authentication
{
    public sealed class AccessTokenAuthenticationOptions : AuthenticationSchemeOptions
    {
        public const string DefaultScheme = "AcessTokenScheme";
        public string TokenHeaderName { get; } = HttpHeaders.ACCESS_TOKEN_HEADER;
    }
}
