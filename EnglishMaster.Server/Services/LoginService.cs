using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using EnglishMaster.Server.Services.Interfaces;
using EnglishMaster.Shared;
using EnglishMaster.Shared.Dto.Response;
using EnglishMaster.Shared.Models;
using Microsoft.IdentityModel.Tokens;

namespace EnglishMaster.Server.Services
{
    public sealed class LoginService : ILoginService
    {
        private readonly DB _db;
        private readonly ILogger<LoginService> _logger;

        public LoginService(DB db, ILogger<LoginService> logger)
        {
            _db = db;
            _logger = logger;
        }

        public string GenerateJWTToken(string email)
        {
            SymmetricSecurityKey key = new SymmetricSecurityKey(ApplicationSettings.JWT_KEY);
            SigningCredentials creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);
            IEnumerable<Claim> claims = new List<Claim> { new Claim("EMAIL", email) };
            JwtSecurityToken token = new JwtSecurityToken(
                issuer: "sato-home.mydns.jp",
                audience: "fukicycle.github.io",
                notBefore: DateTime.Now,
                expires: DateTime.Now.AddDays(30),
                signingCredentials: creds,
                claims: claims);
            return new JwtSecurityTokenHandler().WriteToken(token);
        }

        public LoginResponseDto Login(string email, string password)
        {
            User? user = _db.Users.FirstOrDefault(a => a.Username == email && a.Password == password);
            if (user == null)
            {
                return new LoginResponseDto(string.Empty);
            }
            string token = GenerateJWTToken(email);
            return new LoginResponseDto(token);
        }

        public string GetValueFromClaims(IEnumerable<Claim> claims, string claimType)
        {
            string? value = claims.FirstOrDefault(a => a.Type.ToLower() == claimType.ToLower())?.Value;
            if (value == null)
            {
                throw new Exception($"No such claim type:{claimType}, value={value}");
            }
            return value;
        }
    }
}