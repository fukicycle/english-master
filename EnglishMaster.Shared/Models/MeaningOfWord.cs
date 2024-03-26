﻿namespace EnglishMaster.Shared.Models
{
    public partial class MeaningOfWord
    {
        public long Id { get; set; }

        public long PartOfSpeechId { get; set; }

        public long WordId { get; set; }

        public string Meaning { get; set; } = null!;

        public long LevelId { get; set; }

        public virtual Level Level { get; set; } = null!;

        public virtual ICollection<MeaningOfWordLearningHistory> MeaningOfWordLearningHistoryAnswerMeaningOfWords { get; set; } = new List<MeaningOfWordLearningHistory>();

        public virtual ICollection<MeaningOfWordLearningHistory> MeaningOfWordLearningHistoryQuestionMeaningOfWords { get; set; } = new List<MeaningOfWordLearningHistory>();

        public virtual PartOfSpeech PartOfSpeech { get; set; } = null!;

        public virtual Word Word { get; set; } = null!;
    }
}