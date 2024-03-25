namespace EnglishMaster.Shared.Dto.Request;

public sealed class LoginRequestDto
{
    public LoginRequestDto(string email, string password)
    {
        Email = email;
        Password = password;
    }
    public string Email { get; }
    public string Password { get; }
}
