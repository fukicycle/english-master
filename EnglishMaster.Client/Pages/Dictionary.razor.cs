

using EnglishMaster.Shared.Dto.Response;
using Newtonsoft.Json;

namespace EnglishMaster.Client.Pages
{
    public partial class Dictionary : PageBase
    {
        private IList<DictionaryWordResponseDto> _dictionaries = new List<DictionaryWordResponseDto>();
        protected override async Task OnInitializedAsync()
        {
            await RunActionWithLoading(async () =>
              {
                  HttpResponseResult dictionaryResult = await HttpClientService.SendAsync(HttpMethod.Get, "/api/v1/dictionaries");
                  if (dictionaryResult.StatusCode != System.Net.HttpStatusCode.OK)
                  {
                      throw new Exception(dictionaryResult.Message);
                  }
                  IList<DictionaryWordResponseDto>? dictionaries = JsonConvert.DeserializeObject<IList<DictionaryWordResponseDto>>(dictionaryResult.Json);
                  if (dictionaries == null)
                  {
                      throw new Exception($"Can not deserialized.{nameof(IList<DictionaryWordResponseDto>)}");
                  }
                  _dictionaries = dictionaries;
              });
        }
    }
}
