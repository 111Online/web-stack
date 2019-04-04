using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using System.Web.Http.Results;
using Newtonsoft.Json;
using NHS111.Business.Builders;
using NHS111.Business.Configuration;
using NHS111.Models.Models.Domain;
using NHS111.Models.Models.Web.FromExternalServices;
using NHS111.Utils.RestTools;
using RestSharp;

namespace NHS111.Business.Services
{
    public class QuestionService : IQuestionService
    {
        private readonly IConfiguration _configuration;
        private readonly IRestClient _restClient;
        private readonly IAnswersForNodeBuilder _answersForNodeBuilder;
        private readonly IModZeroJourneyStepsBuilder _modZeroJourneyStepsBuilder;
        public QuestionService(IConfiguration configuration, IRestClient restClientDomainApi, IAnswersForNodeBuilder answersForNodeBuilder, IModZeroJourneyStepsBuilder modZeroJourneyStepsBuilder)
        {
            _configuration = configuration;
            _restClient = restClientDomainApi;
            _answersForNodeBuilder = answersForNodeBuilder;
            _modZeroJourneyStepsBuilder = modZeroJourneyStepsBuilder;
        }

        public async Task<QuestionWithAnswers> GetQuestion(string id)
        {
            var questions = await _restClient.ExecuteTaskAsync<QuestionWithAnswers>(new JsonRestRequest(_configuration.GetDomainApiQuestionUrl(id), Method.GET));
            return questions.Data;
        }

        public async Task<Answer[]> GetAnswersForQuestion(string id)
        {
            var questions = await _restClient.ExecuteTaskAsync<Answer[]>(new JsonRestRequest(_configuration.GetDomainApiAnswersForQuestionUrl(id), Method.GET));
            return questions.Data;
        }

        public async Task<QuestionWithAnswers> GetNextQuestion(string id, string nodeLabel, string answer)
        {
            var request = new JsonRestRequest(_configuration.GetDomainApiAnswersForQuestionUrl(id), Method.POST);
            request.AddJsonBody(answer);
            var questions = await _restClient.ExecuteTaskAsync<QuestionWithAnswers>(request);
            return questions.Data;
        }

        public async Task<QuestionWithAnswers> GetFirstQuestion(string pathwayId)
        {
            var questions = await _restClient.ExecuteTaskAsync<QuestionWithAnswers>(new JsonRestRequest(_configuration.GetDomainApiFirstQuestionUrl(pathwayId), Method.GET));
            return questions.Data;
        }

        public async Task<IEnumerable<QuestionWithAnswers>> GetJustToBeSafeQuestionsFirst(string pathwayId)
        {
            var questions = await _restClient.ExecuteTaskAsync<IEnumerable<QuestionWithAnswers>>(new JsonRestRequest(_configuration.GetDomainApiJustToBeSafeQuestionsFirstUrl(pathwayId), Method.GET));
            return questions.Data;
        }

        public async Task<IEnumerable<QuestionWithAnswers>> GetFullPathwayJourney(string traumaType, JourneyStep[] steps, string startingPathwayId, string dispositionCode, IDictionary<string, string> state)
        {
            var age = GetAgeFromState(state);
            var gender = GetGenderFromState(state);
            var moduleZeroJourney = await GetModuleZeroJourney(gender, age, traumaType);
            
            var pathwaysJourney = await GetPathwayJourney(steps, startingPathwayId, dispositionCode);
            var filteredJourneySteps = NavigateReadNodeLogic(steps, pathwaysJourney.ToList(), state);

            return moduleZeroJourney.Concat(filteredJourneySteps);
        }

        private int GetAgeFromState(IDictionary<string, string> state)
        {
            int age;
            if (!int.TryParse(FindStateValue(state, "PATIENT_AGE"), out age))
                throw new ArgumentException("State value for key 'PATIENT_AGE' must be an integer.");
            return age;
        }

        private string GetGenderFromState(IDictionary<string, string> state)
        {
            var genderStateValue = FindStateValue(state, "PATIENT_GENDER");

            if (!(genderStateValue == "\"F\"" || genderStateValue == "\"M\""))
                throw new ArgumentException("State value for key 'PATIENT_GENDER' must be of value \"F\" or \"M\"");

            var gender = genderStateValue == "\"F\"" ? "Female" : "Male";
            return gender;
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

            var request = new JsonRestRequest(_configuration.GetDomainApiPathwayJourneyUrl(pathwayJourney.PathwayId, pathwayJourney.DispositionId), Method.POST);
            request.AddJsonBody(steps);
            var moduleZeroJourney = await _restClient.ExecuteTaskAsync<IEnumerable<QuestionWithAnswers>>(request);

            var state = BuildState(gender, age, pathwayJourney.State);
            var filteredModZeroJourney = NavigateReadNodeLogic(steps.ToArray(), moduleZeroJourney.Data.ToList(), state);

            return filteredModZeroJourney;
        }

        public async Task<IEnumerable<QuestionWithAnswers>> GetPathwayJourney(JourneyStep[] steps, string startingPathwayId, string dispositionCode)
        {
            var request = new JsonRestRequest(_configuration.GetDomainApiPathwayJourneyUrl(startingPathwayId, dispositionCode), Method.POST);
            request.AddJsonBody(steps);
            var pathwayJourney = await _restClient.ExecuteTaskAsync<IEnumerable<QuestionWithAnswers>>(request);

            return pathwayJourney.Data;
        }

        public async Task<IEnumerable<QuestionWithAnswers>> GetJustToBeSafeQuestionsNext(string pathwayId, IEnumerable<string> answeredQuestionIds, bool multipleChoice, string selectedQuestionId)
        {
            var questions = await _restClient.ExecuteTaskAsync<IEnumerable<QuestionWithAnswers>>(new JsonRestRequest(_configuration.GetDomainApiJustToBeSafeQuestionsNextUrl(pathwayId, answeredQuestionIds, multipleChoice, selectedQuestionId), Method.GET));
            return questions.Data;
        }
    }

    public interface IQuestionService
    {
        Task<IEnumerable<QuestionWithAnswers>> GetFullPathwayJourney(string traumaType, JourneyStep[] steps, string startingPathwayId, string dispositionCode, IDictionary<string, string> state);
        Task<QuestionWithAnswers> GetQuestion(string id);
        Task<Answer[]> GetAnswersForQuestion(string id);
        Task<QuestionWithAnswers> GetNextQuestion(string id, string nodeLabel,  string answer);
        Task<QuestionWithAnswers> GetFirstQuestion(string pathwayId);
        Task<IEnumerable<QuestionWithAnswers>> GetJustToBeSafeQuestionsFirst(string pathwayId);
        Task<IEnumerable<QuestionWithAnswers>> GetJustToBeSafeQuestionsNext(string pathwayId, IEnumerable<string> answeredQuestionIds, bool multipleChoice, string selectedQuestionId);
    }
}