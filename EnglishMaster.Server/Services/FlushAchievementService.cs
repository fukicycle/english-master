﻿using EnglishMaster.Server.Services.Interfaces;
using EnglishMaster.Shared;
using EnglishMaster.Shared.Dto.Response;
using EnglishMaster.Shared.Models;
using Microsoft.EntityFrameworkCore;

namespace EnglishMaster.Server.Services
{
    public sealed class FlushAchievementService : IAchievementService
    {
        private readonly DB _db;
        private readonly ILogger<FlushAchievementService> _logger;
        public FlushAchievementService(DB db, ILogger<FlushAchievementService> logger)
        {
            _db = db;
            _logger = logger;
        }

        public IList<AchievementGraphResponseDto> GetAchievementGraphResponseDtosByPartOfSpeech(long userId)
        {
            List<AchievementGraphResponseDto> achievementGraphResponseDtos = new List<AchievementGraphResponseDto>();
            User? user = _db.Users
                            .Include(a => a.MeaningOfWordLearningHistories)
                            .ThenInclude(a => a.QuestionMeaningOfWord)
                            .ThenInclude(a => a.PartOfSpeech)
                            .Include(a => a.MeaningOfWordLearningHistories)
                            .ThenInclude(a => a.QuestionMeaningOfWord)
                            .FirstOrDefault(a => a.Id == userId);
            if (user == null)
            {
                return achievementGraphResponseDtos;
            }
            IList<PartOfSpeech> partOfSpeeches = _db.PartOfSpeeches
                                                    .Include(a => a.MeaningOfWords)
                                                    .Where(a => a.MeaningOfWords.Count() >= ApplicationSettings.NUMBER_OF_MIN_LIMIT)
                                                    .ToList();
            foreach (PartOfSpeech partOfSpeech in partOfSpeeches)
            {
                int numberOfAnswerWord = user.MeaningOfWordLearningHistories
                                            .Where(a => a.ModeId == StudyMode.Flush)
                                            .Count(a => a.QuestionMeaningOfWord.PartOfSpeechId == partOfSpeech.Id);
                int numberOfCoorectAnswerWord = user.MeaningOfWordLearningHistories
                                                 .Where(a => a.QuestionMeaningOfWord.PartOfSpeechId == partOfSpeech.Id && a.ModeId == StudyMode.Flush)
                                                 .Count(a => a.IsCorrect);
                if (numberOfAnswerWord == 0)
                {
                    //Parts of speech with no answer will not be displayed.
                }
                else
                {
                    decimal correctAnswerRate = Math.Round((numberOfCoorectAnswerWord + 0.0m) / numberOfAnswerWord * 100.0m, 1);
                    achievementGraphResponseDtos.Add(new AchievementGraphResponseDto(partOfSpeech.Name, correctAnswerRate));
                }
            }
            return achievementGraphResponseDtos;
        }

        public IList<AchievementGraphResponseDto> GetAchievementGraphResponseDtosByWeek(long userId)
        {
            List<AchievementGraphResponseDto> achievementGraphResponseDtos = new List<AchievementGraphResponseDto>();
            User? user = _db.Users
                            .Include(a => a.MeaningOfWordLearningHistories)
                            .ThenInclude(a => a.QuestionMeaningOfWord)
                            .ThenInclude(a => a.PartOfSpeech)
                            .Include(a => a.MeaningOfWordLearningHistories)
                            .ThenInclude(a => a.QuestionMeaningOfWord)
                            .FirstOrDefault(a => a.Id == userId);
            if (user == null)
            {
                return achievementGraphResponseDtos;
            }
            DateTime startDate = DateTime.Today.AddDays(-7);
            for (DateTime dt = startDate; dt < DateTime.Today; dt = dt.AddDays(1))
            {
                int numberOfAnswerWord = user.MeaningOfWordLearningHistories
                                            .Count(a => a.Date.Date == dt && a.ModeId == StudyMode.Flush);
                int numberOfCorrectAnswerWord = user.MeaningOfWordLearningHistories
                                                .Where(a => a.Date.Date == dt && a.ModeId == StudyMode.Flush)
                                                .Count(a => a.IsCorrect);
                if (numberOfAnswerWord == 0)
                {
                    achievementGraphResponseDtos.Add(new AchievementGraphResponseDto(dt.ToString("MM/dd"), null));
                }
                else
                {
                    decimal correctAnswerRate = Math.Round((numberOfCorrectAnswerWord + 0.0m) / numberOfAnswerWord * 100.0m, 1);
                    achievementGraphResponseDtos.Add(new AchievementGraphResponseDto(dt.ToString("MM/dd"), correctAnswerRate));
                }
            }
            return achievementGraphResponseDtos;
        }

        public IList<AchievementResponseDto> GetAchievementResponseDtos(long userId)
        {
            List<AchievementResponseDto> achievementResponseDtos = new List<AchievementResponseDto>();
            User? user = _db.Users
                            .Include(a => a.MeaningOfWordLearningHistories)
                            .ThenInclude(a => a.QuestionMeaningOfWord)
                            .ThenInclude(a => a.PartOfSpeech)
                            .Include(a => a.MeaningOfWordLearningHistories)
                            .ThenInclude(a => a.QuestionMeaningOfWord)
                            .ThenInclude(a => a.Level)
                            .FirstOrDefault(a => a.Id == userId);
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
                    int actual = user.MeaningOfWordLearningHistories
                                .Where(a => a.QuestionMeaningOfWord.LevelId == level.Id &&
                                            a.QuestionMeaningOfWord.PartOfSpeechId == partOfSpeech.Id &&
                                            a.IsCorrect &&
                                            a.ModeId == StudyMode.Flush)
                                .GroupBy(a => a.QuestionMeaningOfWordId)
                                .Count();
                    _logger.LogInformation($"Actual/Total[{partOfSpeech.Name},{level.Name}]:{actual}/{total}");
                    detailResponseDtos.Add(new AchievementDetailResponseDto(partOfSpeech.Name, total, actual));
                }
                achievementResponseDtos.Add(new AchievementResponseDto(level.Name, detailResponseDtos));
            }
            return achievementResponseDtos;
        }

        public IList<TreeFarmResponseDto> GetTreeFarmData(long userId, DateTime startDate)
        {
            throw new NotImplementedException("Please use Choice Achievement service.");
        }
    }
}