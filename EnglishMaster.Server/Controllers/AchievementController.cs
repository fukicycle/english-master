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
        private readonly IAchievementService _achievementService;
        public AchievementController(IAchievementService achievementService)
        {
            _achievementService = achievementService;
        }

        [HttpGet]
        [Route("")]
        public IActionResult GetAchievements()
        {
            try
            {
                long userId = HttpContext.GetUserId();
                return Ok(_achievementService.GetAchievementResponseDtos(userId));
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex.Message);
            }
        }

        [HttpGet]
        [Route("car/week")]
        [Authorize]
        public IActionResult GetAchievementCARByWeek()
        {
            try
            {
                long userId = HttpContext.GetUserId();
                return Ok(_achievementService.GetAchievementGraphResponseDtosByWeek(userId));
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex.Message);
            }
        }

        [HttpGet]
        [Route("car/part-of-speech")]
        [Authorize]
        public IActionResult GetAchievementCARByPartOfSpeech()
        {
            try
            {
                long userId = HttpContext.GetUserId();
                return Ok(_achievementService.GetAchievementGraphResponseDtosByPartOfSpeech(userId));
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
                return Ok(_achievementService.GetTreeFarmData(userId, startDate));
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex.Message);
            }
        }
    }
}