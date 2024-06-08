namespace EnglishMaster.Server.Security.Service
{
    public interface IAccessTokenAuthenticationService
    {
        AccessTokenAuthenticationResult Authenticate(string username, string password);
        long VerifyToken(string token);
    }
}
