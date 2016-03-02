using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NHS111.Models.Models.Web.ITK
{
    public class GpPractice
    {
        public string Name { get; set; }
        public Address Address { get; set; }
        public string Telephone { get; set; }
        public string ODS { get; set; }
    }
}
