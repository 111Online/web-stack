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
        public string Id { get; set; }
        public string Name { get; set; }
        public string PostCode { get; set; }
        public string OdsCode { get; set; }
        public string Ccg { get; set; }
        public string Stp { get; set; }
    }
}
