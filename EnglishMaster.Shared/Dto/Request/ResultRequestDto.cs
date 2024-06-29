namespace EnglishMaster.Shared.Dto.Request
{
    public sealed class ResultRequestDto
    {
        public ResultRequestDto(long questionMeaningOfWordId, bool isCorrect, long modeId)
        {
            QuestionMeaningOfWordId = questionMeaningOfWordId;
            IsCorrect = isCorrect;
            ModeId = modeId;
        }
        public long QuestionMeaningOfWordId { get; }
        public bool IsCorrect { get; }
        public long ModeId { get; }
    }
}
