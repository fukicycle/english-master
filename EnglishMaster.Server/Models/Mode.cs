using System;
using System.Collections.Generic;

namespace EnglishMaster.Server.Models;

public partial class Mode
{
    public long Id { get; set; }

    public string Name { get; set; } = null!;

    public string Description { get; set; } = null!;

    public virtual ICollection<MeaningOfWordLearningHistory> MeaningOfWordLearningHistories { get; set; } = new List<MeaningOfWordLearningHistory>();
}
