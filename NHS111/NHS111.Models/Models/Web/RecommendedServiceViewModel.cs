using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Runtime.Remoting.Messaging;
using NHS111.Models.Models.Web.FromExternalServices;
using NHS111.Models.Models.Web.Outcome;

namespace NHS111.Models.Models.Web
{
    public class RecommendedServiceViewModel : ServiceViewModel
    {
        private readonly IEnumerable<long> _callbackCASIdList = new List<long> { 130, 133, 137, 138 };
        private readonly long _oohServiceId = 25;

        public string ReasonText { get; set; }
        public DetailsViewModel Details { get; set; }

        public string GetServiceDisplayHtml()
        {
            var serviceDisplayHtml = GetServiceTypeAliasHtml();
            if (IsCallbackServiceOfferingCallback && !IsOohService) return serviceDisplayHtml;

            serviceDisplayHtml += GetServiceNameHtml();
            
            if (!ShouldShowAddress) return serviceDisplayHtml;

            serviceDisplayHtml += GetServiceAddressHtml();
            return serviceDisplayHtml;
        }

        public string GetOtherServicesServiceDisplayHtml()
        {
            var serviceDisplayHtml = GetServiceTypeAliasHtml();
            if (IsCallbackServiceNotOfferingCallback && !ShouldShowAddress) 
                return serviceDisplayHtml;

            serviceDisplayHtml += GetOtherServicesSecondLineHtml();
            return serviceDisplayHtml;
        }

        public bool ShouldShowServiceTypeDescription()
        {
            return !string.IsNullOrEmpty(ServiceTypeDescription) && !IsCallbackServiceNotOfferingCallback;
        }

        public bool ShouldShowOtherServicesServiceTypeDescription(bool isFromOtherServices)
        {
            return !string.IsNullOrEmpty(ServiceTypeDescription) && isFromOtherServices && !IsCallbackServiceNotOfferingCallback;
        }

        private string GetServiceTypeAliasHtml()
        {
            var serviceTypeAlias = IsCallbackServiceNotOfferingCallback ? PublicName : ServiceTypeAlias;
            return string.Format("<b class=\"service-details__alias\">{0}</b>", WebUtility.HtmlDecode(serviceTypeAlias));
        }

        private string GetServiceNameHtml()
        {
            if (IsCallbackServiceNotOfferingCallback) return string.Empty;

            if ((IsOohService || ShouldShowAddress) && string.IsNullOrEmpty(PublicNameOnly)) return string.Empty;

            return string.Format("<br />{0}", !string.IsNullOrEmpty(PublicNameOnly) ? WebUtility.HtmlDecode(PublicNameOnly) : WebUtility.HtmlDecode(PublicName));
        }

        private string GetServiceAddressHtml()
        {
            var fullAddressHtml = AddressLines.Where(address => !string.IsNullOrEmpty(address)).Aggregate(string.Empty, (current, address) => current + string.Format("{0}<br />", WebUtility.HtmlDecode(address)));
            return string.Format("<br />{0}", fullAddressHtml);
        }

        private string GetOtherServicesSecondLineHtml()
        {
            if (IsCASCallbackServiceWithNoAddress) return string.Empty;

            if (IsOohServiceWithCallbackAndNoPublicName) return string.Empty;

            if (IsNotACallbackServiceWithPublicName) return string.Format("<br />{0}", WebUtility.HtmlDecode(PublicName));

            if(!ShouldShowAddress) return string.Format("<br />{0}", WebUtility.HtmlDecode(PublicName));

            var firstLineOfAddress = AddressLines.FirstOrDefault(a => !string.IsNullOrEmpty(a));
            return string.Format("<br />{0}", WebUtility.HtmlDecode(firstLineOfAddress));
        }

        public bool IsOohService
        {
            get { return ServiceType.Id.Equals(_oohServiceId); }
        }

        public bool IsCASCallbackServiceWithNoAddress
        {
            get { return IsCallbackService && !IsOohService && !ShouldShowAddress; }
        }

        public bool IsOohServiceWithCallback
        {
            get { return ServiceType.Id.Equals(_oohServiceId) && OnlineDOSServiceType.Equals(OnlineDOSServiceType.Callback); }
        }

        public bool IsOohServiceWithCallbackAndNoPublicName
        {
            get { return IsOohServiceWithCallback && string.IsNullOrEmpty(PublicNameOnly); }
        }

        public bool IsCallbackService
        {
            get { return _callbackCASIdList.Contains(ServiceType.Id) || IsOohService; }
        }

        public bool IsCallbackServiceOfferingCallback
        {
            get { return IsCallbackService && OnlineDOSServiceType.Equals(OnlineDOSServiceType.Callback); }
        }

        public bool IsCallbackServiceNotOfferingCallback
        {
            get { return IsCallbackService && !OnlineDOSServiceType.Equals(OnlineDOSServiceType.Callback); }
        }

        public bool IsNotACallbackServiceWithPublicName
        {
            get {  return !string.IsNullOrEmpty(PublicNameOnly) && !IsCallbackService; }
        }
    }
}
