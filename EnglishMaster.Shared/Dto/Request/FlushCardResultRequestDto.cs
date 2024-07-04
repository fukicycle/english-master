namespace EnglishMaster.Shared.Dto.Request;

public sealed class FlushCardResultRequestDto
{
    public FlushCardResultRequestDto(long wordId, bool isCorrect)
    {
        WordId = wordId;
        IsCorrect = isCorrect;
    }
    public long WordId { get; }
    public bool IsCorrect { get; }
}
