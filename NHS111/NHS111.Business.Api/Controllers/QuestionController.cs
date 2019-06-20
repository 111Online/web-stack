using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web;
using System.Web.Helpers;
using System.Web.Http;
using System.Web.Http.Results;
using System.Web.Mvc;
using Newtonsoft.Json;
using NHS111.Business.Builders;
using NHS111.Business.Services;
using NHS111.Business.Transformers;
using NHS111.Models.Models.Business.Question;
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

        [System.Web.Http.HttpPost]
        [System.Web.Http.Route("node/{pathwayId}/{currentNodeType}/next_node/{nodeId}")]
        public async Task<JsonResult<QuestionWithAnswers>> GetNextNode(string pathwayId, NodeType currentNodeType, string nodeId,
            string state, [FromBody] string answer)
        {
            return await GetNextNode(pathwayId, currentNodeType.ToString(), nodeId, state, answer);
        }

        [System.Web.Http.HttpPost]
        [System.Web.Http.Route("questions/fullPathwayJourney")]
        public async Task<JsonResult<IEnumerable<QuestionWithAnswers>>> GetFullPathwayJourney([FromBody]FullPathwayJourney fullPathwayJourney)
        {
            var journey = await _questionService.GetFullPathwayJourney(fullPathwayJourney.StartingPathwayType, fullPathwayJourney.JourneySteps.ToArray(), fullPathwayJourney.StartingPathwayId, fullPathwayJourney.DispostionCode, fullPathwayJourney.State);
            return Json(journey);
        }


        public async Task<JsonResult<QuestionWithAnswers>> GetNextNode(string pathwayId, string nodeLabel, string nodeId, string state, [FromBody]string answer, string cacheKey = null)
        {
#if !DEBUG
                cacheKey = cacheKey ?? string.Format("{0}-{1}-{2}-{3}", pathwayId, nodeId, answer, state);

                var cacheValue = await _cacheManager.Read(cacheKey);
                if (!string.IsNullOrEmpty(cacheValue))
                {
                    return Json(JsonConvert.DeserializeObject<QuestionWithAnswers>(cacheValue));
                }
#endif

            var question = await _questionService.GetNextQuestion(nodeId, nodeLabel, answer);
            var stateDictionary = JsonConvert.DeserializeObject<IDictionary<string, string>>(HttpUtility.UrlDecode(state));

            var nextLabel = question.Labels.FirstOrDefault();

            if (nextLabel == "Question" || nextLabel == "Outcome")
            {
                question.State = stateDictionary;
                var result = _questionTransformer.AsQuestionWithAnswers(question);

#if !DEBUG
                    _cacheManager.Set(cacheKey, JsonConvert.SerializeObject(result));
#endif

                return Json(result);
            }

            if (nextLabel == "DeadEndJump" || nextLabel == "Page")
            {
                question.State = stateDictionary;
                //var result = _questionTransformer.AsQuestionWithDeadEnd(question);
                return Json(question);
            }

            if (nextLabel == "PathwaySelectionJump")
            {
                question.State = stateDictionary;
                //var result = _questionTransformer.AsQuestionWithPathwaySelection(question);
                return Json(question);
            }

            if (nextLabel == "Set" || nextLabel == "Read")
            {
                var computedAnswer = question.Answers.First();
                if (nextLabel == "Read")
                {
                    var value = stateDictionary.ContainsKey(question.Question.Title)
                        ? stateDictionary[question.Question.Title]
                        : null;
                    computedAnswer = _answersForNodeBuilder.SelectAnswer(question.Answers, value);
                }
                else
                {
                    if (!stateDictionary.ContainsKey(question.Question.Title))
                        stateDictionary.Add(question.Question.Title, computedAnswer.Title);
                }
                var updatedState = JsonConvert.SerializeObject(stateDictionary);
                var nextQuestion = (await GetNextNode(pathwayId, nextLabel, question.Question.Id, updatedState, computedAnswer.Title, cacheKey)).Content;
                
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

                return Json(nextQuestion);
            }

            if (nextLabel == "CareAdvice")
            {
                stateDictionary.Add(question.Question.QuestionNo, "");
                question.State = stateDictionary;
                //next.Answers.First().Keywords += "|" + answered.Keywords;
                //nextAnswer.ExcludeKeywords += "|" + answered.ExcludeKeywords;

                //var result = _questionTransformer.AsQuestionWithAnswers(question);
                return Json(question);
            }

            if (nextLabel == "InlineDisposition")
            {
                return await GetNextNode(pathwayId, question.Question.Id, state, question.Answers.First().Title, cacheKey);
            }

            throw new Exception(string.Format("Unrecognized node of type '{0}'.", nextLabel));
        }

        [System.Web.Http.Route("node/{pathwayId}/answers/{questionId}")]
        public async Task<JsonResult<IEnumerable<Answer>>> GetAnswers(string pathwayId, string questionId)
        {
            var answers = await _questionService.GetAnswersForQuestion(questionId);
            return Json(_questionTransformer.AsAnswers(answers));
        }

        [System.Web.Http.Route("node/{pathwayId}/question/{questionId}")]
        public async Task<JsonResult<QuestionWithAnswers>> GetQuestionById(string pathwayId, string questionId, string cacheKey = null)
        {
#if !DEBUG
                cacheKey = cacheKey ?? string.Format("GetQuestionById-{0}-{1}", pathwayId, questionId);

                var cacheValue = await _cacheManager.Read(cacheKey);
                if (!string.IsNullOrEmpty(cacheValue))
                {
                    return Json(JsonConvert.DeserializeObject<QuestionWithAnswers>(cacheValue));
                }
#endif

            var node = await _questionService.GetQuestion(questionId);

            var nextLabel = node.Labels.FirstOrDefault();

            if (nextLabel == "Question" || nextLabel == "Outcome" || nextLabel == "CareAdvice" || nextLabel == "Page")
            {
                var result = _questionTransformer.AsQuestionWithAnswers(node);

#if !DEBUG
                    _cacheManager.Set(cacheKey, JsonConvert.SerializeObject(result));
#endif

                return Json(result);
            }

            if (nextLabel == "DeadEndJump")
            {
                //var result = _questionTransformer.AsQuestionWithDeadEnd(JsonConvert.SerializeObject(node));
                return Json(node);
            }

            throw new Exception(string.Format("Unrecognized node of type '{0}'.", nextLabel));

        }

        [System.Web.Http.HttpGet]
        [System.Web.Http.Route("node/{pathwayId}/questions/first")]
        public async Task<JsonResult<QuestionWithAnswers>> GetFirstQuestion(string pathwayId, [FromUri]string state)
        {
            var firstNodeJson = await _questionService.GetFirstQuestion(pathwayId);
            var firstNode = _questionTransformer.AsQuestionWithAnswers(firstNodeJson);

            var stateDictionary = JsonConvert.DeserializeObject<IDictionary<string, string>>(HttpUtility.UrlDecode(state));

            // set the system variables relevant to online
            foreach (var systemVariable in _systemVariables)
                stateDictionary.Add(systemVariable.Key, systemVariable.Value);

            var nextLabel = firstNode.Labels.FirstOrDefault();

            if (nextLabel == "Read")
            {
                var answers = await _questionService.GetAnswersForQuestion(firstNode.Question.Id);
                var value = stateDictionary.ContainsKey(firstNode.Question.Title) ? stateDictionary[firstNode.Question.Title] : null;
                var selected = _answersForNodeBuilder.SelectAnswer(answers, value);
                return await GetNextNode(pathwayId, nextLabel, firstNode.Question.Id, JsonConvert.SerializeObject(stateDictionary), selected.Title);
            }
            if (nextLabel == "Set")
            {
                var answers = await _questionService.GetAnswersForQuestion(firstNode.Question.Id);
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

            return Json(firstNode);
        }

        [System.Web.Http.Route("node/{pathwayId}/jtbs_first")]
        public async Task<JsonResult<IEnumerable<QuestionWithAnswers>>> GetJustToBeSafePartOneNodes(string pathwayId)
        {
            var questionsWithAnswers = await _questionService.GetJustToBeSafeQuestionsFirst(pathwayId);
            return Json(_questionTransformer.AsQuestionWithAnswersList(questionsWithAnswers));
        }

        [System.Web.Http.Route("node/{pathwayId}/jtbs/second/{answeredQuestionIds}/{multipleChoice}/{questionId?}")]
        public async Task<JsonResult<IEnumerable<QuestionWithAnswers>>> GetJustToBeSafePartTwoNodes(string pathwayId, string answeredQuestionIds, bool multipleChoice, string questionId = "")
        {
            var questionsWithAnswers = await _questionService.GetJustToBeSafeQuestionsNext(pathwayId, answeredQuestionIds.Split(','), multipleChoice, questionId);
            return Json(_questionTransformer.AsQuestionWithAnswersList(questionsWithAnswers));
        }
    }
}