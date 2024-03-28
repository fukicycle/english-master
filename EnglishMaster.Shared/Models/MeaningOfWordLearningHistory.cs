namespace EnglishMaster.Shared.Models
{
    public partial class MeaningOfWordLearningHistory
    {
        public long Id { get; set; }

        public long UserId { get; set; }

        public long QuestionMeaningOfWordId { get; set; }

        public long? AnswerMeaningOfWordId { get; set; }

        public DateTime Date { get; set; }

        public bool IsDone { get; set; }

        public virtual MeaningOfWord? AnswerMeaningOfWord { get; set; }

        public virtual MeaningOfWord QuestionMeaningOfWord { get; set; } = null!;

        public virtual User User { get; set; } = null!;
    }
}