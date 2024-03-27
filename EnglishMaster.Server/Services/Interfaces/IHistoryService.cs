using EnglishMaster.Shared.Dto.Response;

namespace EnglishMaster.Server.Services.Interfaces
{
    public interface IHistoryService
    {
        int GetNumberOfAnswer(string email);
        IList<HistoryResponseDto> GetHistoryResponseDtos(string email);
    }
}