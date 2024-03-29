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
        public IList<ResultResponseDto> GetResultResponseDtosByEmail(string email)
        {
            User user = _db.Users.Single(a => a.Username == email);
            IList<MeaningOfWordLearningHistory> histories = _db.MeaningOfWordLearningHistories
                                                                .Include(a => a.QuestionMeaningOfWord)
                                                                .Include(a => a.AnswerMeaningOfWord)
                                                                .Where(a => a.UserId == user.Id).ToList();
            IList<ResultResponseDto> result = new List<ResultResponseDto>();
            foreach (MeaningOfWordLearningHistory history in histories)
            {
                result.Add(new ResultResponseDto(
                    history.QuestionMeaningOfWordId,
                    history.QuestionMeaningOfWord.Word.Word1,
                    history.AnswerMeaningOfWord?.Word?.Word1 ?? "",
                    history.QuestionMeaningOfWord.Word.Word1));
            }
            return result;
        }

        public int RegisterResult(string email, IEnumerable<ResultRequestDto> results)
        {
            if (!results.Any())
            {
                return 0;
            }
            User user = _db.Users.Single(a => a.Username == email);
            foreach (ResultRequestDto result in results)
            {
                _db.MeaningOfWordLearningHistories.Add(new MeaningOfWordLearningHistory
                {
                    UserId = user.Id,
                    QuestionMeaningOfWordId = result.QuestionMeaningOfWordId,
                    AnswerMeaningOfWordId = result.AnswerMeaningOfWordId,
                    Date = DateTime.Now,
                    IsDone = result.QuestionMeaningOfWordId == result.AnswerMeaningOfWordId
                });
            }
            return _db.SaveChanges();
        }
    }
}
