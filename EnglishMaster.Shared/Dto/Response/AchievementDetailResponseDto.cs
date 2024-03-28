namespace EnglishMaster.Shared.Dto.Response
{
    public sealed class AchievementDetailResponseDto
    {
        public AchievementDetailResponseDto(string partOfSpeechName, int total, int actual)
        {
            PartOfSpeechName = partOfSpeechName;
            Total = total;
            Actual = actual;
            if (Actual > 0 && Total > 0)
            {
                Progress = Math.Round(Actual * 100.0 / Total, 0);
            }
            else
            {
                Progress = 0;
            }
        }
        public string PartOfSpeechName { get; }
        public int Total { get; }
        public int Actual { get; }
        public double Progress { get; }
    }
}