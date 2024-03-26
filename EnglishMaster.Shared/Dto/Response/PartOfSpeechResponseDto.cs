namespace EnglishMaster.Shared.Dto.Response
{
    public sealed class PartOfSpeechResponseDto
    {
        public PartOfSpeechResponseDto(long id, string name, string japaneseName)
        {
            Id = id;
            Name = name;
            JapaneseName = japaneseName;
        }
        public long Id { get; }
        public string Name { get; }
        public string JapaneseName { get; }
    }
}