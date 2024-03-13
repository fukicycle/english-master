namespace EnglishMaster.Shared.Dto.Response
{
    public class WordResultResponseDto
    {
        public string Word { get; set; } = null!;
        public long WordID { get; set; }
        public string Answer { get; set; } = null!;
        public string Question { get; set; } = null!;
        public bool IsCorrect { get; set; }
        public DateTime Date { get; set; }
    }
}
