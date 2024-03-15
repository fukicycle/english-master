using EnglishMaster.Shared;

namespace EnglishMaster.Server;

public interface ILevelService
{
    IList<LevelResponseDto> GetLevelResponseDtos();
}
