using EnglishMaster.Shared.Dto.Request;

namespace EnglishMaster.Server.Services.Interfaces;

public interface IFlushCardResultService
{
    void Register(FlushCardResultRequestDto flushCardResultRequestDto, long userId);
}
