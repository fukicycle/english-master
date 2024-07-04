using EnglishMaster.Server.Services.Interfaces;
using EnglishMaster.Shared;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace EnglishMaster.Server.Controllers
{
    [Route(ApiEndPoint.QUESTION)]
    public sealed class QuestionController : ControllerBase
    {
        private readonly IQuestionService _questionService;
        public QuestionController(IQuestionService questionService)
        {
            _questionService = questionService;
        }

        [HttpGet]
        [Route("part-of-speeches/{partOfSpeechId}/levels/{levelId}")]
        [Authorize, AllowAnonymous]
        public IActionResult GetQuestions(long partOfSpeechId, long levelId)
        {
            try
            {
                //Fixed size number of quetion.
                if (HttpContext.User.Identity?.IsAuthenticated == true)
                {
                    long userId = HttpContext.GetUserId();
                    return Ok(_questionService.GetQuestionResponseDtos(userId, partOfSpeechId, levelId, 10));
                }
                return Ok(_questionService.GetQuestionResponseDtos(partOfSpeechId, levelId, 10));
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex.Message);
            }
        }
    }
}