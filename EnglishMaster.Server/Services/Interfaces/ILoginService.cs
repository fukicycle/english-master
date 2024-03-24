using EnglishMaster.Shared.Dto.Response;

namespace EnglishMaster.Server.Services.Interfaces;

public interface ILoginService
{
    LoginResponseDto Login(string email);

    protected string GenerateJWTToken(string email);
}
