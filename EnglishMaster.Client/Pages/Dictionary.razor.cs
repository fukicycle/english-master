using EnglishMaster.Shared.Dto.Response;
using Microsoft.AspNetCore.Components;

namespace EnglishMaster.Client.Pages
{
    public partial class Dictionary
    {
        private List<DictionaryWordResponseDto> _dictionaries = new List<DictionaryWordResponseDto>();
        protected override async Task OnInitializedAsync()
        {
            _dictionaries = await ExecuteAsync(DictionaryClientService.GetDictionariesAsync, true);
        }

        private void SearchInputOnChanged(ChangeEventArgs e)
        {
            if (e.Value == null)
            {
                return;
            }
            string? value = e.Value.ToString();
            if (value == null)
            {
                return;
            }
            _dictionaries = DictionaryClientService.Filter(value);
        }
    }
}
