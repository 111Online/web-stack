using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web;
using System.Web.Http;
using Newtonsoft.Json;
using NHS111.Business.Builders;
using NHS111.Business.Services;
using NHS111.Business.Transformers;
using NHS111.Utils.Attributes;
using NHS111.Models.Models.Domain;
using NHS111.Models.Models.Web.Enums;
using NHS111.Models.Models.Web.FromExternalServices;
using NHS111.Utils.Cache;
using NHS111.Utils.Extensions;

namespace NHS111.Business.Api.Controllers
{
    [LogHandleErrorForApi]
    public class QuestionController : ApiController
    {
        private const string SectionName = "pathwaysSystemVariables";
        private readonly Dictionary<string, string> _systemVariables;

        private readonly IQuestionService _questionService;
        private readonly IQuestionTransformer _questionTransformer;
        private readonly IAnswersForNodeBuilder _answersForNodeBuilder;
        private readonly ICacheManager<string, string> _cacheManager;

        public QuestionController(IQuestionService questionService, IQuestionTransformer questionTransformer, IAnswersForNodeBuilder answersForNodeBuilder, ICacheManager<string, string> cacheManager)
        {
            _questionService = questionService;
            _questionTransformer = questionTransformer;
            _answersForNodeBuilder = answersForNodeBuilder;
            _cacheManager = cacheManager;

            var section = ConfigurationManager.GetSection(SectionName) as System.Collections.Hashtable;
            if (section == null)
                throw new InvalidOperationException(string.Format("Missing section name {0}", SectionName));

            _systemVariables = section
                .Cast<System.Collections.DictionaryEntry>()
                .ToDictionary(n => n.Key.ToString(), n => n.Value.ToString());
        }

        [HttpPost]
        [Route("node/{pathwayId}/{currentNodeType}/next_node/{nodeId}")]
        public async Task<HttpResponseMessage> GetNextNode(string pathwayId, NodeType currentNodeType, string nodeId,
            string state, [FromBody] string answer)
        {
            return await GetNextNode(pathwayId, currentNodeType.ToString(), nodeId, state, answer);
        }

        [HttpPost]
        [Route("questions/fullPathwaysJourney/{startingPathwayId}")]
        public async Task<HttpResponseMessage> GetFullPathwayJourney([FromBody]JourneyStep[] steps, string startingPathwayId)
        {
            var response = await _questionService.GetFullPathwayJourney(steps, startingPathwayId);
            return response;
        }


        public async Task<HttpResponseMessage> GetNextNode(string pathwayId, string nodeLabel, string nodeId, string state, [FromBody]string answer, string cacheKey = null)
        {
#if !DEBUG
                cacheKey = cacheKey ?? string.Format("{0}-{1}-{2}-{3}", pathwayId, nodeId, answer, state);

                var cacheValue = await _cacheManager.Read(cacheKey);
                if (cacheValue != null)
                {
                    return cacheValue.AsHttpResponse();
                }
#endif

            var next = JsonConvert.DeserializeObject<QuestionWithAnswers>(await (await _questionService.GetNextQuestion(nodeId, nodeLabel, answer)).Content.ReadAsStringAsync());
            var stateDictionary = JsonConvert.DeserializeObject<IDictionary<string, string>>(HttpUtility.UrlDecode(state));

            var nextLabel = next.Labels.FirstOrDefault();

            if (nextLabel == "Question" || nextLabel == "Outcome")
            {
                next.State = stateDictionary;
                var result = _questionTransformer.AsQuestionWithAnswers(JsonConvert.SerializeObject(next));

#if !DEBUG
                    _cacheManager.Set(cacheKey, result);
#endif

                return result.AsHttpResponse();
            }

            if (nextLabel == "DeadEndJump")
            {
                next.State = stateDictionary;
                var result = _questionTransformer.AsQuestionWithDeadEnd(JsonConvert.SerializeObject(next));
                return result.AsHttpResponse();
            }

            if (nextLabel == "PathwaySelectionJump")
            {
                next.State = stateDictionary;
                var result = _questionTransformer.AsQuestionWithDeadEnd(JsonConvert.SerializeObject(next));
                return result.AsHttpResponse();
            }

            if (nextLabel == "Set" || nextLabel == "Read")
            {
                var computedAnswer = next.Answers.First();
                if (nextLabel == "Read")
                {
                    var value = stateDictionary.ContainsKey(next.Question.Title)
                        ? stateDictionary[next.Question.Title]
                        : null;
                    computedAnswer = _answersForNodeBuilder.SelectAnswer(next.Answers, value);
                }
                else
                {
                    if (!stateDictionary.ContainsKey(next.Question.Title))
                        stateDictionary.Add(next.Question.Title, computedAnswer.Title);
                }
                var updatedState = JsonConvert.SerializeObject(stateDictionary);
                var httpResponseMessage = await GetNextNode(pathwayId, nextLabel, next.Question.Id, updatedState, computedAnswer.Title, cacheKey);
                var nextQuestion = JsonConvert.DeserializeObject<QuestionWithAnswers>(await httpResponseMessage.Content.ReadAsStringAsync());

                nextQuestion.NonQuestionKeywords = computedAnswer.Keywords;
                nextQuestion.NonQuestionExcludeKeywords = computedAnswer.ExcludeKeywords;
                if (nextQuestion.Answers != null)
                {
                    foreach (var nextAnswer in nextQuestion.Answers)
                    {
                        nextAnswer.Keywords += "|" + computedAnswer.Keywords;
                        nextAnswer.ExcludeKeywords += "|" + computedAnswer.ExcludeKeywords;
                    }
                }

                // have to add the node to the cache so thats its not missed when going back
                // to collect keywords 
                var result = JsonConvert.SerializeObject(nextQuestion);

#if !DEBUG
 	                _cacheManager.Set(cacheKey, result);
#endif

                return result.AsHttpResponse();
            }

            if (nextLabel == "CareAdvice")
            {
                stateDictionary.Add(next.Question.QuestionNo, "");
                next.State = stateDictionary;
                //next.Answers.First().Keywords += "|" + answered.Keywords;
                //nextAnswer.ExcludeKeywords += "|" + answered.ExcludeKeywords;

                var result = _questionTransformer.AsQuestionWithAnswers(JsonConvert.SerializeObject(next));
                return result.AsHttpResponse();
            }

            if (nextLabel == "InlineDisposition")
            {
                return await GetNextNode(pathwayId, next.Question.Id, state, next.Answers.First().Title, cacheKey);
            }

            throw new Exception(string.Format("Unrecognized node of type '{0}'.", nextLabel));
        }

