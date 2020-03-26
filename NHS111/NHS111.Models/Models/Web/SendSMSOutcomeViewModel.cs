using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NHS111.Models.Models.Web
{
    public class SendSmsOutcomeViewModel : OutcomeViewModel
    {
        public string MobileNumber { get; set; }
        public int Age { get; set; }
        public DateTime SymptomsStartedDate { get; set; }
        public bool LivesAlone { get; set; }
    }
}
