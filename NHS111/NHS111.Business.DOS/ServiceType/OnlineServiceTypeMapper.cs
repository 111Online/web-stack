using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Web;
using NHS111.Business.DOS.Configuration;
using NHS111.Models.Models.Web.CCG;
using NHS111.Models.Models.Web.FromExternalServices;
using RestSharp;
using BusinessModels = NHS111.Models.Models.Business;

namespace NHS111.Business.DOS.Service
{
    public class OnlineServiceTypeMapper : IOnlineServiceTypeMapper
    {
        private readonly IRestClient _restCCGApi;
        private readonly IConfiguration _configuration;

        public OnlineServiceTypeMapper(IRestClient restCCGApi, IConfiguration configuration)
        {
            _restCCGApi = restCCGApi;
            _configuration = configuration;
        }

        public async Task<List<BusinessModels.DosService>> Map(List<BusinessModels.DosService> resultsToMap, string postCode)
        {
            const string phoneText = "You must telephone this service before attending";
            const string goToText = "You can go straight to this service. You do not need to telephone beforehand";
            var localITKWhiteList = await PopulateLocalCCGITKServiceIdWhitelist(postCode);

            foreach (var service in resultsToMap)
            {
                if (localITKWhiteList.Contains(service.Id.ToString()))
                    service.OnlineDOSServiceType = OnlineDOSServiceType.Callback;
                else if (string.IsNullOrEmpty(service.ReferralText))
                    service.OnlineDOSServiceType = OnlineDOSServiceType.Unknown;
                else
                {
                    if (service.ReferralText.RemovePunctuationAndWhitespace().Contains(phoneText.RemovePunctuationAndWhitespace()) 
                        && !string.IsNullOrEmpty(service.ContactDetails))
                        service.OnlineDOSServiceType = OnlineDOSServiceType.PublicPhone;
                    else if (service.ReferralText.RemovePunctuationAndWhitespace().Contains(goToText.RemovePunctuationAndWhitespace()))
                        service.OnlineDOSServiceType = OnlineDOSServiceType.GoTo;
                    else
                        service.OnlineDOSServiceType = OnlineDOSServiceType.Unknown;
                }
            }

            return resultsToMap;
        }

        private async Task<ServiceListModel> PopulateLocalCCGITKServiceIdWhitelist(string postCode)
        {
            var response = await _restCCGApi.ExecuteTaskAsync<CCGDetailsModel>(
                    new RestRequest(string.Format(_configuration.CCGApiGetCCGByPostcode, postCode), Method.GET));

                if (response.StatusCode != HttpStatusCode.OK)
            throw new HttpException("CCG Service Error Response");

                if (response.Data != null && response.Data.ItkServiceIdWhitelist != null)
            return response.Data.ItkServiceIdWhitelist;

            return new ServiceListModel();
        }
    }

    public static class ReferralStringExtension
    {
        public static string RemovePunctuationAndWhitespace(this string str)
        {
            return new string(str.ToCharArray().Where(c => !char.IsPunctuation(c) && !char.IsWhiteSpace(c)).ToArray()).ToLower();
        }
    }

    public interface IOnlineServiceTypeMapper
    {
        Task<List<BusinessModels.DosService>> Map(List<BusinessModels.DosService> resultsToMap, string postCode);
    }
}
