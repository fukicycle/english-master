using EnglishMaster.Server.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace EnglishMaster.Server.Controllers;

[Route("/api/v1/questions")]
public sealed class QuestionController : ControllerBase
{
    private readonly IQuestionService _questionService;
    public QuestionController(IQuestionService questionService)
    {
        _questionService = questionService;
    }

    [HttpGet]
    [Route("part-of-speeches/{partSpeechId}/levels/{levelId}")]
    public IActionResult GetQuestions(long partSpeechId, long levelId)
    {
        try
        {
            return Ok(_questionService.GetQuestionResponseDtos(partSpeechId, levelId));
        }
        catch (Exception ex)
        {
            return StatusCode(500, ex.Message);
        }
    }

    [HttpGet]
    [Route("part-of-speeches/{partSpeechId}/levels/{levelId}/number-of-question/{numberOfQuestion}")]
    public IActionResult GetQuestionsWithNumberOfQuestions(long partSpeechId, long levelId, int numberOfQuestion)
    {
        try
        {
            return Ok(_questionService.GetQuestionResponseDtos(partSpeechId, levelId, numberOfQuestion));
        }
        catch (Exception ex)
        {
            return StatusCode(500, ex.Message);
        }
    }
}
