using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NHS111.Models.Models.Business
{
    public class ProfileServiceTimes
    {
        public DayOfWeek Day { get; set; }
        public string InHoursStartTime { get; set; }
        public string InHoursEndTime { get; set; }
        public string OutOfHoursStartTime { get; set; }
        public string OutOfHoursEndTime { get; set; }
    }
}
