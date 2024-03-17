using EnglishMaster.Server.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace EnglishMaster.Server.Controllers;

[Route("/api/v1/part-of-speeches")]
public sealed class PartOfSpeechController : ControllerBase
{
    private readonly IPartOfSpeechService _partOfSpeechService;
    public PartOfSpeechController(IPartOfSpeechService partOfSpeechService)
    {
        _partOfSpeechService = partOfSpeechService;
    }

    [HttpGet]
    [Route("")]
    public IActionResult GetPartOfSpeeches()
    {
        try
        {
            return Ok(_partOfSpeechService.GetPartOfSpeechResponseDtos());
        }
        catch (Exception ex)
        {
            return StatusCode(500, ex.Message);
        }
    }
}
