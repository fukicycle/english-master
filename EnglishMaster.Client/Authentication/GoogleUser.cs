using System.Security.Principal;

namespace EnglishMaster.Client.Authentication
{
    public sealed class GoogleUser
    {
        public GoogleUser(string id, string email, bool verifiedEmail, string name, string givenName, string familyName, string picture)
        {
            Id = id;
            Email = email;
            VerifiedEmail = verifiedEmail;
            Name = name;
            GivenName = givenName;
            FamilyName = familyName;
            Picture = picture;
        }
        public string Id { get; }
        public string Email { get; }
        public bool VerifiedEmail { get; }
        public string Name { get; }
        public string GivenName { get; }
        public string FamilyName { get; }
        public string Picture { get; }
    }
}
