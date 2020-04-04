using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NHS111.Models.Models.Web
{
    public class SendSmsOutcomeViewModel : OutcomeViewModel
    {
        private string _mobileNumber;
        public string MobileNumber {
            get { return _mobileNumber; }
            set { _mobileNumber = value.Replace(" ", ""); }
        }
        public int Age { get; set; }
        public int SymptomsStartedDaysAgo { get; set; }
        public bool LivesAlone { get; set; }
    }
}
