namespace EnglishMaster.Shared.Models
{
    public partial class MeaningOfIdiom
    {
        public long Id { get; set; }

        public long IdiomId { get; set; }

        public string Meaning { get; set; } = null!;

        public virtual Idiom Idiom { get; set; } = null!;
    }
}