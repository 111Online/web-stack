using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NHS111.Business.DOS;
using NHS111.Models.Models.Business;
using NUnit.Framework;
namespace NHS111.Business.DOS.Tests
{
    [TestFixture()]
    public class ServiceAvailabilityProfileTests
    {
        private ServiceAvailabilityProfile _serviceAvailabilityProfile = new ServiceAvailabilityProfile();
        public static DateTime InHoursStartTime = new DateTime(2016, 11, 17, 10,0,0); // Thurs 17 Nov 10am 

        [Test()]
        // Test for dispo IH perios and timeframe within IH perion
        public void GetServiceAvailability_In_Hours_And_Timeframe_In_hours_Test()
        {

            var result = _serviceAvailabilityProfile.GetServiceAvailability(InHoursStartTime, 60);
            Assert.AreEqual(ServiceAvailability.DispositionAndTimeFrameInHours, result);
        }
    }
}
