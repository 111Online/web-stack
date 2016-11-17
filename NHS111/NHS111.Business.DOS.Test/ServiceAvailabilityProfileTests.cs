using System;
using NHS111.Models.Models.Business.Enums;
using NHS111.Models.Models.Web.Clock;
using NUnit.Framework;

namespace NHS111.Business.DOS.Test
{
    [TestFixture()]
    public class ServiceAvailabilityProfileTests
    {
        private readonly ServiceAvailabilityProfile _serviceAvailabilityProfile = new ServiceAvailabilityProfile();
        public static IClock InHoursStartTime = new InHoursClock();  

        [Test()]
        // Test for dispo IH period and timeframe within IH perion
        public void GetServiceAvailability_In_Hours_And_Timeframe_In_hours_Test()
        {
            var result = _serviceAvailabilityProfile.GetServiceAvailability(InHoursStartTime.Now, 60);
            Assert.AreEqual(DispositionTimePeriod.DispositionAndTimeFrameInHours, result);
        }
    }

    public class InHoursClock : IClock
    {
        // Thurs 17 Nov 10am
        public DateTime Now { get { return new DateTime(2016, 11, 17, 10, 0, 0); } } 
    }
}
