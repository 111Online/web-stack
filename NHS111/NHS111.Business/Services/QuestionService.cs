using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;
using NHS111.Business.Configuration;
using NHS111.Models.Models.Web.FromExternalServices;
using NHS111.Utils.Helpers;

namespace NHS111.Business.Services
{
    public class QuestionService : IQuestionService
    {
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


        public async Task<HttpResponseMessage> GetFullPathwayJourney(JourneyStep[] steps)
        {
            var request = new HttpRequestMessage { Content = new StringContent(JsonConvert.SerializeObject(steps), Encoding.UTF8, "application/json") };
            var response = await _restfulHelper.PostAsync(_configuration.GetDomainApiFullPathwayJourneyUrl(), request).ConfigureAwait(false);
            return response;
        }

        public async Task<HttpResponseMessage> GetFullPathwayJourney(JourneyStep[] steps, string startingPathwayId)
        {
            var request = new HttpRequestMessage { Content = new StringContent(JsonConvert.SerializeObject(steps), Encoding.UTF8, "application/json") };
            var response = await _restfulHelper.PostAsync(_configuration.GetDomainApiFullPathwayJourneyUrl(startingPathwayId), request).ConfigureAwait(false);
            return response;
        }

        public async Task<string> GetJustToBeSafeQuestionsNext(string pathwayId, IEnumerable<string> answeredQuestionIds, bool multipleChoice, string selectedQuestionId)
        {
            return await _restfulHelper.GetAsync(_configuration.GetDomainApiJustToBeSafeQuestionsNextUrl(pathwayId, answeredQuestionIds, multipleChoice, selectedQuestionId));
        }
    }

    public interface IQuestionService
    {
        Task<HttpResponseMessage> GetFullPathwayJourney(JourneyStep[] steps);
        Task<HttpResponseMessage> GetFullPathwayJourney(JourneyStep[] steps, string startingPathwayId);
        Task<string> GetQuestion(string id);
        Task<string> GetAnswersForQuestion(string id);
        Task<HttpResponseMessage> GetNextQuestion(string id, string nodeLabel,  string answer);
        Task<string> GetFirstQuestion(string pathwayId);
        Task<string> GetJustToBeSafeQuestionsFirst(string pathwayId);
        Task<string> GetJustToBeSafeQuestionsNext(string pathwayId, IEnumerable<string> answeredQuestionIds, bool multipleChoice, string selectedQuestionId);
    }
}