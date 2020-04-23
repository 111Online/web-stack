using NHS111.Domain.Repository;
using NHS111.Models.Models.Domain;
using NHS111.Models.Models.Web.FromExternalServices;
using NHS111.Utils.Attributes;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web.Http;
using System.Web.Http.Results;

namespace NHS111.Domain.Api.Controllers
{
    [LogHandleErrorForApi]
    public class QuestionController : ApiController
    {
        private readonly IQuestionRepository _questionRepository;

        public QuestionController(IQuestionRepository questionRepository)
        {
            _questionRepository = questionRepository;
        }

        [HttpGet]
        [Route("questions/{questionId}")]
        public async Task<JsonResult<QuestionWithAnswers>> GetQuestion(string questionId)
        {
            var questionWithAnswers = await _questionRepository.GetQuestion(questionId);
            return Json(questionWithAnswers);
        }

        [HttpGet]
        [Route("questions/{questionId}/answers")]
        public async Task<JsonResult<IEnumerable<Answer>>> GetAnswersForQuestion(string questionId)
        {
            var answers = await _questionRepository.GetAnswersForQuestion(questionId);
            return Json(answers);
        }

        [HttpPost]
        [Route("questions/{questionId}/{nodeLabel}/answersNext")]
        public async Task<JsonResult<QuestionWithAnswers>> GetNextQuestion(string questionId, string nodeLabel, [FromBody]string answer)
        {
            var questionWithAnswers = await _questionRepository.GetNextQuestion(questionId, nodeLabel, answer);
            return Json(questionWithAnswers);
        }


        [HttpPost]
        [Route("questions/fullPathwayJourney/{startingPathwayId}/{dispositionCode}/{gender}/{age}")]
        public async Task<JsonResult<IEnumerable<QuestionWithAnswers>>> GetFullPathwayJourney([FromBody]JourneyStep[] steps, string startingPathwayId, string dispositionCode, string gender, int age)
        {
            var questionsWithAnswers = await _questionRepository.GetPathwaysJourney(steps.ToList(), startingPathwayId, dispositionCode, gender, age);
            return Json(questionsWithAnswers);
        }

        [HttpGet]
        [Route("pathways/{pathwayId}/questions/first")]
        public async Task<JsonResult<QuestionWithAnswers>> GetFirstQuestion(string pathwayId)
        {
            var questionWithAnswers = await _questionRepository.GetFirstQuestion(pathwayId);
            return Json(questionWithAnswers);
        }

        [HttpGet]
        [Route("pathways/{pathwayId}/just-to-be-safe/first")]
        public async Task<JsonResult<IEnumerable<QuestionWithAnswers>>> GetJustToBeSafeQuestionsFirst(string pathwayId)
        {
            var questionsWithAnswers = await _questionRepository.GetJustToBeSafeQuestions(pathwayId, "1");
            return Json(questionsWithAnswers);
        }

        [HttpGet]
        [Route("pathways/{pathwayId}/just-to-be-safe/next")]
        public async Task<JsonResult<IEnumerable<QuestionWithAnswers>>> GetJustToBeSafeQuestionsNext(string pathwayId, [FromUri]string answeredQuestionIds, [FromUri]bool multipleChoice, [FromUri]string selectedQuestionId = "")
        {
            var questionsWithAnswers = await _questionRepository.GetJustToBeSafeQuestions(pathwayId, selectedQuestionId, multipleChoice, answeredQuestionIds);
            return Json(questionsWithAnswers);
        }
    }
}