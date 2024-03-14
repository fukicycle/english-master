

using EnglishMaster.Shared.Dto.Response;
using Newtonsoft.Json;

namespace EnglishMaster.Client.Pages
{
    public partial class Dictionary : PageBase
    {
        private List<DictionaryWordResponseDto> _dictionaries = new List<DictionaryWordResponseDto>();
        protected override async Task OnInitializedAsync()
        {
            try
            {
                StateContainer.IsLoading = true;
                HttpResponseResult dictionaryResult = await HttpClientService.SendAsync(HttpMethod.Get, "/api/v1/dictionaries");
                if (dictionaryResult.StatusCode != System.Net.HttpStatusCode.OK)
                {
                    throw new Exception(dictionaryResult.Message);
                }
                List<DictionaryWordResponseDto>? dictionaries = JsonConvert.DeserializeObject<List<DictionaryWordResponseDto>>(dictionaryResult.Json);
                if (dictionaries == null)
                {
                    throw new Exception($"Can not deserialized.{nameof(List<DictionaryWordResponseDto>)}");
                }
                _dictionaries = dictionaries;
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
    }
}
