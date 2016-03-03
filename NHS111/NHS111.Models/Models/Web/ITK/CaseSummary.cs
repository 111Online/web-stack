using System.Collections.Generic;
using System.Xml;
using System.Xml.Serialization;

namespace NHS111.Models.Models.Web.ITK
{
    public class CaseSummary
    {
        [XmlElement("SummaryItem")]
        public List<SummaryItem> SummaryItem { get; set; }
    }
}
