using EnglishMaster.Shared;
using EnglishMaster.Shared.Dto.Response;

namespace EnglishMaster.Server.Services.Interfaces;
public interface IQuestionService
{
    IList<QuestionResponseDto> GetQuestionResponseDtos(long partOfSpeechId, long levelId = 0, int numberOfQuestions = 10);
}
