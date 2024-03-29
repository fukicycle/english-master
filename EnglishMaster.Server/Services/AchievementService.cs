using EnglishMaster.Server.Services.Interfaces;
using EnglishMaster.Shared.Dto.Response;
using EnglishMaster.Shared.Models;
using Microsoft.EntityFrameworkCore;

namespace EnglishMaster.Server.Services
{
    public sealed class AchievementService : IAchievementService
    {
        private readonly DB _db;
        private readonly ILogger<AchievementService> _logger;
        public AchievementService(DB db, ILogger<AchievementService> logger)
        {
            _db = db;
            _logger = logger;
        }
        public IList<AchievementResponseDto> GetAchievementResponseDtosByEmail(string email)
        {
            List<AchievementResponseDto> achievementResponseDtos = new List<AchievementResponseDto>();
            User? user = _db.Users
                            .Include(a => a.MeaningOfWordLearningHistories)
                            .ThenInclude(a => a.QuestionMeaningOfWord)
                            .ThenInclude(a => a.PartOfSpeech)
                            .Include(a => a.MeaningOfWordLearningHistories)
                            .ThenInclude(a => a.QuestionMeaningOfWord)
                            .ThenInclude(a => a.Level)
                            .FirstOrDefault(a => a.Username == email);
            if (user == null)
            {
                return achievementResponseDtos;
            }
            IList<Level> levels = _db.Levels.Include(a => a.MeaningOfWords).Where(a => a.MeaningOfWords.Count >= 50).ToList();
            foreach (Level level in levels)
            {
                IList<AchievementDetailResponseDto> detailResponseDtos = new List<AchievementDetailResponseDto>();
                IList<PartOfSpeech> partOfSpeeches = _db.PartOfSpeeches.Include(a => a.MeaningOfWords).Where(a => a.MeaningOfWords.Count >= 50).ToList();
                foreach (PartOfSpeech partOfSpeech in partOfSpeeches)
                {
                    int total = partOfSpeech.MeaningOfWords.Where(a => a.LevelId == level.Id).Count();
                    int actual = user.MeaningOfWordLearningHistories.Where(a => a.QuestionMeaningOfWord.LevelId == level.Id && a.QuestionMeaningOfWord.PartOfSpeechId == partOfSpeech.Id && a.AnswerMeaningOfWordId == a.QuestionMeaningOfWordId).GroupBy(a => a.QuestionMeaningOfWordId).Count();
                    _logger.LogInformation($"Actual/Total[{partOfSpeech.Name},{level.Name}]:{actual}/{total}");
                    detailResponseDtos.Add(new AchievementDetailResponseDto(partOfSpeech.Name, total, actual));
                }
                achievementResponseDtos.Add(new AchievementResponseDto(level.Name, detailResponseDtos));
            }
            return achievementResponseDtos;
        }
    }
}