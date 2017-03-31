using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NHS111.Models.Models.Web
{
    public class InformantViewModel
    {
        public string Forename { get; set; }
        public string Surname { get; set; }
        public string TelephoneNumber { get; set; }
        public bool IsInformant { get; set; }
    }
}
