using System.Collections.Generic;
using System.Linq;
using System.Net;
using NHS111.Models.Models.Web.Outcome;

namespace NHS111.Models.Models.Web
{
    public class RecommendedServiceViewModel : ServiceViewModel
    {
        private readonly IEnumerable<long> _callbackCASIdList = new List<long> { 130, 133, 137 };

        public string ReasonText { get; set; }
        public DetailsViewModel Details { get; set; }

        public string GetServiceNameDisplayHtml()
        {
            var nameDisplayHtml = string.Format("<b class=\"service-details__alias\">{0}</b>", WebUtility.HtmlDecode(ServiceTypeAlias));
            if (!_callbackCASIdList.Contains(ServiceType.Id) && !string.IsNullOrEmpty(PublicName))
                nameDisplayHtml += string.Format("<br />{0}", WebUtility.HtmlDecode(PublicName));

            if (!ShouldShowAddress) return nameDisplayHtml;

            nameDisplayHtml += "<br />";
            foreach (var address in AddressLines)
            {
                if (!string.IsNullOrEmpty(address))
                    nameDisplayHtml += string.Format("{0}<br />", WebUtility.HtmlDecode(address));
            }
            return nameDisplayHtml;
        }
        
    }
}
