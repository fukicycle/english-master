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
    }
}