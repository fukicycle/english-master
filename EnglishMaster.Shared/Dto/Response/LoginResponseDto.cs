namespace EnglishMaster.Shared.Dto.Response;

public class LoginResponseDto
{
    public LoginResponseDto(string token)
    {
        Token = token;
    }
    public string Token { get; }
}
