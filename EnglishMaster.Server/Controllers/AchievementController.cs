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
        private readonly ILoginService _loginService;
        public AchievementController(IAchievementService achievementService, ILoginService loginService)
        {
            _achievementService = achievementService;
            _loginService = loginService;
        }

        [HttpGet]
        [Route("")]
        public IActionResult GetAchievements()
        {
            try
            {
                string email = _loginService.GetValueFromClaims(HttpContext.User.Claims, "email");
                return Ok(_achievementService.GetAchievementResponseDtosByEmail(email));
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
                string email = _loginService.GetValueFromClaims(HttpContext.User.Claims, "email");
                return Ok(_achievementService.GetAchievementGraphResponseDtosByWeek(email));
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
                string email = _loginService.GetValueFromClaims(HttpContext.User.Claims, "email");
                return Ok(_achievementService.GetAchievementGraphResponseDtosByPartOfSpeech(email));
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex.Message);
            }
        }

        [HttpGet]
        [Route("tree/farm/data")]
        [Authorize]
        public IActionResult GetTreeFarmData()
        {
            try
            {
                string email = _loginService.GetValueFromClaims(HttpContext.User.Claims, "email");
                return Ok(_achievementService.GetTreeFarmData(email));
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex.Message);
            }
        }
    }
}