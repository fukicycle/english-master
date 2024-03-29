namespace EnglishMaster.Shared.Dto.Request
{
    public sealed class ResultRequestDto
    {
        public ResultRequestDto(string email, long questionMeaningOfWordId, long answerMeaningOfWordId)
        {
            Email = email;
            QuestionMeaningOfWordId = questionMeaningOfWordId;
            AnswerMeaningOfWordId = answerMeaningOfWordId;
        }
        public string Email { get; }
        public long QuestionMeaningOfWordId { get; }
        public long AnswerMeaningOfWordId { get; }
    }
}
