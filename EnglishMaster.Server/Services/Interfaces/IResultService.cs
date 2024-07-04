using EnglishMaster.Shared.Dto.Request;
using EnglishMaster.Shared.Dto.Response;

namespace EnglishMaster.Server.Services.Interfaces
{
    public interface IResultService
    {
        int RegisterResult(long userId, IEnumerable<ResultRequestDto> results);
    }
}
