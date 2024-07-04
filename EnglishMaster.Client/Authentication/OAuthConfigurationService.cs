using System.Text;

namespace EnglishMaster.Client.Authentication
{
    public sealed class OAuthConfigurationService
    {
        private readonly IConfiguration _configuration;
        public OAuthConfigurationService(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        private const string ClientId = "client_id";
        private const string ResponseType = "response_type";
        private const string RedirectUri = "redirect_uri";
        private const string Scope = "scope";

        public string GetOAuthUri()
        {
            GoogleOAuth? googleOAuth = _configuration.GetSection(nameof(GoogleOAuth)).Get<GoogleOAuth>();
            if (googleOAuth == null)
            {
                throw new ArgumentException("Please setup google oauth configuration.");
            }
            return GenerateUri(googleOAuth);
        }

        private string GenerateUri(GoogleOAuth googleOAuth)
        {
            StringBuilder sb = new StringBuilder();
            sb.Append(googleOAuth.BaseUri);
            sb.Append("?");
            sb.Append($"{ClientId}={googleOAuth.ClientId}");
            sb.Append("&");
            sb.Append($"{RedirectUri}={googleOAuth.RedirectUri}");
            sb.Append("&");
            sb.Append($"{ResponseType}={googleOAuth.ResponseType}");
            sb.Append("&");
            sb.Append($"{Scope}={string.Join("+", googleOAuth.Scopes)}");
            return sb.ToString();
        }
    }
}
