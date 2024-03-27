using EnglishMaster.Server.Services.Interfaces;
using EnglishMaster.Shared;
using Microsoft.AspNetCore.Mvc;

namespace EnglishMaster.Server.Controllers
{
    [Route(ApiEndPoint.PART_OF_SPEECH)]
    public sealed class PartOfSpeechController : ControllerBase
    {
        private readonly IPartOfSpeechService _partOfSpeechService;
        public PartOfSpeechController(IPartOfSpeechService partOfSpeechService)
        {
            _partOfSpeechService = partOfSpeechService;
        }

        [HttpGet]
        [Route("")]
        public IActionResult GetPartOfSpeeches()
        {
            try
            {
                return Ok(_partOfSpeechService.GetPartOfSpeechResponseDtos());
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex.Message);
            }
        }
    }
}