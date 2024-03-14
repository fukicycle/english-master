using EnglishMaster.Shared.Dto.Response;

namespace EnglishMaster.Server.Services.Interfaces
{
    public interface IDictionaryService
    {
        IList<DictionaryResponseDto> GetDictionaryResponseDtos();
    }
}
