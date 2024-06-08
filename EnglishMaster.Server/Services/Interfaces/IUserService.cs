using EnglishMaster.Shared.Dto.Response;

namespace EnglishMaster.Server.Services.Interfaces
{
    public interface IUserService
    {
        void Register(string email, string password, string firstName, string lastName, string? nickname,string? iconUrl);
        UserResponseDto GetUserResponseDto(long userId);
    }
}
