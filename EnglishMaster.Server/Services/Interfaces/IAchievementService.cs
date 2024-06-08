using EnglishMaster.Shared.Dto.Response;

namespace EnglishMaster.Server.Services.Interfaces
{
    public interface IAchievementService
    {
        IList<AchievementResponseDto> GetAchievementResponseDtos(long userId);
        IList<AchievementGraphResponseDto> GetAchievementGraphResponseDtosByWeek(long userId);
        IList<AchievementGraphResponseDto> GetAchievementGraphResponseDtosByPartOfSpeech(long userId);
        IList<TreeFarmResponseDto> GetTreeFarmData(long userId, DateTime startDate);
    }
}