namespace EnglishMaster.Shared.Dto.Request;

public sealed class LoginRequestDto
{
    public LoginRequestDto(string email)
    {
        Email = email;
    }
    public string Email { get; }
}
