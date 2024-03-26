namespace EnglishMaster.Shared.Models
{
    public partial class Idiom
    {
        public long Id { get; set; }

        public string Idiom1 { get; set; } = null!;

        public virtual ICollection<MeaningOfIdiom> MeaningOfIdioms { get; set; } = new List<MeaningOfIdiom>();
    }
}