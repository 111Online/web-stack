using System;
using System.Collections.Generic;
using System.Dynamic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;
using System.Xml.Serialization;

namespace NHS111.Models.Models.Web.ITK
{
    public class ServiceDetails
    {
        [XmlElement("id")]
        public string Id { get; set; }
        [XmlElement("name")]
        public string Name { get; set; }
        [XmlElement("odsCode")]
        public string OdsCode { get; set; }
        [XmlElement("contactDetails")]
        public ContactDetails ContactDetails { get; set; }
        [XmlElement("address")]
        public string Address { get; set; }
        [XmlElement("postcode")]
        public string PostCode { get; set; }
    }
}
