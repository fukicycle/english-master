using EnglishMaster.Shared.Dto.Response;

namespace EnglishMaster.Server.Services.Interfaces;

public interface ILevelService
{
    IList<LevelResponseDto> GetLevelResponseDtos();
}
