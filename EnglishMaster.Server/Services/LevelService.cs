using EnglishMaster.Server.Services.Interfaces;
using EnglishMaster.Shared.Dto.Response;
using EnglishMaster.Shared.Models;

namespace EnglishMaster.Server.Services
{
    public sealed class LevelService : ILevelService
    {
        private readonly DB _db;
        private readonly ILogger<LevelService> _logger;

        public LevelService(DB db, ILogger<LevelService> logger)
        {
            _db = db;
            _logger = logger;
        }
        public IList<LevelResponseDto> GetLevelResponseDtos()
        {
            IEnumerable<Level> levels = _db.Levels.Where(a => a.MeaningOfWords.Count >= 50).ToList();
            IList<LevelResponseDto> levelResponseDtos = new List<LevelResponseDto>();
            foreach (Level level in levels)
            {
                LevelResponseDto levelResponseDto = new LevelResponseDto(level.Id, level.Name);
                levelResponseDtos.Add(levelResponseDto);
            }
            return levelResponseDtos;
        }
    }
}