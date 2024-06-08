namespace EnglishMaster.Server.Security
{
    public sealed class AccessTokenAuthenticationResult
    {
        public AccessTokenAuthenticationResult(AccessTokenAuthenticationResultCode resultCode, string? token)
        {
            ResultCode = resultCode;
            Token = token;
        }
        public AccessTokenAuthenticationResultCode ResultCode { get; }
        public string? Token { get; }
    }
}
