using EnglishMaster.Server.Services.Interfaces;
using EnglishMaster.Shared;
using EnglishMaster.Shared.Dto.Request;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace EnglishMaster.Server.Controllers
{
    [Authorize]
    [Route(ApiEndPoint.RESULT)]
    public sealed class ResultController : ControllerBase
    {
        private readonly IResultService _resultService;
        private readonly ILoginService _loginService;
        public ResultController(IResultService resultService, ILoginService loginService)
        {
            _resultService = resultService;
            _loginService = loginService;

        }

        [HttpGet]
        [Route("")]
        public IActionResult GetResults()
        {
            try
            {
                string email = _loginService.GetValueFromClaims(HttpContext.User.Claims, "email");
                return Ok(_resultService.GetResultResponseDtosByEmail(email));
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex.Message);
            }
        }

        [HttpPost]
        [Route("")]
        public IActionResult PostResults([FromBody] IEnumerable<ResultRequestDto> resultRequestDtos)
        {
            try
            {
                string email = _loginService.GetValueFromClaims(HttpContext.User.Claims, "email");
                return Ok(_resultService.RegisterResult(email, resultRequestDtos));
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex.Message);
            }
        }
    }
}
