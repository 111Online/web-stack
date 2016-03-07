using System.Xml.Serialization;

namespace NHS111.Models.Models.Web.ITK
{
    public class Value
    {
        [XmlElement("Value")]
        public string ItemValue { get; set; }
    }
}
