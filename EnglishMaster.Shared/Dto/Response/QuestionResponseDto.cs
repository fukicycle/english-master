namespace EnglishMaster.Shared.Dto.Response;

public class QuestionResponseDto
{
    public QuestionResponseDto(int number, long meaningOfWordId, string word, long partOfSpeechId, long levelId, IEnumerable<AnswerResponseDto> answerResponseDtos)
    {
        Number = number;
        MeaningOfWordId = meaningOfWordId;
        Word = word;
        PartOfSpeechId = partOfSpeechId;
        LevelId = levelId;
        AnswerResponseDtos = answerResponseDtos;
    }
    public int Number { get; }
    public long MeaningOfWordId { get; }
    public string Word { get; }
    public long PartOfSpeechId { get; }
    public long LevelId { get; }
    public IEnumerable<AnswerResponseDto> AnswerResponseDtos { get; }
}
