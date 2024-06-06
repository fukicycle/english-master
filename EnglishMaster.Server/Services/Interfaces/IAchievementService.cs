using EnglishMaster.Shared.Dto.Response;

namespace EnglishMaster.Server.Services.Interfaces
{
    public interface IAchievementService
    {
        IList<AchievementResponseDto> GetAchievementResponseDtosByEmail(string email);
        IList<AchievementGraphResponseDto> GetAchievementGraphResponseDtosByWeek(string email);
        IList<AchievementGraphResponseDto> GetAchievementGraphResponseDtosByPartOfSpeech(string email);
    }
}