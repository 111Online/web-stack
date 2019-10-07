using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Azure;
using Microsoft.WindowsAzure.Storage;
using Microsoft.WindowsAzure.Storage.Table;
using Newtonsoft.Json;
using NHS111.Models.Models.Web;
using NHS111.Utils.Helpers;
using NHS111.Utils.RestTools;
using RestSharp;
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

        public async Task<FeedbackConfirmation> FeedbackBuilder(FeedbackViewModel feedback)
        {
            feedback.DateAdded = DateTime.Now;
            feedback.PageData = await _pageDateViewModelBuilder.PageDataBuilder(feedback.PageData);
            feedback.PageId = feedback.PageData.ToString();
            try {
                var request = new JsonRestRequest(_configuration.FeedbackAddFeedbackUrl, Method.POST);
                request.AddJsonBody(feedback);
                request.AddHeader("Authorization", _configuration.FeedbackAuthorization);
                var response = await _restClient.ExecuteTaskAsync(request);

                if (response.StatusCode == HttpStatusCode.OK || response.StatusCode == HttpStatusCode.Created) {
                    return FeedbackConfirmation.Success;
                }
            } catch {
                return FeedbackConfirmation.Error;
            }
            return FeedbackConfirmation.Error;
        }
    }

    public interface IFeedbackViewModelBuilder
    {
        Task<FeedbackConfirmation> FeedbackBuilder(FeedbackViewModel feedback);
    }
}
