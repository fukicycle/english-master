
using EnglishMaster.Server.Security;
using EnglishMaster.Server.Security.Service;
using EnglishMaster.Server.Services.Interfaces;
using EnglishMaster.Shared;
using EnglishMaster.Shared.Dto.Request;
using EnglishMaster.Shared.Dto.Response;
using Microsoft.AspNetCore.Identity.Data;
using Microsoft.AspNetCore.Mvc;

namespace EnglishMaster.Server.Controllers
{
    [Route(ApiEndPoint.LOGIN)]
    public sealed class LoginController : ControllerBase
    {
        private readonly IAccessTokenAuthenticationService _accessTokenAuthenticationService;

        public LoginController(IAccessTokenAuthenticationService accessTokenAuthenticationService)
        {
            _accessTokenAuthenticationService = accessTokenAuthenticationService;
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
                var result = _accessTokenAuthenticationService
                                .Authenticate(loginRequestDto.Email, loginRequestDto.Password);
                if (result.ResultCode == AccessTokenAuthenticationResultCode.INVALID_CREDENTIAL)
                {
                    return Unauthorized();
                }
                if (result.Token == null)
                {
                    return StatusCode(500, "Token generation failed.");
                }
                LoginResponseDto loginResponseDto = new LoginResponseDto(result.Token);
                return Ok(loginResponseDto);
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex.Message);
            }
        }
    }
}