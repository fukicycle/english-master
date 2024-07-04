namespace EnglishMaster.Shared.Dto.Response;

public sealed class FlushCardResponseDto
{
    public FlushCardResponseDto(string word, long wordId, Dictionary<string, List<string>> means)
    {
        Word = word;
        WordId = wordId;
        Means = means;
    }
    public string Word { get; }
    public long WordId { get; }
    public Dictionary<string, List<string>> Means { get; }
}
