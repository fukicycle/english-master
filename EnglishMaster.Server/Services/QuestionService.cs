using EnglishMaster.Server.Services.Interfaces;
using EnglishMaster.Shared.Dto.Response;
using EnglishMaster.Shared.Models;
using Microsoft.EntityFrameworkCore;

namespace EnglishMaster.Server.Services
{
    public sealed class QuestionService : IQuestionService
    {
        private readonly DB _db;
        private readonly ILogger<QuestionService> _logger;

        public QuestionService(DB db, ILogger<QuestionService> logger)
        {
            _db = db;
            _logger = logger;
        }
        public IList<QuestionResponseDto> GetQuestionResponseDtos(long partOfSpeechId = 0, long levelId = 0, int numberOfQuestions = 10)
        {
            IEnumerable<MeaningOfWord> originals = _db.MeaningOfWords.Include(a => a.Word).ToList();
            IEnumerable<MeaningOfWord> meaningOfWords = Filter(originals, partOfSpeechId, levelId).OrderByDescending(a => Guid.NewGuid());
            IList<QuestionResponseDto> questionResponseDtos = new List<QuestionResponseDto>();
            int number = 1;
            foreach (MeaningOfWord meaningOfWord in meaningOfWords.Take(numberOfQuestions))
            {
                IEnumerable<MeaningOfWord> answerTargets = Filter(originals, meaningOfWord.PartOfSpeechId, 0);
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
            IEnumerable<MeaningOfWord> randoms = answerTargets.Where(a => a.WordId != questionWord.WordId).OrderByDescending(a => Guid.NewGuid()).Take(numberOfAnswer - 1);
            randoms = randoms.Append(questionWord).OrderByDescending(a => Guid.NewGuid());
            return randoms;
        }

        private IEnumerable<MeaningOfWord> Filter(IEnumerable<MeaningOfWord> items, long partOfSpeechId, long levelId)
        {
            if (partOfSpeechId != 0) return Filter(items.Where(a => a.PartOfSpeechId == partOfSpeechId), 0, levelId);
            if (levelId != 0) return Filter(items.Where(a => a.LevelId == levelId), partOfSpeechId, 0);
            return items;
        }

        public IList<QuestionResponseDto> GetQuestionResponseDtosWithCredentials(string email, long partOfSpeechId = 0, long levelId = 0, int numberOfQuestions = 10)
        {
            IEnumerable<MeaningOfWord> originals = _db.MeaningOfWords.Include(a => a.Word).ToList();
            IEnumerable<MeaningOfWord> meaningOfWords = Filter(originals, partOfSpeechId, levelId).OrderByDescending(a => Guid.NewGuid());
            User user = _db.Users.Single(a => a.Username == email);
            IList<MeaningOfWordLearningHistory> histories = _db.MeaningOfWordLearningHistories.Where(a => a.UserId == user.Id && a.IsDone).ToList();
            IEnumerable<MeaningOfWord> questions = GetQuestions(meaningOfWords, histories);
            int number = 1;
            IList<QuestionResponseDto> questionResponseDtos = new List<QuestionResponseDto>();
            foreach (MeaningOfWord meaningOfWord in questions.Take(numberOfQuestions))
            {
                IEnumerable<MeaningOfWord> answerTargets = Filter(originals, meaningOfWord.PartOfSpeechId, 0);
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

        private IEnumerable<MeaningOfWord> GetQuestions(IEnumerable<MeaningOfWord> meaningOfWords, IList<MeaningOfWordLearningHistory> histories, int numberOfAnswer = 1)
        {
            IEnumerable<long> historyIds = histories.GroupBy(a => a.QuestionMeaningOfWordId).Where(a => a.Count() >= numberOfAnswer).Select(a => a.Key);
            IEnumerable<MeaningOfWord> questions = meaningOfWords.Where(a => !historyIds.Contains(a.Id));
            _logger.LogInformation($"Run get questions:{numberOfAnswer}");
            if (numberOfAnswer >= 1000)
            {
                //すべての単語の勉強を1000回以上頑張ってしまうとこのアプリはリセットしない限り問題を提出できなくなる。
                return questions;
            }
            if (questions.Count() <= 0)
            {
                return GetQuestions(meaningOfWords, histories, numberOfAnswer + 1);
            }
            return questions;
        }
    }
}