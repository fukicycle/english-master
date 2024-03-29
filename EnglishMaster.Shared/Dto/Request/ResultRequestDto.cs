namespace EnglishMaster.Shared.Dto.Request
{
    public sealed class ResultRequestDto
    {
        public ResultRequestDto(long questionMeaningOfWordId, long answerMeaningOfWordId)
        {
            QuestionMeaningOfWordId = questionMeaningOfWordId;
            AnswerMeaningOfWordId = answerMeaningOfWordId;
        }
        public long QuestionMeaningOfWordId { get; }
        public long AnswerMeaningOfWordId { get; }
    }
}
