using System;
using System.Collections.Generic;

namespace EnglishMaster.Server.Models;

public partial class MeaningOfWordLearningHistory
{
    public long Id { get; set; }

    public long UserId { get; set; }

    public long QuestionMeaningOfWordId { get; set; }

    public DateTime Date { get; set; }

    public long? ModeId { get; set; }

    public bool IsCorrect { get; set; }

    public virtual Mode? Mode { get; set; }

    public virtual MeaningOfWord QuestionMeaningOfWord { get; set; } = null!;

    public virtual User User { get; set; } = null!;
}
