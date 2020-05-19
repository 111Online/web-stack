using NHS111.Business.DOS.WhiteListPopulator;
using NHS111.Models.Models.Web.FromExternalServices;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using BusinessModels = NHS111.Models.Models.Business;

namespace NHS111.Business.DOS.Service
{
    public class OnlineServiceTypeMapper : IOnlineServiceTypeMapper
    {
        private readonly IWhiteListPopulator _whiteListPopulator;

        public OnlineServiceTypeMapper(IWhiteListPopulator whiteListPopulator)
        {
            _whiteListPopulator = whiteListPopulator;
        }

        public async Task<List<BusinessModels.DosService>> Map(List<BusinessModels.DosService> resultsToMap, string postCode)
        {
            var localisedReferralWhiteList = await _whiteListPopulator.PopulateCCGWhitelist(postCode);

            foreach (var service in resultsToMap)
            {
                if (localisedReferralWhiteList.Contains(service.Id.ToString()))
                {
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
