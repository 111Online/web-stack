using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;
using NHS111.Business.Configuration;
using NHS111.Models.Models.Configuration;
using NHS111.Models.Models.Domain;
using NHS111.Models.Models.Web.FromExternalServices;
using NHS111.Utils.Helpers;

namespace NHS111.Business.Services
{
    public class QuestionService : IQuestionService
    {
        private const string Section = "moduleZeroJourneys";

        private readonly IConfiguration _configuration;
        private readonly IRestfulHelper _restfulHelper;

        public QuestionService(IConfiguration configuration, IRestfulHelper restfulHelper)
        {
            _configuration = configuration;
            _restfulHelper = restfulHelper;
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

        public async Task<HttpResponseMessage> GetFullPathwayJourney(string gender, string age, bool isTrauma, JourneyStep[] steps, string startingPathwayId)
        {
            var moduleZeroJourney = await GetModuleZeroJourney(gender, age, isTrauma);
            var pathwaysJourney = await GetPathwaysJourney(steps, startingPathwayId);

            var content = new StringContent(JsonConvert.SerializeObject(moduleZeroJourney.Concat(pathwaysJourney)), Encoding.UTF8, "application/json");
            return new HttpResponseMessage(HttpStatusCode.OK) { Content = content };
        }

        private async Task<IEnumerable<QuestionWithAnswers>> GetModuleZeroJourney(string gender, string age, bool isTrauma)
        {
            var section = ConfigurationManager.GetSection(Section) as ModZeroJourneysSection;
            if (section == null)
                throw new InvalidOperationException(string.Format("Missing section name {0}", "moduleZeroTriage"));

            var modZeroJourney = section.ModuleZeroJourneys.GetModZeroJourneyElement(age, gender, isTrauma);
            var steps = modZeroJourney.JourneySteps.Select(j => new JourneyStep {QuestionId = j.Id, Answer = new Answer {Order = j.Order}});

            var request = new HttpRequestMessage { Content = new StringContent(JsonConvert.SerializeObject(steps), Encoding.UTF8, "application/json") };
            var response = await _restfulHelper.PostAsync(_configuration.GetDomainApiFullPathwayJourneyUrl(modZeroJourney.Id), request).ConfigureAwait(false);
            return JsonConvert.DeserializeObject<IEnumerable<QuestionWithAnswers>>(await response.Content.ReadAsStringAsync());
        }

        private async Task<IEnumerable<QuestionWithAnswers>> GetPathwaysJourney(JourneyStep[] steps, string startingPathwayId)
        {
            var request = new HttpRequestMessage { Content = new StringContent(JsonConvert.SerializeObject(steps), Encoding.UTF8, "application/json") };
            var response = await _restfulHelper.PostAsync(_configuration.GetDomainApiFullPathwayJourneyUrl(startingPathwayId), request).ConfigureAwait(false);
            return JsonConvert.DeserializeObject<IEnumerable<QuestionWithAnswers>>(await response.Content.ReadAsStringAsync());
        }

        public async Task<string> GetJustToBeSafeQuestionsNext(string pathwayId, IEnumerable<string> answeredQuestionIds, bool multipleChoice, string selectedQuestionId)
        {
            return await _restfulHelper.GetAsync(_configuration.GetDomainApiJustToBeSafeQuestionsNextUrl(pathwayId, answeredQuestionIds, multipleChoice, selectedQuestionId));
        }
    }

    public interface IQuestionService
    {
        Task<HttpResponseMessage> GetFullPathwayJourney(string gender, string age, bool isTrauma, JourneyStep[] steps, string startingPathwayId);
        Task<string> GetQuestion(string id);
        Task<string> GetAnswersForQuestion(string id);
        Task<HttpResponseMessage> GetNextQuestion(string id, string nodeLabel,  string answer);
        Task<string> GetFirstQuestion(string pathwayId);
        Task<string> GetJustToBeSafeQuestionsFirst(string pathwayId);
        Task<string> GetJustToBeSafeQuestionsNext(string pathwayId, IEnumerable<string> answeredQuestionIds, bool multipleChoice, string selectedQuestionId);
    }
}