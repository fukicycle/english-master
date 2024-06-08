using EnglishMaster.Shared.Dto.Request;
using EnglishMaster.Shared.Dto.Response;

namespace EnglishMaster.Server.Services.Interfaces
{
    public interface IResultService
    {
        IList<ResultResponseDto> GetResultResponseDtos(long userId, int count);
        int RegisterResult(long userId, IEnumerable<ResultRequestDto> results);
    }
}
