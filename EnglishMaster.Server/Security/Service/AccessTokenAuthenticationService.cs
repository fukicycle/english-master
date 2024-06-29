using EnglishMaster.Shared.Models;
using Microsoft.AspNetCore.Http.HttpResults;
using System.Text;

namespace EnglishMaster.Server.Security.Service
{
    public sealed class AccessTokenAuthenticationService : IAccessTokenAuthenticationService
    {
        private readonly DB _db;
        public AccessTokenAuthenticationService(DB db)
        {
            _db = db;
        }

        public AccessTokenAuthenticationResult Authenticate(string username, string password)
        {
            if (!_db.Users.Any(a => a.Username == username))
            {
                return new AccessTokenAuthenticationResult(AccessTokenAuthenticationResultCode.UNREGISTERED_USER, null, null);
            }

            User? user = _db.Users.FirstOrDefault(a =>
                            a.Username == username &&
                            a.Password == password);

            if (user == default)
            {
                return new AccessTokenAuthenticationResult(AccessTokenAuthenticationResultCode.INVALID_CREDENTIAL, null, null);
            }
            AccessToken? accessToken = _db.AccessTokens
                                        .OrderByDescending(a => a.Expires)
                                        .FirstOrDefault(a => a.UserId == user.Id &&
                                                             a.Expires >= DateTime.UtcNow);

            if (accessToken == default)
            {
                string newToken = GenerateToken();
#if DEBUG
                DateTime expires = DateTime.UtcNow.AddDays(10);
#else
                DateTime expires = DateTime.UtcNow.AddMonths(1);
#endif
                accessToken = new AccessToken
                {
                    Expires = expires,
                    UserId = user.Id,
                    Token = newToken
                };
                _db.AccessTokens.Add(accessToken);
                _db.SaveChanges();
            }

            return new AccessTokenAuthenticationResult(AccessTokenAuthenticationResultCode.SUCCESS, accessToken.Token, accessToken.Expires);
        }

        private string GenerateToken()
        {
            int tokenLength = 64;
            StringBuilder sb = new StringBuilder(tokenLength);
            string lower = "abcdefghijklmnopqrstuvwxyz";
            string upper = "abcdefghijklmnopqrstuvwxyz".ToUpper();
            Random r = Random.Shared;
            for (int i = 0; i < tokenLength; i++)
            {
                if (r.Next() % 2 == 0)
                {
                    sb.Append(lower[r.Next(0, lower.Length - 1)]);
                }
                else
                {
                    sb.Append(upper[r.Next(0, upper.Length - 1)]);
                }
            }
            return sb.ToString();
        }

        public long VerifyToken(string token)
        {
            if (!_db.AccessTokens.Any(a => a.Token == token))
            {
                throw new ArgumentException("Token is not found.");
            }
            AccessToken accessToken = _db.AccessTokens.First(a => a.Token == token);
            return accessToken.UserId;
        }
    }
}
