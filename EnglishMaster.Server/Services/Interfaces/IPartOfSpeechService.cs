using EnglishMaster.Shared;

namespace EnglishMaster.Server;

public interface IPartOfSpeechService
{
    IList<PartOfSpeechResponseDto> GetPartOfSpeechResponseDtos();
}
