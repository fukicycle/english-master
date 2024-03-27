

using EnglishMaster.Shared;
using EnglishMaster.Shared.Dto.Response;
using Microsoft.AspNetCore.Components;
using Newtonsoft.Json;

namespace EnglishMaster.Client.Pages
{
    public partial class Dictionary : PageBase
    {
        private List<DictionaryWordResponseDto> _dictionaries = new List<DictionaryWordResponseDto>();
        private List<DictionaryWordResponseDto> _originalDictionaries = new List<DictionaryWordResponseDto>();
        protected override async Task OnInitializedAsync()
        {
            try
            {
                StateContainer.IsLoading = true;
                HttpResponseResult dictionaryResult = await HttpClientService.SendAsync(HttpMethod.Get, ApiEndPoint.DICTIONARY);
                if (dictionaryResult.StatusCode != System.Net.HttpStatusCode.OK)
                {
                    throw new Exception(dictionaryResult.Message);
                }
                List<DictionaryWordResponseDto>? dictionaries = JsonConvert.DeserializeObject<List<DictionaryWordResponseDto>>(dictionaryResult.Json);
                if (dictionaries == null)
                {
                    throw new Exception($"Can not deserialized.{nameof(List<DictionaryWordResponseDto>)}");
                }
                _originalDictionaries = dictionaries;
                _dictionaries.AddRange(_originalDictionaries);
            }
            catch (Exception ex)
            {
                StateContainer.Message = ex.Message;
            }
            finally
            {
                StateContainer.IsLoading = false;
            }
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
            _dictionaries = Filter(_originalDictionaries, value);
        }

        private List<DictionaryWordResponseDto> Filter(List<DictionaryWordResponseDto> items, string searchString)
        {
            if (!string.IsNullOrEmpty(searchString)) return Filter(items.Where(a => a.Word.ToLower().Contains(searchString.ToLower())).ToList(), "");
            return items.OrderBy(a => a.Word).ToList();
        }
    }
}
