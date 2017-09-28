using System.Net.Http;
using System.Threading.Tasks;
using System.Web.Http;
using NHS111.Domain.Repository;
using NHS111.Utils.Attributes;
using NHS111.Utils.Extensions;

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
        public async Task<HttpResponseMessage> GetQuestion(string questionId)
        {
            return await _questionRepository.GetQuestion(questionId).AsJson().AsHttpResponse();
        }

        [HttpGet]
        [Route("questions/{questionId}/answers")]
        public async Task<HttpResponseMessage> GetAnswersForQuestion(string questionId)
        {
            return await _questionRepository.GetAnswersForQuestion(questionId).AsJson().AsHttpResponse();
        }

        [HttpPost]
        [Route("questions/{questionId}/{nodeLabel}/answersNext")]
        public async Task<HttpResponseMessage> GetNextQuestion(string questionId, string nodeLabel, [FromBody]string answer)
        {
            return await _questionRepository.GetNextQuestion(questionId, nodeLabel, answer).AsJson().AsHttpResponse();
        }

        [HttpGet]
        [Route("pathways/{pathwayId}/questions/first")]
        public async Task<HttpResponseMessage> GetFirstQuestion(string pathwayId)
        {
            return await _questionRepository.GetFirstQuestion(pathwayId).AsJson().AsHttpResponse();
        }

        [HttpGet]
        [Route("pathways/{pathwayId}/just-to-be-safe/first")]
        public async Task<HttpResponseMessage> GetJustToBeSafeQuestionsFirst(string pathwayId)
        {
            return await _questionRepository.GetJustToBeSafeQuestions(pathwayId, "1").AsJson().AsHttpResponse();
        }

        [HttpGet]
        [Route("pathways/{pathwayId}/just-to-be-safe/next")]
        public async Task<HttpResponseMessage> GetJustToBeSafeQuestionsNext(string pathwayId, [FromUri]string answeredQuestionIds, [FromUri]bool multipleChoice, [FromUri]string selectedQuestionId = "")
        {
            return await _questionRepository.GetJustToBeSafeQuestions(pathwayId, selectedQuestionId, multipleChoice, answeredQuestionIds).AsJson().AsHttpResponse();
        }
    }
}