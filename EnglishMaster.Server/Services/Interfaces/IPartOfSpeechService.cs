using EnglishMaster.Shared;
using EnglishMaster.Shared.Dto.Response;

namespace EnglishMaster.Server.Services.Interfaces;

public interface IPartOfSpeechService
{
    IList<PartOfSpeechResponseDto> GetPartOfSpeechResponseDtos();
}
