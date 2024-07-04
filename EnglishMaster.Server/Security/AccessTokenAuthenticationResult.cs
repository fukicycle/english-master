namespace EnglishMaster.Server.Security
{
    public sealed class AccessTokenAuthenticationResult
    {
        public AccessTokenAuthenticationResult(AccessTokenAuthenticationResultCode resultCode, string? token, DateTime? expires)
        {
            ResultCode = resultCode;
            Token = token;
            Expires = expires;
        }
        public AccessTokenAuthenticationResultCode ResultCode { get; }
        public string? Token { get; }
        public DateTime? Expires { get; set; }
    }
}
