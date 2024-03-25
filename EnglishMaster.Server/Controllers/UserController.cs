using EnglishMaster.Server.Services.Interfaces;
using EnglishMaster.Shared.Dto.Request;
using Microsoft.AspNetCore.Mvc;

namespace EnglishMaster.Server.Controllers
{
    [Route("/api/v1/users")]
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
                _userService.Register(userReqestDto.Username, userReqestDto.Password, userReqestDto.FirstName, userReqestDto.LastName);
                return Created();
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex.Message);
            }
        }
    }
}
