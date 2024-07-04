using EnglishMaster.Shared.Dto.Response;

namespace EnglishMaster.Server.Services.Interfaces
{
    public interface IQuestionService
    {
        IList<QuestionResponseDto> GetQuestionResponseDtos(long partOfSpeechId = 0, long levelId = 0, int numberOfQuestions = 10);
        IList<QuestionResponseDto> GetQuestionResponseDtos(long userId, long partOfSpeechId = 0, long levelId = 0, int numberOfQuestions = 10);
    }
}