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
        public ResultController(IResultService resultService)
        {
            _resultService = resultService;

        }

        [HttpGet]
        [Route("")]
        public IActionResult GetResults(int count)
        {
            try
            {
                long userId = HttpContext.GetUserId();
                return Ok(_resultService.GetResultResponseDtos(userId, count));
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
                long userId = HttpContext.GetUserId();
                return Ok(_resultService.RegisterResult(userId, resultRequestDtos));
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex.Message);
            }
        }
    }
}
