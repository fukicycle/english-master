using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Principal;
using System.Text;
using System.Threading.Tasks;

namespace EnglishMaster.Shared.Dto.Response
{
    public sealed class DictionaryResponseDto
    {
        public DictionaryResponseDto(long id, string word, string meaning, string partOfSpeech)
        {
            Id = id;
            Word = word;
            Meaning = meaning;
            PartOfSpeech = partOfSpeech;
        }
        public long Id { get; }
        public string Word { get; }
        public string Meaning { get; }
        public string PartOfSpeech { get; }
    }
}
