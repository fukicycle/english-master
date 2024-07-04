using EnglishMaster.Shared.Dto.Response;

namespace EnglishMaster.Server.Services.Interfaces;

public interface IFlushCardQuestionService
{
    IEnumerable<FlushCardResponseDto> GetFlushCardResponseDtos();
}
