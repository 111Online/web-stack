using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using Newtonsoft.Json;
using NHS111.Business.Builders;
using NHS111.Business.Configuration;
using NHS111.Models.Models.Configuration;
using NHS111.Models.Models.Domain;
using NHS111.Models.Models.Web.FromExternalServices;
using NHS111.Utils.Helpers;

namespace NHS111.Business.Services
{
    public class QuestionService : IQuestionService
    {
        private readonly IConfiguration _configuration;
        private readonly IRestfulHelper _restfulHelper;
        private readonly IAnswersForNodeBuilder _answersForNodeBuilder;
        private readonly IModZeroJourneyStepsBuilder _modZeroJourneyStepsBuilder;
        public QuestionService(IConfiguration configuration, IRestfulHelper restfulHelper, IAnswersForNodeBuilder answersForNodeBuilder, IModZeroJourneyStepsBuilder modZeroJourneyStepsBuilder)
        {
            _configuration = configuration;
            _restfulHelper = restfulHelper;
            _answersForNodeBuilder = answersForNodeBuilder;
            _modZeroJourneyStepsBuilder = modZeroJourneyStepsBuilder;
        }

        public async Task<string> GetQuestion(string id)
        {
            return await _restfulHelper.GetAsync(_configuration.GetDomainApiQuestionUrl(id));
        }

        public async Task<string> GetAnswersForQuestion(string id)
        {
            return await _restfulHelper.GetAsync(_configuration.GetDomainApiAnswersForQuestionUrl(id));
        }

        public async Task<HttpResponseMessage> GetNextQuestion(string id, string nodeLabel, string answer)
        {
            var request = new HttpRequestMessage { Content = new StringContent(JsonConvert.SerializeObject(answer), Encoding.UTF8, "application/json") };
            return (await _restfulHelper.PostAsync(_configuration.GetDomainApiNextQuestionUrl(id, nodeLabel), request));
        }

        public async Task<string> GetFirstQuestion(string pathwayId)
        {
            return await _restfulHelper.GetAsync(_configuration.GetDomainApiFirstQuestionUrl(pathwayId));

        }

        public async Task<string> GetJustToBeSafeQuestionsFirst(string pathwayId)
        {
            return await _restfulHelper.GetAsync(_configuration.GetDomainApiJustToBeSafeQuestionsFirstUrl(pathwayId));
        }

        public async Task<HttpResponseMessage> GetFullPathwayJourney(string traumaType, JourneyStep[] steps, string startingPathwayId, string dispositionCode, IDictionary<string, string> state)
        {
            var age = 0;
            if (!int.TryParse(FindStateValue(state, "PATIENT_AGE"), out age))
            {
                var ageGroup = FindStateValue(state, "PATIENT_AGEGROUP");
                age = new AgeCategory(ageGroup).MinimumAge;
            }

            var gender = FindStateValue(state, "PATIENT_GENDER") == "\"F\"" ? "Female" : "Male";
  
            var moduleZeroJourney = await GetModuleZeroJourney(gender, age, traumaType);
            
            var pathwaysJourney = await GetPathwayJourney(steps, startingPathwayId, dispositionCode);
            var filteredJourneySteps = NavigateReadNodeLogic(steps, pathwaysJourney.ToList(), state);

            var content = new StringContent(JsonConvert.SerializeObject(moduleZeroJourney.Concat(filteredJourneySteps)), Encoding.UTF8, "application/json");
            return new HttpResponseMessage(HttpStatusCode.OK) { Content = content };
        }

        private IEnumerable<QuestionWithAnswers> NavigateReadNodeLogic(JourneyStep[] answeredQuestions, IEnumerable<QuestionWithAnswers> journey, IDictionary<string, string> state)
        {
            var filteredJourney = new List<QuestionWithAnswers>();

            var groupledRead = journey.Where(s => s.Labels.Contains("Read")).GroupBy(s => s.Question.Id, s => s.Answered,
                (key, g) => new { Node = key, Answers = g.ToList().Distinct() });

            var pathNavigationAnswers = new List<Answer>();
            foreach (var step in journey)
            {
                if (!step.Labels.Contains("Read"))
                {
                    if(!step.Labels.Contains("Question") || (step.Labels.Contains("Question") && answeredQuestions.Any(q => q.QuestionId == step.Question.Id)))
                        filteredJourney.Add(step);
                }
                else
                {
                    var answers = groupledRead.First(s => s.Node == step.Question.Id).Answers;
                    var value = FindStateValue(state, step);
                    var answer = _answersForNodeBuilder.SelectAnswer(answers, value);
                    if (answer != default(Answer) && step.Answered.Title == answer.Title)
                    {
                        filteredJourney.Add(step);
                        pathNavigationAnswers.Add(answer);
                    }
                }
            }

            return filteredJourney;
        }

