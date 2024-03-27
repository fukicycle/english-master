using EnglishMaster.Server.Services.Interfaces;
using EnglishMaster.Shared;
using Microsoft.AspNetCore.Mvc;

namespace EnglishMaster.Server.Controllers
{
    [Route(ApiEndPoint.LEVEL)]
    public sealed class LevelController : ControllerBase
    {
        private readonly ILevelService _levelService;
        public LevelController(ILevelService levelService)
        {
            _levelService = levelService;
        }

        [HttpGet]
        [Route("")]
        public IActionResult GetLevels()
        {
            try
            {
                return Ok(_levelService.GetLevelResponseDtos());
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex.Message);
            }
        }
    }
}