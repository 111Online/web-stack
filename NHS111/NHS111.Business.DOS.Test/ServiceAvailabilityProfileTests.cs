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

        public static IClock OutOfHursClock = new OutOfHoursClock();

        private static Tuple<DateTime, int> InHoursToInHoursPeriodWeekday = new Tuple<DateTime, int>(InHoursStartTime.Now, 60);

        private static Tuple<DateTime, int> InHoursToOOHoursPeriodWeekday = new Tuple<DateTime, int>(InHoursStartTime.Now, 12*60);

        private static Tuple<DateTime, int> OOHoursToOOHoursPeriodWeekday = new Tuple<DateTime, int>(OutOfHursClock.Now, 60);

        private static Tuple<DateTime, int> OOHoursToInHoursPeriodWeekday = new Tuple<DateTime, int>(OutOfHursClock.Now, 60*60);

        [Test()]
        // Test for dispo IH period and timeframe within IH perion
        public void GetServiceAvailability_In_Hours_And_Timeframe_In_hours_Test()
        {
            var result = _serviceAvailabilityProfile.GetServiceAvailability(InHoursStartTime.Now, 60);
            Assert.AreEqual(DispositionTimePeriod.DispositionAndTimeFrameInHours, result);
        }
        
        [Test()]
        // Test for dispo IH perios and timeframe within IH perion
        public void GetServiceAvailability_In_Hours_And_Timeframe_Out_of_hours_Test()
        {
            var result = _serviceAvailabilityProfile.GetServiceAvailability(InHoursToOOHoursPeriodWeekday.Item1, InHoursToOOHoursPeriodWeekday.Item2);
            Assert.AreEqual(DispositionTimePeriod.DispositionAndTimeFrameInHours, result);
        }
    }

    public class InHoursClock : IClock
    {
        // Thurs 17 Nov 10am
        public DateTime Now { get { return new DateTime(2016, 11, 17, 10, 0, 0); } } 
    }

    public class OutOfHoursClock : IClock
    {
        // Thurs 17 Nov 8pm
        public DateTime Now { get { return new DateTime(2016, 11, 17, 20, 0, 0); } }
    }
}
