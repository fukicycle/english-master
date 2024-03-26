namespace EnglishMaster.Shared.Models
{
    public class Rank
    {
        public long UserID { get; set; }
        public string Nickname { get; set; } = null!;
        public int Ranking { get; set; }
        public decimal CorrectRate { get; set; }
        public int Point { get; set; }
    }
}