using System.Collections.Generic;
using NHS111.Models.Models.Business;

namespace NHS111.Business.DOS
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
