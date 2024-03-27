namespace EnglishMaster.Shared.Dto.Response
{
    public class AnswerResponseDto
    {
        public AnswerResponseDto(long wordId, string meaning)
        {
            WordId = wordId;
            Meaning = meaning;
        }
        public long WordId { get; }
        public string Meaning { get; }
    }
}