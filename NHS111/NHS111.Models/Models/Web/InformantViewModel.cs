using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NHS111.Models.Models.Web
{
    public class InformantViewModel
    {
        public InformantViewModel()
        {
            Type = "Self";
        }

        public string Forename { get; set; }
        public string Surname { get; set; }
        public string TelephoneNumber { get; set; }
        public string Type { get; set; }
    }
}
