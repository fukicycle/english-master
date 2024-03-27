namespace EnglishMaster.Shared.Dto.Response
{
    public class UserResponseDto
    {
        public UserResponseDto(string firstName, string lastName, string? nickname)
        {
            FirstName = firstName;
            LastName = lastName;
            Nickname = nickname;
        }
        public string FirstName { get; }
        public string LastName { get; }
        public string? Nickname { get; }

        public string GetDisplayName()
        {
            if (string.IsNullOrEmpty(Nickname))
            {
                return $"{FirstName} {LastName}";
            }
            else
            {
                return Nickname;
            }
        }
    }
}