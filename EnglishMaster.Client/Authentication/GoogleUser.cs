using System.Security.Principal;
using System.Text.Json.Serialization;

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
        [JsonPropertyName("verified_email")]
        public bool VerifiedEmail { get; }
        public string Name { get; }
        [JsonPropertyName("given_name")]
        public string GivenName { get; }
        [JsonPropertyName("family_name")]
        public string FamilyName { get; }
        public string Picture { get; }
    }
}
