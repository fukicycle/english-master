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
                    Date = DateTime.Now,
                    IsCorrect = result.QuestionMeaningOfWordId == result.AnswerMeaningOfWordId
                });
            }
            return _db.SaveChanges();
        }
    }
}
