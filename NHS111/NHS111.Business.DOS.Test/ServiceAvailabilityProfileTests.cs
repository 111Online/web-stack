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
        public static DateTime InHoursWeekdayStartTime = new DateTime(2016, 11, 17, 10,0,0); // Thurs 17 Nov 10am 
        public static DateTime OOHoursWeekdayStartTime = new DateTime(2016, 11, 17, 20, 0, 0); // Thurs 17 Nov 8pm 
        private static Tuple<DateTime, int> InHoursToInHoursPeriodWeekday = new Tuple<DateTime, int>(InHoursWeekdayStartTime, 60);
        private static Tuple<DateTime, int> InHoursToOOHoursPeriodWeekday = new Tuple<DateTime, int>(InHoursWeekdayStartTime, 12 * 60);
        private static Tuple<DateTime, int> OOHoursToOOHoursPeriodWeekday = new Tuple<DateTime, int>(OOHoursWeekdayStartTime, 60);
        private static Tuple<DateTime, int> OOHoursToInHoursPeriodWeekday = new Tuple<DateTime, int>(OOHoursWeekdayStartTime, 60 * 60);

        [Test()]
        // Test for dispo IH perios and timeframe within IH perion
        public void GetServiceAvailability_In_Hours_And_Timeframe_In_hours_Test()
        {
            var result = _serviceAvailabilityProfile.GetServiceAvailability(InHoursToInHoursPeriodWeekday.Item1, InHoursToInHoursPeriodWeekday.Item2);
            Assert.AreEqual(ServiceAvailability.DispositionAndTimeFrameInHours, result);
        }

        [Test()]
        // Test for dispo IH perios and timeframe within IH perion
        public void GetServiceAvailability_In_Hours_And_Timeframe_Out_of_hours_Test()
        {
            var result = _serviceAvailabilityProfile.GetServiceAvailability(InHoursToOOHoursPeriodWeekday.Item1, InHoursToOOHoursPeriodWeekday.Item2);
            Assert.AreEqual(ServiceAvailability.DispositionAndTimeFrameInHours, result);
        }
    }
}
