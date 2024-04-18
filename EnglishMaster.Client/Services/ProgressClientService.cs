using EnglishMaster.Shared.Dto.Response;

namespace EnglishMaster.Client.Services
{
    public sealed class ProgressClientService
    {
        private readonly ILogger<ProgressClientService> _logger;

        public ProgressClientService(ILogger<ProgressClientService> logger)
        {
            _logger = logger;
        }

        public double GetProgress(List<AchievementResponseDto> achievements)
        {
            if (achievements.Count == 0) return 0;
            return Math.Round(achievements.Average(a => a.Progress), 2);
        }

    }
}
