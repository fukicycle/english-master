using EnglishMaster.Server.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace EnglishMaster.Server.Controllers
{
    [Route("/api/v1/dictionaries")]
    public sealed class DictionaryController : ControllerBase
    {
        private readonly IDictionaryService _dictionaryService;
        public DictionaryController(IDictionaryService dictionaryService)
        {
            _dictionaryService = dictionaryService;
        }

        [HttpGet]
        public IActionResult GetDictionaries()
        {
            try
            {
                return Ok(_dictionaryService.GetDictionaryResponseDtos());
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex.Message);
            }
        }
    }
}
