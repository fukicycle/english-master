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
        private readonly ILoginService _loginService;
        public QuestionController(IQuestionService questionService, ILoginService loginService)
        {
            _questionService = questionService;
            _loginService = loginService;
        }

        [HttpGet]
        [Route("part-of-speeches/{partOfSpeechId}/levels/{levelId}")]
        [Authorize, AllowAnonymous]
        public IActionResult GetQuestions(long partOfSpeechId, long levelId)
        {
            try
            {
                if (HttpContext.User.Identity?.IsAuthenticated == true)
                {
                    string email = _loginService.GetValueFromClaims(HttpContext.User.Claims, "email");
                    return Ok(_questionService.GetQuestionResponseDtosWithCredentials(email, partOfSpeechId, levelId, 0));
                }
                return Ok(_questionService.GetQuestionResponseDtos(partOfSpeechId, levelId));
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex.Message);
            }
        }

        [HttpGet]
        [Route("part-of-speeches/{partOfSpeechId}/levels/{levelId}/number-of-question/{numberOfQuestion}")]
        public IActionResult GetQuestionsWithNumberOfQuestions(long partOfSpeechId, long levelId, int numberOfQuestion)
        {
            try
            {
                return Ok(_questionService.GetQuestionResponseDtos(partOfSpeechId, levelId, numberOfQuestion));
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex.Message);
            }
        }
    }
}