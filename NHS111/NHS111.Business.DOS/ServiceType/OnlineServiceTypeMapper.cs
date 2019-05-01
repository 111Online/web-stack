using System.Collections.Generic;
using System.Linq;
using System.Net;
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
           var localisedReferralWhiteList = await PopulateCCGReferralServiceIdWhitelist(postCode);

            foreach (var service in resultsToMap)
            {
                if (localisedReferralWhiteList.Contains(service.Id.ToString())) { 
                    service.OnlineDOSServiceType = SetCallbackType(service.ReferralText, service.ContactDetails);
                }
                else if (string.IsNullOrEmpty(service.ReferralText))
                    service.OnlineDOSServiceType = OnlineDOSServiceType.Unknown;
                else
                {
                    if (ReferralTextIncluded(service.ReferralText, OnlineDOSServiceType.PublicPhone.ReferralText) && !string.IsNullOrEmpty(service.ContactDetails))
                        service.OnlineDOSServiceType = OnlineDOSServiceType.PublicPhone;
                    else if (ReferralTextIncluded(service.ReferralText, OnlineDOSServiceType.GoTo.ReferralText))
                        service.OnlineDOSServiceType = OnlineDOSServiceType.GoTo;
                    else
                        service.OnlineDOSServiceType = OnlineDOSServiceType.Unknown;
                }
            }

            return resultsToMap;
        }

        private OnlineDOSServiceType SetCallbackType(string serviceReferralText, string contactDetails)
        {
            if (ReferralTextIncluded(serviceReferralText, OnlineDOSServiceType.ReferRingAndGo.ReferralText))
            {
                return !string.IsNullOrEmpty(contactDetails) ? OnlineDOSServiceType.ReferRingAndGo : OnlineDOSServiceType.Unknown;
            }
            
            return OnlineDOSServiceType.Callback;
        }

        private bool ReferralTextIncluded(string serviceText, string typeText)
        {
            if (string.IsNullOrEmpty(serviceText) || string.IsNullOrEmpty(typeText)) return false;

            return serviceText.RemovePunctuationAndWhitespace().Contains(typeText.RemovePunctuationAndWhitespace());
        }

        private async Task<ServiceListModel> PopulateCCGReferralServiceIdWhitelist(string postCode)
        {
            var response = await _restCCGApi.ExecuteTaskAsync<CCGDetailsModel>(
                    new RestRequest(string.Format(_configuration.CCGApiGetCCGDetailsByPostcode, postCode), Method.GET));

                if (response.StatusCode != HttpStatusCode.OK)
            throw new HttpException("CCG Service Error Response");

                if (response.Data != null && response.Data.ReferralServiceIdWhitelist != null)
            return response.Data.ReferralServiceIdWhitelist;

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
