using EnglishMaster.Shared;
using EnglishMaster.Shared.Models;
using Microsoft.EntityFrameworkCore;

namespace EnglishMaster.Server;

public sealed class QuestionService : IQuestionService
{
    private readonly DB _db;
    private readonly ILogger<QuestionService> _logger;

    public QuestionService(DB db, ILogger<QuestionService> logger)
    {
        _db = db;
        _logger = logger;
    }
    public IList<QuestionResponseDto> GetQuestionResponseDtos(long partOfSpeechId, long levelId = 0, int numberOfQuestions = 50)
    {
        IEnumerable<MeaningOfWord> originals = _db.MeaningOfWords.Include(a => a.Word).ToList();
        IEnumerable<MeaningOfWord> meaningOfWords = Filter(originals, partOfSpeechId, levelId);
        IList<QuestionResponseDto> questionResponseDtos = new List<QuestionResponseDto>();
        int number = 1;
        foreach (MeaningOfWord meaningOfWord in meaningOfWords.Take(numberOfQuestions))
        {
            IEnumerable<MeaningOfWord> answerTargets = Filter(originals, meaningOfWord.PartOfSpeechId, meaningOfWord.LevelId);
            IEnumerable<MeaningOfWord> randomAnswers = GetRandomChoices(answerTargets, meaningOfWord);
            IList<AnswerResponseDto> answerResponseDtos = new List<AnswerResponseDto>();
            foreach (MeaningOfWord answer in randomAnswers)
            {
                AnswerResponseDto answerResponseDto = new AnswerResponseDto(answer.Id, answer.Meaning);
                answerResponseDtos.Add(answerResponseDto);
            }
            QuestionResponseDto questionResponseDto = new QuestionResponseDto(number, meaningOfWord.Id, meaningOfWord.Word.Word1, meaningOfWord.PartOfSpeechId, meaningOfWord.LevelId, answerResponseDtos);
            questionResponseDtos.Add(questionResponseDto);
            number++;
        }
        return questionResponseDtos;
    }

    private IEnumerable<MeaningOfWord> GetRandomChoices(IEnumerable<MeaningOfWord> answerTargets, MeaningOfWord questionWord, int numberOfAnswer = 4)
    {
        IEnumerable<MeaningOfWord> randoms = answerTargets.OrderByDescending(a => Guid.NewGuid()).Take(numberOfAnswer - 1);
        randoms = randoms.Append(questionWord).OrderByDescending(a => Guid.NewGuid());
        return randoms;
    }

    private IEnumerable<MeaningOfWord> Filter(IEnumerable<MeaningOfWord> items, long partOfSpeechId, long levelId)
    {
        if (partOfSpeechId != 0) return Filter(items.Where(a => a.PartOfSpeechId == partOfSpeechId), 0, levelId);
        if (levelId != 0) return Filter(items.Where(a => a.LevelId == levelId), levelId, 0);
        return items;
    }
}
