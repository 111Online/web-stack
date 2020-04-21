using NHS111.Models.Models.Web;
using NHS111.Utils.RestTools;
using RestSharp;
using System;
using System.Net;
using System.Threading.Tasks;
using IConfiguration = NHS111.Web.Presentation.Configuration.IConfiguration;

namespace NHS111.Web.Presentation.Builders
{
    public class FeedbackViewModelBuilder : IFeedbackViewModelBuilder
    {
        private IRestClient _restClient;
        private readonly IConfiguration _configuration;
        private readonly IPageDataViewModelBuilder _pageDateViewModelBuilder;

        public FeedbackViewModelBuilder(IRestClient restClient, IConfiguration configuration, IPageDataViewModelBuilder pageDataViewModelBuilder)
        {
            _restClient = restClient;
            _configuration = configuration;
            _pageDateViewModelBuilder = pageDataViewModelBuilder;
        }

        public async Task<FeedbackResultViewModel> FeedbackResultBuilder(FeedbackViewModel feedback)
        {
            feedback.DateAdded = DateTime.Now;
            feedback.PageData = await _pageDateViewModelBuilder.PageDataBuilder(feedback.PageData);
            feedback.PageId = feedback.PageData.ToString();
            try
            {
                var request = new JsonRestRequest(_configuration.FeedbackAddFeedbackUrl, Method.POST);
                request.AddJsonBody(feedback);
                request.AddHeader("Authorization", _configuration.FeedbackAuthorization);
                var response = await _restClient.ExecuteTaskAsync(request);

                if (response.StatusCode == HttpStatusCode.OK || response.StatusCode == HttpStatusCode.Created)
                {
                    return new FeedbackConfirmationResultViewModel(feedback);
                }
            }
            catch
            {
                return new FeedbackErrorResultViewModel(feedback);
            }
            return new FeedbackErrorResultViewModel(feedback);
        }
    }

    public interface IFeedbackViewModelBuilder
    {
        Task<FeedbackResultViewModel> FeedbackResultBuilder(FeedbackViewModel feedback);
    }
}
