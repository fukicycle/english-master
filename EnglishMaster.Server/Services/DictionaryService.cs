using EnglishMaster.Server.Services.Interfaces;
using EnglishMaster.Shared.Dto.Response;
using EnglishMaster.Shared.Models;
using Microsoft.EntityFrameworkCore;

namespace EnglishMaster.Server.Services
{
    public sealed class DictionaryService : IDictionaryService
    {
        private readonly DB _db;
        private readonly ILogger<DictionaryService> _logger;

        public DictionaryService(DB db, ILogger<DictionaryService> logger)
        {
            _db = db;
            _logger = logger;
        }

        public IList<DictionaryResponseDto> GetDictionaryResponseDtos()
        {
            return _db.MeaningOfWords
                .Include(a => a.Word).Include(a => a.PartOfSpeech)
                .ToList()
                .Select(a => new DictionaryResponseDto(a.WordId, a.Word.Word1, a.Meaning, a.PartOfSpeech.InJapanese))
                .OrderBy(a => a.Word)
                .ToList();
        }
    }
}
