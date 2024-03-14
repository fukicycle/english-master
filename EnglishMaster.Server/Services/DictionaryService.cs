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

        public IList<DictionaryWordResponseDto> GetDictionaryResponseDtos()
        {
            IList<DictionaryWordResponseDto> dictionaryWordResponseDtos = new List<DictionaryWordResponseDto>();
            IList<MeaningOfWord> meanings = _db.MeaningOfWords
                .Include(a => a.Word).Include(a => a.PartOfSpeech)
                .ToList();
            foreach (var grouping in meanings.GroupBy(a => new
            {
                a.Word.Id,
                a.Word.Word1
            }))
            {
                IEnumerable<DictionaryMeaningResponseDto> dictionaryMeaningResponseDtos = grouping.Select(a => new DictionaryMeaningResponseDto(a.Meaning, a.PartOfSpeech.InJapanese));
                DictionaryWordResponseDto dictionaryWordResponseDto = new DictionaryWordResponseDto(grouping.Key.Id, grouping.Key.Word1, dictionaryMeaningResponseDtos);
                dictionaryWordResponseDtos.Add(dictionaryWordResponseDto);
            }
            return dictionaryWordResponseDtos.OrderBy(a => a.Word).ToList();
        }
    }
}
