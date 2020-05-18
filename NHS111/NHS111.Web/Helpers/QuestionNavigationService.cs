using Newtonsoft.Json;
using NHS111.Models.Models.Domain;
using NHS111.Models.Models.Web;
using NHS111.Utils.RestTools;
using NHS111.Web.Presentation.Builders;
using NHS111.Web.Presentation.Configuration;
using RestSharp;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;

namespace NHS111.Web.Helpers
{
    public class QuestionNavigationService : IQuestionNavigiationService
    {
        private readonly IJourneyViewModelBuilder _journeyViewModelBuilder;
        private readonly IConfiguration _configuration;
        private readonly ILoggingRestClient _restClientBusinessApi;
        private readonly IViewRouter _viewRouter;

        public QuestionNavigationService(IJourneyViewModelBuilder journeyViewModelBuilder, IConfiguration configuration, ILoggingRestClient restClientBusinessApi, IViewRouter viewRouter)
        {
            _journeyViewModelBuilder = journeyViewModelBuilder;
            _configuration = configuration;
            _restClientBusinessApi = restClientBusinessApi;
            _viewRouter = viewRouter;
        }

        public async Task<JourneyResultViewModel> NextQuestion(QuestionViewModel model, ControllerContext context)
        {
            var nextModel = await GetNextJourneyViewModel(model).ConfigureAwait(false);
            var viewRouter = _viewRouter.Build(nextModel, context);
            return viewRouter;
        }

        public async Task<QuestionWithAnswers> GetNextNode(QuestionViewModel model)
        {
            var answer = JsonConvert.DeserializeObject<Answer>(model.SelectedAnswer);
            var serialisedState = HttpUtility.UrlEncode(model.StateJson);
            var request = new JsonRestRequest(_configuration.GetBusinessApiNextNodeUrl(model.PathwayId, model.NodeType, model.Id, serialisedState, true), Method.POST);
            request.AddJsonBody(answer.Title);
            var response = await _restClientBusinessApi.ExecuteAsync<QuestionWithAnswers>(request).ConfigureAwait(false);
            return response.Data;
        }

        public async Task<JourneyViewModel> GetNextJourneyViewModel(QuestionViewModel model)
        {
            var nextNode = await GetNextNode(model).ConfigureAwait(false);
            return await _journeyViewModelBuilder.Build(model, nextNode).ConfigureAwait(false);
        }
    }

    public interface IQuestionNavigiationService
    {
        Task<JourneyResultViewModel> NextQuestion(QuestionViewModel model, ControllerContext context);
        Task<QuestionWithAnswers> GetNextNode(QuestionViewModel model);
        Task<JourneyViewModel> GetNextJourneyViewModel(QuestionViewModel model);
    }


}