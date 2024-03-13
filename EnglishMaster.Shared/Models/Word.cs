using System;
using System.Collections.Generic;

namespace EnglishMaster.Shared.Models;

public partial class Word
{
    public long Id { get; set; }

    public string Word1 { get; set; } = null!;

    public virtual ICollection<MeaningOfWord> MeaningOfWords { get; set; } = new List<MeaningOfWord>();
}
