using EnglishMaster.Server.Services;
using EnglishMaster.Shared;
using EnglishMaster.Shared.Dto.Request;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace EnglishMaster.Server.Controllers;

[Authorize]
[Route(ApiEndPoint.FLUSH_RESULT)]
public sealed class FlushCardResultController : ControllerBase
{
    private readonly ILogger<FlushCardResultController> _logger;
    private readonly FlushCardResultService _flushCardResultService;
    public FlushCardResultController(ILogger<FlushCardResultController> logger, FlushCardResultService flushCardResultService)
    {
        _logger = logger;
        _flushCardResultService = flushCardResultService;
    }

    [HttpPost]
    public IActionResult PostResult([FromBody] FlushCardResultRequestDto flushCardResultRequestDto)
    {
        try
        {
            _flushCardResultService.Register(flushCardResultRequestDto, HttpContext.GetUserId());
            return Ok();
        }
        catch (Exception ex)
        {
            return StatusCode(500, ex.Message);
        }
    }
}
