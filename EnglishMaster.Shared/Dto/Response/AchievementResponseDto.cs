namespace EnglishMaster.Shared.Dto.Response
{
    public sealed class AchievementResponseDto
    {
        public AchievementResponseDto(string levelName, IList<AchievementDetailResponseDto> details)
        {
            LevelName = levelName;
            Details = details;
            if (Details.Any())
            {
                Progress = Math.Round(Details.Average(a => a.Progress), 0);
            }
            else
            {
                Progress = 0;
            }

        }
        public string LevelName { get; }
        public IList<AchievementDetailResponseDto> Details { get; }
        public double Progress { get; }
    }
}