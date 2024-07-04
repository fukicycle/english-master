namespace EnglishMaster.Shared.Models
{
    public partial class MeaningOfWordLearningHistory
    {
        public long Id { get; set; }

        public long UserId { get; set; }

        public long QuestionMeaningOfWordId { get; set; }

        public DateTime Date { get; set; }

        public bool IsCorrect { get; set; }
        public long ModeId { get; set; }


        public virtual MeaningOfWord QuestionMeaningOfWord { get; set; } = null!;
        public virtual Mode Mode { get; set; } = null!;

        public virtual User User { get; set; } = null!;
    }
}