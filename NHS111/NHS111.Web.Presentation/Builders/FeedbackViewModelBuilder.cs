using System.Net;
using System.Net.Configuration;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using NHS111.Models.Models.Domain;
using NHS111.Models.Models.Web;
using NHS111.Models.Models.Web.FromExternalServices;
using NHS111.Utils.Helpers;
using IConfiguration = NHS111.Web.Presentation.Configuration.IConfiguration;

namespace NHS111.Web.Presentation.Builders
{
    public class FeedbackViewModelBuilder : IFeedbackViewModelBuilder
    {
        private IRestfulHelper _restfulHelper;
        private readonly IConfiguration _configuration;

        public FeedbackViewModelBuilder(IRestfulHelper restfulHelper, IConfiguration configuration)
        {
            _restfulHelper = restfulHelper;
            _configuration = configuration;
        }

        public async Task<FeedbackConfirmation> FeedbackBuilder(Feedback feedback)
        {
            var model = new FeedbackConfirmation();

            var request = new HttpRequestMessage { Content = new StringContent(JsonConvert.SerializeObject(feedback), Encoding.UTF8, "application/json") };
            var response = await _restfulHelper.PostAsync(_configuration.FeedbackAddFeedbackUrl, request);

            if (response.StatusCode == HttpStatusCode.OK)
            {
                model.Message = "Feedback Submitted";
                model.Success = true;
            }
            else
            {
                model.Message = "Feedback did not submit, please try again later";
                model.Success = false;
            }

            return model;
        }
    }

    public interface IFeedbackViewModelBuilder
    {
        Task<FeedbackConfirmation> FeedbackBuilder(Feedback feedback);
    }
}
