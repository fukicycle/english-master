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
        public UserController(IUserService userService)
        {
            _userService = userService;
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
                long userId = HttpContext.GetUserId();
                return Ok(_userService.GetUserResponseDto(userId));
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex.Message);
            }
        }
    }
}
