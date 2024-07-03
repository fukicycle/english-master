using System;
using System.Collections.Generic;

namespace EnglishMaster.Server.Models;

public partial class Idiom
{
    public long Id { get; set; }

    public string Idiom1 { get; set; } = null!;

    public virtual ICollection<MeaningOfIdiom> MeaningOfIdioms { get; set; } = new List<MeaningOfIdiom>();
}
