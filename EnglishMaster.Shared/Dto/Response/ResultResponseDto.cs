namespace EnglishMaster.Shared.Dto.Response
{
    public sealed class ResultResponseDto
    {
        public ResultResponseDto(long wordId, string word, string userAnswer, string correctAnswer)
        {
            WordId = wordId;
            Word = word;
            UserAnswer = userAnswer;
            CorrectAnswer = correctAnswer;
        }
        public long WordId { get; }
        public string Word { get; }
        public string UserAnswer { get; }
        public string CorrectAnswer { get; }
    }
}
