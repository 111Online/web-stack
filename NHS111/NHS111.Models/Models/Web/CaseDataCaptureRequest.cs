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
        public string Gender { get; set; }
        public string PostCode { get; set; }
        public int Age { get; set; }
        public string Nino { get; set; }
        public string Phone { get; set; }
        public int DaysSinceSymptomsStarted { get; set; }
        public bool LiveAlone { get; set; }
        public int HouseHoldSize { get; set; }
    }
}
