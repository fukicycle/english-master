namespace EnglishMaster.Client.Authentication
{
    public sealed class StoredAccessToken
    {
        public StoredAccessToken(string token, DateTime expires)
        {
            Token = token;
            Expires = expires;
        }
        public string Token { get; set; }
        public DateTime Expires { get; }
    }
}