        private static IDictionary<string, string> BuildState(string gender, int age, IDictionary<string, string> state)
        {
            var ageCategory = new AgeCategory(age);

            state.Add("PATIENT_AGE", age.ToString());
            state.Add("PATIENT_GENDER", string.Format("\"{0}\"", gender.First().ToString().ToUpper()));
            state.Add("PATIENT_PARTY", "1");
            state.Add("PATIENT_AGEGROUP", ageCategory.Value);

            return state;
        }

        private string FindStateValue(IDictionary<string, string> state, QuestionWithAnswers step)
        {
            return FindStateValue(state, step.Question.Title);
        }

        private string FindStateValue(IDictionary<string, string> state, string key)
        {
            return state.ContainsKey(key)
                ? state[key]
                : null;
        }

        private async Task<IEnumerable<QuestionWithAnswers>> GetModuleZeroJourney(string gender, int age, string traumaType)
        {
            var pathwayJourney = _modZeroJourneyStepsBuilder.GetModZeroJourney(gender, age, traumaType);
            var steps = pathwayJourney.Steps;

            var request = new HttpRequestMessage { Content = new StringContent(JsonConvert.SerializeObject(steps), Encoding.UTF8, "application/json") };
            var response = await _restfulHelper.PostAsync(_configuration.GetDomainApiPathwayJourneyUrl(pathwayJourney.PathwayId, pathwayJourney.DispositionId), request).ConfigureAwait(false);

            var moduleZeroJourney = JsonConvert.DeserializeObject<IEnumerable<QuestionWithAnswers>>(await response.Content.ReadAsStringAsync());
            var state = BuildState(gender, age, pathwayJourney.State);
            var filteredModZeroJourney = NavigateReadNodeLogic(steps.ToArray(), moduleZeroJourney.ToList(), state);

            return filteredModZeroJourney;
        }

        public async Task<IEnumerable<QuestionWithAnswers>> GetPathwayJourney(JourneyStep[] steps, string startingPathwayId, string dispositionCode)
        {
            var request = new HttpRequestMessage { Content = new StringContent(JsonConvert.SerializeObject(steps), Encoding.UTF8, "application/json") };
            var response = await _restfulHelper.PostAsync(_configuration.GetDomainApiPathwayJourneyUrl(startingPathwayId, dispositionCode), request).ConfigureAwait(false);
            return JsonConvert.DeserializeObject<IEnumerable<QuestionWithAnswers>>(await response.Content.ReadAsStringAsync());
        }

        public async Task<string> GetJustToBeSafeQuestionsNext(string pathwayId, IEnumerable<string> answeredQuestionIds, bool multipleChoice, string selectedQuestionId)
        {
            return await _restfulHelper.GetAsync(_configuration.GetDomainApiJustToBeSafeQuestionsNextUrl(pathwayId, answeredQuestionIds, multipleChoice, selectedQuestionId));
        }
    }

    public interface IQuestionService
    {
        Task<IEnumerable<QuestionWithAnswers>> GetPathwayJourney(JourneyStep[] steps, string startingPathwayId, string dispositionCode);
        Task<HttpResponseMessage> GetFullPathwayJourney(string traumaType, JourneyStep[] steps, string startingPathwayId, string dispositionCode, IDictionary<string, string> state);
        Task<string> GetQuestion(string id);
        Task<string> GetAnswersForQuestion(string id);
        Task<HttpResponseMessage> GetNextQuestion(string id, string nodeLabel,  string answer);
        Task<string> GetFirstQuestion(string pathwayId);
        Task<string> GetJustToBeSafeQuestionsFirst(string pathwayId);
        Task<string> GetJustToBeSafeQuestionsNext(string pathwayId, IEnumerable<string> answeredQuestionIds, bool multipleChoice, string selectedQuestionId);
    }
}