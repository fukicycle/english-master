using EnglishMaster.Server.Services.Interfaces;
using EnglishMaster.Shared.Dto.Response;
using EnglishMaster.Shared.Models;
using Microsoft.EntityFrameworkCore;

namespace EnglishMaster.Server.Services;

public sealed class FlushCardQuestionService : IFlushCardQuestionService
{
    private readonly ILogger<FlushCardQuestionService> _logger;
    private readonly DB _db;
    public FlushCardQuestionService(DB db, ILogger<FlushCardQuestionService> logger)
    {
        _db = db;
        _logger = logger;
    }

    public IEnumerable<FlushCardResponseDto> GetFlushCardResponseDtos()
    {
        IEnumerable<MeaningOfWord> meaningOfWords = _db.MeaningOfWords
                                                        .Include(a => a.PartOfSpeech)
                                                        .ToList();
        foreach (Word word in GetRandomWord(10))
        {
            KeyValuePair<string, List<string>> means = new KeyValuePair<string, List<string>>();
            foreach (var grouping in meaningOfWords.Where(a => a.WordId == word.Id)
                                                   .GroupBy(a => a.PartOfSpeech))
            {
                means = new KeyValuePair<string, List<string>>(
                        grouping.Key.InJapanese,
                        grouping.Select(a => a.Meaning).ToList());
            }
            yield return new FlushCardResponseDto(word.Word1, word.Id, means);
        }
    }

    private IEnumerable<Word> GetRandomWord(int numberOfWords)
    {
        IEnumerable<Word> originalWords = _db.Words.ToList();
        return originalWords.OrderBy(a => Guid.NewGuid()).Take(numberOfWords);
    }
}
