using EnglishMaster.Server.Services.Interfaces;
using EnglishMaster.Shared.Dto.Response;
using EnglishMaster.Shared.Models;

namespace EnglishMaster.Server.Services
{
    public sealed class PartOfSpeechService : IPartOfSpeechService
    {
        private readonly DB _db;
        private readonly ILogger<PartOfSpeechService> _logger;

        public PartOfSpeechService(DB db, ILogger<PartOfSpeechService> logger)
        {
            _db = db;
            _logger = logger;
        }
        public IList<PartOfSpeechResponseDto> GetPartOfSpeechResponseDtos()
        {
            IEnumerable<PartOfSpeech> partOfSpeeches = _db.PartOfSpeeches.Where(a => a.MeaningOfWords.Count >= 50).ToList();
            IList<PartOfSpeechResponseDto> partOfSpeechResponseDtos = new List<PartOfSpeechResponseDto>();
            foreach (PartOfSpeech partOfSpeech in partOfSpeeches)
            {
                PartOfSpeechResponseDto partOfSpeechResponseDto = new PartOfSpeechResponseDto(partOfSpeech.Id, partOfSpeech.Name, partOfSpeech.InJapanese);
                partOfSpeechResponseDtos.Add(partOfSpeechResponseDto);
            }
            return partOfSpeechResponseDtos;
        }
    }
}