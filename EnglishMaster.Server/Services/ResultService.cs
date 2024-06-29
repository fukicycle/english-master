using EnglishMaster.Server.Services.Interfaces;
using EnglishMaster.Shared.Dto.Request;
using EnglishMaster.Shared.Dto.Response;
using EnglishMaster.Shared.Models;
using Microsoft.EntityFrameworkCore;

namespace EnglishMaster.Server.Services
{
    public sealed class ResultService : IResultService
    {
        private readonly DB _db;
        private readonly ILogger<ResultService> _logger;
        public ResultService(DB db, ILogger<ResultService> logger)
        {
            _db = db;
            _logger = logger;
        }
        public IList<ResultResponseDto> GetResultResponseDtos(long userId, int count)
        {
            User user = _db.Users.Single(a => a.Id == userId);
            IList<MeaningOfWordLearningHistory> histories = _db.MeaningOfWordLearningHistories
                                                                .Include(a => a.QuestionMeaningOfWord)
                                                                .ThenInclude(a => a.Word)
                                                                .Include(a => a.AnswerMeaningOfWord)
                                                                .ThenInclude(a => a.Word)
                                                                .Where(a => a.UserId == user.Id)
                                                                .OrderByDescending(a => a.Date)
                                                                .Take(count)
                                                                .ToList();
            IList<ResultResponseDto> result = new List<ResultResponseDto>();
            foreach (MeaningOfWordLearningHistory history in histories)
            {
                result.Add(new ResultResponseDto(
                    history.QuestionMeaningOfWordId,
                    history.QuestionMeaningOfWord.Word.Word1,
                    history.AnswerMeaningOfWord?.Meaning ?? "",
                    history.QuestionMeaningOfWord.Meaning));
            }
            return result;
        }

        public int RegisterResult(long userId, IEnumerable<ResultRequestDto> results)
        {
            if (!results.Any())
            {
                _logger.LogWarning("Empty results.");
                return 0;
            }
            User user = _db.Users.Single(a => a.Id == userId);
            foreach (ResultRequestDto result in results)
            {
                _db.MeaningOfWordLearningHistories.Add(new MeaningOfWordLearningHistory
                {
                    UserId = user.Id,
                    QuestionMeaningOfWordId = result.QuestionMeaningOfWordId,
                    AnswerMeaningOfWordId = result.AnswerMeaningOfWordId,
                    Date = DateTime.Now,
                    IsCorrect = result.QuestionMeaningOfWordId == result.AnswerMeaningOfWordId
                });
            }
            return _db.SaveChanges();
        }
    }
}
