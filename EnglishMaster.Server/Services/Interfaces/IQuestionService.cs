using EnglishMaster.Shared;

namespace EnglishMaster.Server;
public interface IQuestionService
{
    IList<QuestionResponseDto> GetQuestionResponseDtos(long partOfSpeechId, long levelId = 0, int numberOfQuestions = 50);
}
