
using EnglishMaster.Server.Services.Interfaces;
using EnglishMaster.Shared.Dto.Request;
using EnglishMaster.Shared.Dto.Response;
using Microsoft.AspNetCore.Identity.Data;
using Microsoft.AspNetCore.Mvc;

namespace EnglishMaster.Server.Controllers
{
    [Route("/api/v1/login")]
    public sealed class LoginController : ControllerBase
    {
        private readonly ILoginService _loginService;

        public LoginController(ILoginService loginService)
        {
            _loginService = loginService;
        }

        [Route("")]
        [HttpOptions]
        public IActionResult OptionsLogin() => Ok();

        [Route("")]
        [HttpPost]
        public IActionResult PostLogin([FromBody] LoginRequestDto loginRequestDto)
        {
            try
            {
                LoginResponseDto loginResponseDto = _loginService.Login(loginRequestDto.Email, loginRequestDto.Password);
                if (loginResponseDto.Token == string.Empty)
                {
                    return NotFound();
                }
                return Ok(loginResponseDto);
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex.Message);
            }
        }
    }
}