﻿using System;
using System.Collections.Generic;

namespace EnglishMaster.Server.Models;

public partial class Level
{
    public long Id { get; set; }

    public string Name { get; set; } = null!;

    public virtual ICollection<MeaningOfWord> MeaningOfWords { get; set; } = new List<MeaningOfWord>();
}
