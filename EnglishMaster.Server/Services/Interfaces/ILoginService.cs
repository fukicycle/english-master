﻿using System.Security.Claims;
using EnglishMaster.Shared.Dto.Response;

namespace EnglishMaster.Server.Services.Interfaces
{
    public interface ILoginService
    {
        LoginResponseDto Login(string email, string password);

        protected string GenerateJWTToken(string email);
        string GetValueFromClaims(IEnumerable<Claim> claims, string claimType);
    }
}