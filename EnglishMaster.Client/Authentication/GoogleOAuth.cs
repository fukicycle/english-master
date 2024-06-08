namespace EnglishMaster.Client.Authentication
{
    public sealed class GoogleOAuth
    {
        public string BaseUri { get; set; } = string.Empty;
        public string ClientId { get; set; } = string.Empty;
        public string[] Scopes { get; set; } = Array.Empty<string>();
        public string RedirectUri { get; set; } = string.Empty;
        public string ResponseType { get; set; } = string.Empty;
    }
}