        [Route("node/{pathwayId}/answers/{questionId}")]
        public async Task<HttpResponseMessage> GetAnswers(string pathwayId, string questionId)
        {
            return _questionTransformer.AsAnswers(await _questionService.GetAnswersForQuestion(questionId)).AsHttpResponse();
        }

        [Route("node/{pathwayId}/question/{questionId}")]
        public async Task<HttpResponseMessage> GetQuestionById(string pathwayId, string questionId, string cacheKey = null)
        {
#if !DEBUG
                cacheKey = cacheKey ?? string.Format("GetQuestionById-{0}-{1}", pathwayId, questionId);

                var cacheValue = await _cacheManager.Read(cacheKey);
                if (cacheValue != null)
                {
                    return cacheValue.AsHttpResponse();
                }
#endif

            var node = JsonConvert.DeserializeObject<QuestionWithAnswers>(await _questionService.GetQuestion(questionId));

            var nextLabel = node.Labels.FirstOrDefault();

            if (nextLabel == "Question" || nextLabel == "Outcome" || nextLabel == "CareAdvice")
            {
                var result = _questionTransformer.AsQuestionWithAnswers(JsonConvert.SerializeObject(node));

#if !DEBUG
                    _cacheManager.Set(cacheKey, result);
#endif

                return result.AsHttpResponse();
            }

            if (nextLabel == "DeadEndJump")
            {
                var result = _questionTransformer.AsQuestionWithDeadEnd(JsonConvert.SerializeObject(node));
                return result.AsHttpResponse();
            }

            throw new Exception(string.Format("Unrecognized node of type '{0}'.", nextLabel));

        }

        [HttpGet]
        [Route("node/{pathwayId}/questions/first")]
        public async Task<HttpResponseMessage> GetFirstQuestion(string pathwayId, [FromUri]string state)
        {
            var firstNodeJson = _questionTransformer.AsQuestionWithAnswers(await (await _questionService.GetFirstQuestion(pathwayId).AsHttpResponse()).Content.ReadAsStringAsync());
            var firstNode = JsonConvert.DeserializeObject<QuestionWithAnswers>(firstNodeJson);

            var stateDictionary = JsonConvert.DeserializeObject<IDictionary<string, string>>(HttpUtility.UrlDecode(state));

            // set the system variables relevant to online
            foreach (var systemVariable in _systemVariables)
                stateDictionary.Add(systemVariable.Key, systemVariable.Value);

            var nextLabel = firstNode.Labels.FirstOrDefault();

            if (nextLabel == "Read")
            {
                var answers = JsonConvert.DeserializeObject<IEnumerable<Answer>>(await _questionService.GetAnswersForQuestion(firstNode.Question.Id));
                var value = stateDictionary.ContainsKey(firstNode.Question.Title) ? stateDictionary[firstNode.Question.Title] : null;
                var selected = _answersForNodeBuilder.SelectAnswer(answers, value);
                return await GetNextNode(pathwayId, nextLabel, firstNode.Question.Id, JsonConvert.SerializeObject(stateDictionary), selected.Title);
            }
            if (nextLabel == "Set")
            {
                var answers = JsonConvert.DeserializeObject<IEnumerable<Answer>>(await _questionService.GetAnswersForQuestion(firstNode.Question.Id));
                stateDictionary.Add(firstNode.Question.Title, answers.First().Title);
                var updatedState = JsonConvert.SerializeObject(stateDictionary);
                return await GetNextNode(pathwayId, nextLabel, firstNode.Question.Id, updatedState, answers.First().Title);
            }

            if (firstNode.State == null)
                firstNode.State = stateDictionary;
            else
            {
                // add the system variables relevant to online
                foreach (var systemVariable in _systemVariables)
                    firstNode.State.Add(systemVariable.Key, systemVariable.Value);
            }

            return JsonConvert.SerializeObject(firstNode).AsHttpResponse();
        }

        [Route("node/{pathwayId}/jtbs_first")]
        public async Task<HttpResponseMessage> GetJustToBeSafePartOneNodes(string pathwayId)
        {
            return _questionTransformer.AsQuestionWithAnswersList(await _questionService.GetJustToBeSafeQuestionsFirst(pathwayId)).AsHttpResponse();
        }

        [Route("node/{pathwayId}/jtbs/second/{answeredQuestionIds}/{multipleChoice}/{questionId?}")]
        public async Task<HttpResponseMessage> GetJustToBeSafePartTwoNodes(string pathwayId, string answeredQuestionIds, bool multipleChoice, string questionId = "")
        {
            return _questionTransformer.AsQuestionWithAnswersList(await _questionService.GetJustToBeSafeQuestionsNext(pathwayId, answeredQuestionIds.Split(','), multipleChoice, questionId)).AsHttpResponse();
        }
    }
}