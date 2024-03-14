

using EnglishMaster.Shared.Dto.Response;
using Newtonsoft.Json;

namespace EnglishMaster.Client.Pages
{
    public partial class Dictionary : PageBase
    {
        private IList<DictionaryResponseDto> _dictionaries = new List<DictionaryResponseDto>();
        protected override async Task OnInitializedAsync()
        {
            await RunActionWithLoading(async () =>
              {
                  HttpResponseResult dictionaryResult = await HttpClientService.SendAsync(HttpMethod.Get, "/api/v1/dictionaries");
                  if (dictionaryResult.StatusCode != System.Net.HttpStatusCode.OK)
                  {
                      throw new Exception(dictionaryResult.Message);
                  }
                  IList<DictionaryResponseDto>? dictionaries = JsonConvert.DeserializeObject<IList<DictionaryResponseDto>>(dictionaryResult.Json);
                  if (dictionaries == null)
                  {
                      throw new Exception($"Can not deserialized.{nameof(IList<DictionaryResponseDto>)}");
                  }
                  _dictionaries = dictionaries;
              });
        }
    }
}
