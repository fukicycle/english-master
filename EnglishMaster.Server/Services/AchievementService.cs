using EnglishMaster.Server.Services.Interfaces;
using EnglishMaster.Shared;
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
            IList<MeaningOfWord> meaningOfWords = _db.MeaningOfWords.ToList();
            IList<Level> levels = _db.Levels.ToList();
            foreach (Level level in levels)
            {
                if (meaningOfWords.Count(a => a.LevelId == level.Id) < ApplicationSettings.NUMBER_OF_MIN_LIMIT)
                {
                    continue;
                }
                IList<AchievementDetailResponseDto> detailResponseDtos = new List<AchievementDetailResponseDto>();
                IList<PartOfSpeech> partOfSpeeches = _db.PartOfSpeeches.ToList();
                foreach (PartOfSpeech partOfSpeech in partOfSpeeches)
                {
                    if (meaningOfWords.Count(a => a.LevelId == level.Id && a.PartOfSpeechId == partOfSpeech.Id) < ApplicationSettings.NUMBER_OF_MIN_LIMIT)
                    {
                        continue;
                    }
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