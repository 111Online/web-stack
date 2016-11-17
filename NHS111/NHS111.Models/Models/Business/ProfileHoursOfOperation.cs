using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NHS111.Models.Models.Business
{
    public class ProfileHoursOfOperation
    {
        private List<ProfileServiceTimes> ServiceTimes { get;  set; }

        public ProfileHoursOfOperation()
        {
            ServiceTimes = new List<ProfileServiceTimes>();
        }

        public void Add(ProfileServiceTimes profileServiceTimes)
        {
            if (profileServiceTimes == null) return;

            ServiceTimes.Add(profileServiceTimes);
        }
    }
}
