using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NHS111.Models.Models.Web
{
    public class CaseDataCaptureRequest
    {
        public string JourneyId { get; set; }
        public string PostCode { get; set; }
        public int Age { get; set; }
        public string Phone { get; set; }
        public string SymptomsStarted { get; set; }
        public bool LiveAlone { get; set; }
    }
}
