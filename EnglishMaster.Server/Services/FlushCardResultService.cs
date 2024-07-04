using EnglishMaster.Server.Services.Interfaces;
using EnglishMaster.Shared;
using EnglishMaster.Shared.Dto.Request;
using EnglishMaster.Shared.Models;

namespace EnglishMaster.Server.Services;

public sealed class FlushCardResultService : IFlushCardResultService
{
    private readonly ILogger<FlushCardResultService> _logger;
    private readonly DB _db;
    public FlushCardResultService(ILogger<FlushCardResultService> logger, DB db)
    {
        _db = db;
        _logger = logger;
    }
    public void Register(FlushCardResultRequestDto flushCardResultRequestDto, long userId)
    {
        IEnumerable<MeaningOfWord> meaningOfWords = _db.MeaningOfWords.Where(a => a.WordId == flushCardResultRequestDto.WordId);
        foreach (MeaningOfWord meaningOfWord in meaningOfWords)
        {
            _db.MeaningOfWordLearningHistories.Add(
                new MeaningOfWordLearningHistory
                {
                    QuestionMeaningOfWordId = meaningOfWord.Id,
                    Date = DateTime.Now,
                    UserId = userId,
                    IsCorrect = flushCardResultRequestDto.IsCorrect,
                    ModeId = StudyMode.Flush
                }
            );
        }
        _db.SaveChanges();
    }
}
