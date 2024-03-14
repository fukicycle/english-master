using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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
