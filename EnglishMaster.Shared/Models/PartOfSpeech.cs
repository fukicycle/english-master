﻿namespace EnglishMaster.Shared.Models
{
    public partial class PartOfSpeech
    {
        public long Id { get; set; }

        public string Name { get; set; } = null!;

        public string InJapanese { get; set; } = null!;

        public virtual ICollection<MeaningOfWord> MeaningOfWords { get; set; } = new List<MeaningOfWord>();
    }
}