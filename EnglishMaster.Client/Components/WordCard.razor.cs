using Microsoft.AspNetCore.Components;

namespace EnglishMaster.Client.Components
{
    public partial class WordCard
    {
        [Parameter]
        public long Id { get; set; }
        public string _word = string.Empty;
        public string _description = string.Empty;

        protected override void OnInitialized()
        {
            _word = "Sample word";
            _description = "動詞,名刺";
        }
    }
}
