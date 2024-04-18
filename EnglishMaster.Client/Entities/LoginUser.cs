namespace EnglishMaster.Client.Entities
{
    public sealed class LoginUser
    {
        public LoginUser(string firstName, string lastName, string? iconUrl)
        {
            Fullname = $"{firstName} {lastName}";
            IconUrl = iconUrl;
        }
        public string Fullname { get; }
        public string? IconUrl { get; }
    }
}
