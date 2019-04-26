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

        public async Task<IEnumerable<FeedbackViewModel>> ViewFeedbackBuilder(int pageNumber = 0, int pageSize = 1000)
        {
            var storageAccount = CloudStorageAccount.Parse(CloudConfigurationManager.GetSetting("StorageConnectionString"));
            var tableClient = storageAccount.CreateCloudTableClient();
            var table = tableClient.GetTableReference(CloudConfigurationManager.GetSetting("StorageTableReference"));

            var query = new TableQuery<FeedbackViewModel>();
            var results = await table.ExecuteQueryAsync(query);

            if (!results.Any()) return new List<FeedbackViewModel>();

            var orderedResults = results.OrderByDescending(f => f.DateAdded);
            var feedback = (pageNumber > 0) ? orderedResults.Skip((pageNumber - 1) * pageSize).Take(pageSize) : orderedResults.Take(pageSize);
            return feedback;
        }
    }

    public interface IFeedbackViewModelBuilder
    {
        Task<FeedbackConfirmation> FeedbackBuilder(FeedbackViewModel feedback);
        Task<IEnumerable<FeedbackViewModel>> ViewFeedbackBuilder(int pageNumber = 0, int pageSize = 1000);
    }
}
