using EnglishMaster.Server.Services.Interfaces;
using EnglishMaster.Shared;
using EnglishMaster.Shared.Dto.Response;
using EnglishMaster.Shared.Models;
using Microsoft.AspNetCore.Identity;
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
            Dictionary<string, List<string>> means = new Dictionary<string, List<string>>();
            foreach (var grouping in meaningOfWords.Where(a => a.WordId == word.Id)
                                                   .GroupBy(a => a.PartOfSpeech))
            {
                means.Add(grouping.Key.InJapanese, grouping.Select(a => a.Meaning).ToList());
            }
            yield return new FlushCardResponseDto(word.Word1, word.Id, means);
        }
    }

    public IEnumerable<FlushCardResponseDto> GetFlushCardResponseDtos(long userId)
    {
        IEnumerable<MeaningOfWord> meaningOfWords = _db.MeaningOfWords
                                                .Include(a => a.PartOfSpeech)
                                                .ToList();
        foreach (Word word in GetRandomWord(10, userId))
        {
            Dictionary<string, List<string>> means = new Dictionary<string, List<string>>();
            foreach (var grouping in meaningOfWords.Where(a => a.WordId == word.Id)
                                                   .GroupBy(a => a.PartOfSpeech))
            {
                means.Add(grouping.Key.InJapanese, grouping.Select(a => a.Meaning).ToList());
            }
            yield return new FlushCardResponseDto(word.Word1, word.Id, means);
        }
    }

    private bool IsNotCorrect(Word word, long userId)
    {
        return word.MeaningOfWords.All(a =>
        {
            MeaningOfWordLearningHistory? meaningOfWordLearningHistory =
                                                    a.MeaningOfWordLearningHistories
                                                            .Where(a => a.UserId == userId && a.ModeId == StudyMode.Flush)
                                                            .OrderByDescending(b => b.Date)
                                                            .FirstOrDefault();
            if (meaningOfWordLearningHistory == null)
            {
                return true;
            }
            return !meaningOfWordLearningHistory.IsCorrect;
        });
    }

    private IEnumerable<Word> GetRandomWord(int numberOfWords)
    {
        IEnumerable<Word> originalWords = _db.Words.ToList();
        return originalWords.OrderBy(a => Guid.NewGuid()).Take(numberOfWords);
    }

    private IEnumerable<Word> GetRandomWord(int numberOfWords, long userId)
    {
        List<Word> words = new List<Word>();
        IEnumerable<Word> originalWords =
                                    _db.Words.Include(a => a.MeaningOfWords)
                                             .ThenInclude(a => a.MeaningOfWordLearningHistories)
                                             .ToList()
                                             .Where(a => IsNotCorrect(a, userId));
        int remain = numberOfWords - originalWords.Count();
        if (remain > 0)
        {
            words.AddRange(GetRandomWord(remain));
            words.AddRange(originalWords);
        }
        else
        {
            words.AddRange(originalWords.OrderBy(a => Guid.NewGuid()).Take(numberOfWords));
        }
        return words;
    }
}
