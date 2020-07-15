using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Runtime.Remoting.Messaging;
using NHS111.Models.Models.Web.Outcome;

namespace NHS111.Models.Models.Web
{
    public class RecommendedServiceViewModel : ServiceViewModel
    {
        private readonly IEnumerable<long> _callbackCASIdList = new List<long> { 130, 133, 137, 138 };

        public string ReasonText { get; set; }
        public DetailsViewModel Details { get; set; }

        public string GetServiceDisplayHtml()
        {
            var serviceDisplayHtml = GetServiceTypeAliasHtml();
            if (_callbackCASIdList.Contains(ServiceType.Id)) return serviceDisplayHtml;

            serviceDisplayHtml += GetServiceNameHtml();
            
            if (!ShouldShowAddress) return serviceDisplayHtml;

            serviceDisplayHtml += GetServiceAddressHtml();
            return serviceDisplayHtml;
        }

        public string GetOtherServicesServiceDisplayHtml()
        {
            var serviceDisplayHtml = GetServiceTypeAliasHtml();
            serviceDisplayHtml += GetOtherServicesSecondLineHtml();
            return serviceDisplayHtml;
        }

        private string GetServiceTypeAliasHtml()
        {
            return string.Format("<b class=\"service-details__alias\">{0}</b>", WebUtility.HtmlDecode(ServiceTypeAlias));
        }

        private string GetServiceNameHtml()
        {
            if ((ServiceType.Id == 25 || ShouldShowAddress) && string.IsNullOrEmpty(PublicNameOnly)) return string.Empty;

            return string.Format("<br />{0}", !string.IsNullOrEmpty(PublicNameOnly) ? WebUtility.HtmlDecode(PublicNameOnly) : WebUtility.HtmlDecode(PublicName));
        }

        private string GetServiceAddressHtml()
        {
            var fullAddressHtml = AddressLines.Where(address => !string.IsNullOrEmpty(address)).Aggregate(string.Empty, (current, address) => current + string.Format("{0}<br />", WebUtility.HtmlDecode(address)));
            return string.Format("<br />{0}", fullAddressHtml);
        }

        private string GetOtherServicesSecondLineHtml()
        {
            if (_callbackCASIdList.Contains(ServiceType.Id)) return string.Empty;

            if (ServiceType.Id == 25 && string.IsNullOrEmpty(PublicNameOnly)) return string.Empty;

            if (!ShouldShowAddress) 
                return string.Format("<br />{0}", WebUtility.HtmlDecode(PublicName));

            if (ShouldShowAddress && !string.IsNullOrEmpty(PublicNameOnly))
                return string.Format("<br />{0}", WebUtility.HtmlDecode(PublicName));

            var firstLineOfAddress = AddressLines.FirstOrDefault(a => !string.IsNullOrEmpty(a));
            return string.Format("<br />{0}", WebUtility.HtmlDecode(firstLineOfAddress));
        }
    }
}
