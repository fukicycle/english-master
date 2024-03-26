namespace EnglishMaster.Shared.Dto.Response
{
    public sealed class DictionaryMeaningResponseDto
    {
        public DictionaryMeaningResponseDto(string meaning, string partOfSpeech)
        {
            Meaning = meaning;
            PartOfSpeech = partOfSpeech;
        }
        public string Meaning { get; }
        public string PartOfSpeech { get; }
    }
}
