namespace EnglishMaster.Shared;

public class QuestionResponseDto
{
    public QuestionResponseDto(long id,string word,long partOfSpeechId,long levelId,IEnumerable<AnswerResponseDto> answerResponseDtos)
    {
        Id = id;
        Word = word;
        PartOfSpeechId = partOfSpeechId;
        LevelId = levelId;
        AnswerResponseDtos = answerResponseDtos;
    }
    public long Id { get; }
    public string Word { get;  }
    public long PartOfSpeechId { get; }
    public long LevelId { get; }
    public IEnumerable<AnswerResponseDto> AnswerResponseDtos { get; }
}
