using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Principal;
using System.Text;
using System.Threading.Tasks;

namespace EnglishMaster.Shared.Dto.Response
{
    public sealed class DictionaryWordResponseDto
    {
        public DictionaryWordResponseDto(long id, string word, IEnumerable<DictionaryMeaningResponseDto> meanings)
        {
            Id = id;
            Word = word;
            Meanings = meanings;
        }
        public long Id { get; }
        public string Word { get; }

        public IEnumerable<DictionaryMeaningResponseDto> Meanings { get; }
    }
}
