namespace EnglishMaster.Shared.Dto.Response;

public sealed class FlushCardResponseDto
{
    public FlushCardResponseDto(string word, long wordId)
    {
        Word = word;
        WordId = wordId;
    }
    public string Word { get; }
    public long WordId { get; }
}
