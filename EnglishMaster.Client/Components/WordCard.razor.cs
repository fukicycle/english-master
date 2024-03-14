using EnglishMaster.Shared.Dto.Response;
using Microsoft.AspNetCore.Components;

namespace EnglishMaster.Client.Components
{
    public partial class WordCard
    {
        [Parameter]
        public DictionaryWordResponseDto Dictionary { get; set; } = null!;

        private string _description = string.Empty;

        protected override void OnParametersSet()
        {
            if (Dictionary == null)
            {
                throw new ArgumentNullException(nameof(Dictionary));
            }
            _description = $"{Dictionary.PartOfSpeech}:{Dictionary.Meaning}";
        }
    }
}
