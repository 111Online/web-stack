using System;
using NHS111.Models.Models.Business.Enums;
using NHS111.Models.Models.Web.Clock;
using NUnit.Framework;

namespace NHS111.Business.DOS.Test
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

        [Test()]
        // Test for dispo IH perios and timeframe within IH perion
        public void GetServiceAvailability_In_Hours_And_Timeframe_Out_of_hours_Test()
        {
            var result = _serviceAvailabilityProfile.GetServiceAvailability(InHoursToOOHoursPeriodWeekday.Item1, InHoursToOOHoursPeriodWeekday.Item2);
            Assert.AreEqual(ServiceAvailability.DispositionAndTimeFrameInHours, result);
        }
    public class InHoursClock : IClock
    {
        // Thurs 17 Nov 10am
        public DateTime Now { get { return new DateTime(2016, 11, 17, 10, 0, 0); } } 
    }
}
