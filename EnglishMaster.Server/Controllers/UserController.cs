using EnglishMaster.Server.Services.Interfaces;
using EnglishMaster.Shared;
using EnglishMaster.Shared.Dto.Request;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace EnglishMaster.Server.Controllers
{
    [Route(ApiEndPoint.USER)]
    public sealed class UserController : ControllerBase
    {
        private readonly IUserService _userService;
        private readonly ILoginService _loginService;
        public UserController(IUserService userService, ILoginService loginService)
        {
            _userService = userService;
            _loginService = loginService;
        }

        [HttpPost]
        [Route("")]
        public IActionResult PostUser([FromBody] UserReqestDto userReqestDto)
        {
            try
            {
                _userService.Register(userReqestDto.Username, userReqestDto.Password, userReqestDto.FirstName, userReqestDto.LastName, userReqestDto.Nickname);
                return StatusCode(201);
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex.Message);
            }
        }

        [HttpGet]
        [Route("")]
        [Authorize]
        public IActionResult GetUser()
        {
            try
            {
                string email = _loginService.GetValueFromClaims(HttpContext.User.Claims, "email");
                return Ok(_userService.GetUserResponseDtoByEmail(email));
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex.Message);
            }
        }
    }
}
