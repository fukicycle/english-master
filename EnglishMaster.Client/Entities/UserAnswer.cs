namespace EnglishMaster.Client.Entities
{
    public sealed class UserAnswer
    {
        public UserAnswer(string questionWord, string answerMeaning, string questionMeaning, long answerMeaningOfWordId, long questionMeaningOfWordId)
        {
            QuestionWord = questionWord;
            AnswerMeaning = answerMeaning;
            QuestionMeaning = questionMeaning;
            AnswerMeaningOfWordId = answerMeaningOfWordId;
            QuestionMeaningOfWordId = questionMeaningOfWordId;
        }
        public string QuestionWord { get; }
        public string AnswerMeaning { get; }
        public string QuestionMeaning { get; }
        public long AnswerMeaningOfWordId { get; }
        public long QuestionMeaningOfWordId { get; }
    }
}
