using EnglishMaster.Server.Controllers;
using EnglishMaster.Server.Services.Interfaces;
using EnglishMaster.Shared;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace EnglishMaster.Server;

[Route(ApiEndPoint.FLUSH_QUESTION)]
public sealed class FlushCardQuestionController : ControllerBase
{
    private readonly ILogger<FlushCardQuestionController> _logger;
    private readonly IFlushCardQuestionService _flushCardQuestionService;
    public FlushCardQuestionController(IFlushCardQuestionService flushCardQuestionService, ILogger<FlushCardQuestionController> logger)
    {
        _flushCardQuestionService = flushCardQuestionService;
        _logger = logger;
    }

    [HttpGet()]
    [Authorize, AllowAnonymous]
    public IActionResult GetQuestions()
    {
        try
        {
            if (HttpContext.User.Identity?.IsAuthenticated == true)
            {
                return Ok(_flushCardQuestionService.GetFlushCardResponseDtos(HttpContext.GetUserId()));
            }
            return Ok(_flushCardQuestionService.GetFlushCardResponseDtos());
        }
        catch (Exception ex)
        {
            return StatusCode(500, ex.Message);
        }
    }
}
