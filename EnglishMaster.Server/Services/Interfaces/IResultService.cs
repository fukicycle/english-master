using EnglishMaster.Shared.Dto.Request;
using EnglishMaster.Shared.Dto.Response;

namespace EnglishMaster.Server.Services.Interfaces
{
    public interface IResultService
    {
        IList<ResultResponseDto> GetResultResponseDtosByEmail(string email, int count);
        int RegisterResult(string email, IEnumerable<ResultRequestDto> results);
    }
}
