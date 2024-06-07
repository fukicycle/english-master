using EnglishMaster.Server.Services.Interfaces;
using EnglishMaster.Shared.Dto.Response;
using EnglishMaster.Shared.Models;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;
using System.Text;

namespace EnglishMaster.Server.Services
{
    public sealed class AccessTokenLoginService : ILoginService
    {
        private readonly DB _db;
        public AccessTokenLoginService(DB db)
        {
            _db = db;
        }

        public string GetValueFromClaims(IEnumerable<Claim> claims, string claimType)
        {
            throw new NotImplementedException();
        }

        public LoginResponseDto Login(string email, string password)
        {
            User? user = _db.Users
                           .Include(a => a.AccessTokens)
                           .FirstOrDefault(a => a.Username == email && a.Password == password);
            if (user == null)
            {
                return new LoginResponseDto(string.Empty);
            }
            AccessToken validateToken = new AccessToken();
            AccessToken? currentAccessToken = user.AccessTokens.OrderByDescending(a => a.Expires).FirstOrDefault();
            if (currentAccessToken == null || currentAccessToken.Expires < DateTime.UtcNow)
            {
                AccessToken newToken = new AccessToken
                {
                    Expires = DateTime.UtcNow.AddMonths(1),
                    UserId = user.Id,
                    Token = GenerateJWTToken(email)
                };
                _db.AccessTokens.Add(newToken);
                _db.SaveChanges();
                validateToken = newToken;
            }
            else
            {
                validateToken = currentAccessToken;
            }
            return new LoginResponseDto(validateToken.Token);
        }

        public string GenerateJWTToken(string email)
        {
            int keyLength = 64;
            StringBuilder sb = new StringBuilder(keyLength);
            string lowerCase = "abcdefghijklmnopqrstuvwxyz";
            string upperCase = "abcdefghijklmnopqrstuvwxyz".ToUpper();
            for (int i = 0; i < keyLength; i++)
            {
                if (Random.Shared.Next() % 2 == 1)
                {
                    //Upper case
                    sb.Append(upperCase[Random.Shared.Next(0, upperCase.Length - 1)]);
                }
                else
                {
                    //Lower case
                    sb.Append(lowerCase[Random.Shared.Next(0, lowerCase.Length - 1)]);
                }
            }
            return sb.ToString();
        }
    }
}
