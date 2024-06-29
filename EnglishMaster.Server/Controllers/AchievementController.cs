using EnglishMaster.Server.Security.Service;
using EnglishMaster.Server.Services.Interfaces;
using EnglishMaster.Shared;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace EnglishMaster.Server.Controllers
{
    [Authorize]
    [Route(ApiEndPoint.ACHIEVEMENT)]
    public sealed class AchievementController : ControllerBase
    {
        private readonly IAchievementService _choiceAchievementService;
        private readonly IAchievementService _flushAchievementService;
        public AchievementController([FromKeyedServices("Choice")] IAchievementService choiceAchievementService, [FromKeyedServices("Flush")] IAchievementService flushAchievementService)
        {
            _choiceAchievementService = choiceAchievementService;
            _flushAchievementService = flushAchievementService;
        }

        [HttpGet("{mode}")]
        [Route("")]
        public IActionResult GetAchievements(long mode)
        {
            try
            {
                long userId = HttpContext.GetUserId();
                switch (mode)
                {
                    case 1:
                        return Ok(_flushAchievementService.GetAchievementResponseDtos(userId));
                    case 2:
                        return Ok(_choiceAchievementService.GetAchievementResponseDtos(userId));
                }
                return BadRequest();
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex.Message);
            }
        }

        [HttpGet()]
        [Route("car/week/{mode}")]
        [Authorize]
        public IActionResult GetAchievementCARByWeek(long mode)
        {
            try
            {
                long userId = HttpContext.GetUserId();
                switch (mode)
                {
                    case 1:
                        return Ok(_flushAchievementService.GetAchievementGraphResponseDtosByWeek(userId));
                    case 2:
                        return Ok(_choiceAchievementService.GetAchievementGraphResponseDtosByWeek(userId));
                }
                return BadRequest();
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex.Message);
            }
        }

        [HttpGet]
        [Route("car/part-of-speech/{mode}")]
        [Authorize]
        public IActionResult GetAchievementCARByPartOfSpeech(long mode)
        {
            try
            {
                long userId = HttpContext.GetUserId();
                switch (mode)
                {
                    case 1:
                        return Ok(_flushAchievementService.GetAchievementGraphResponseDtosByPartOfSpeech(userId));
                    case 2:
                        return Ok(_choiceAchievementService.GetAchievementGraphResponseDtosByPartOfSpeech(userId));
                }
                return BadRequest();
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex.Message);
            }
        }

        [HttpGet]
        [Route("tree/farm/data")]
        [Authorize]
        public IActionResult GetTreeFarmData(DateTime startDate)
        {
            try
            {
                long userId = HttpContext.GetUserId();
                return Ok(_choiceAchievementService.GetTreeFarmData(userId, startDate));
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex.Message);
            }
        }
    }
}