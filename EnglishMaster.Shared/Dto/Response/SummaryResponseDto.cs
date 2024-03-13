namespace EnglishMaster.Shared.Dto.Response
{
    public class SummaryResponseDto
    {
        public int Total { get; set; }
        public int Correct { get; set; }
        public int Incorrect { get; set; }
        public DateTime Date { get; set; }
    }
}