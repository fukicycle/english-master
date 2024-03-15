namespace EnglishMaster.Shared;

public sealed class LevelResponseDto
{
    public LevelResponseDto(long id, string name)
    {
        Id = id;
        Name = name;
    }
    public long Id { get; }
    public string Name { get; }
}
