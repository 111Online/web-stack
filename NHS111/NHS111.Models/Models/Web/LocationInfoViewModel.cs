using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NHS111.Models.Models.Web
{
    public class LocationInfoViewModel
    {

        public PersonalDetailsAddressViewModel PatientCurrentAddress { get; set; }
        public string PatientHomeAddreess { get; set; }
        public bool HomeAddressSameAsCurrent { get; set; }
    }
}
