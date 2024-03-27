namespace EnglishMaster.Shared.Dto.Request
{
    public sealed class UserReqestDto
    {
        public UserReqestDto(string username, string password, string firstName, string lastName, string? nickname)
        {
            Username = username;
            Password = password;
            FirstName = firstName;
            LastName = lastName;
            Nickname = nickname;
        }
        public string Username { get; }
        public string Password { get; }
        public string FirstName { get; }
        public string LastName { get; }
        public string? Nickname { get; }
    }
}
