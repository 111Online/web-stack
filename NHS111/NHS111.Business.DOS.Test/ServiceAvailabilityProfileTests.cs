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

        public static IClock OutOfHoursClock = new OutOfHoursClock();

        private static readonly Tuple<DateTime, int> InHoursToOoHoursPeriodWeekday = new Tuple<DateTime, int>(InHoursStartTime.Now, 12*60);

        private static readonly Tuple<DateTime, int> OoHoursToOoHoursPeriodWeekday = new Tuple<DateTime, int>(OutOfHoursClock.Now, 60);

        private static readonly Tuple<DateTime, int> OoHoursToInHoursPeriodWeekday = new Tuple<DateTime, int>(OutOfHoursClock.Now, 60*60);

        [Test()]
        public void GetServiceAvailability_In_Hours_And_Timeframe_In_hours_Test()
        {
            var result = _serviceAvailabilityProfile.GetServiceAvailability(InHoursToOoHoursPeriodWeekday.Item1, 60);
            Assert.AreEqual(DispositionTimePeriod.DispositionAndTimeFrameInHours, result);
        }

        [Test()]
        public void GetServiceAvailability_In_Hours_And_Timeframe_Out_of_hours_Test()
        {
            var result = _serviceAvailabilityProfile.GetServiceAvailability(InHoursToOoHoursPeriodWeekday.Item1, InHoursToOoHoursPeriodWeekday.Item2);
            Assert.AreEqual(DispositionTimePeriod.DispositionInHoursTimeFrameOutOfHours, result);
        }

        [Test()]
        public void GetServiceAvailability_Out_of_Hours_And_Timeframe_In_hours_Test()
        {
            var result = _serviceAvailabilityProfile.GetServiceAvailability(OoHoursToInHoursPeriodWeekday.Item1, OoHoursToInHoursPeriodWeekday.Item2);
            Assert.AreEqual(DispositionTimePeriod.DispositionOutOfHoursTimeFrameInHours, result);
        }

        [Test()]
        public void GetServiceAvailability_Out_of_Hours_And_Timeframe_Out_of_hours_traverses_in_hours_Test()
        {
            var result = _serviceAvailabilityProfile.GetServiceAvailability(InHoursToOoHoursPeriodWeekday.Item1, OoHoursToOoHoursPeriodWeekday.Item2);
            Assert.AreEqual(DispositionTimePeriod.DispositionAndTimeFrameOutOfHoursTraversesInHours, result);
        }

        //only out of hours if starts in ooh and ends in ooh without traversing an entire in hours period or ends in shoulder time
        [Test()]
        public void GetServiceAvailability_Out_of_Hours_And_Timeframe_Out_of_hours_Test()
        {
            var result = _serviceAvailabilityProfile.GetServiceAvailability(OoHoursToOoHoursPeriodWeekday.Item1, OoHoursToOoHoursPeriodWeekday.Item2);
            Assert.AreEqual(DispositionTimePeriod.DispositionAndTimeFrameOutOfHours, result);
        }

        [Test()]
        public void GetServiceAvailability_Out_of_Hours_And_Timeframe_in_shoulder_Test()
        {
            var result = _serviceAvailabilityProfile.GetServiceAvailability(OoHoursToOoHoursPeriodWeekday.Item1, OoHoursToOoHoursPeriodWeekday.Item2);
            Assert.AreEqual(DispositionTimePeriod.DispositionOutOfHoursTimeFrameInShoulder, result);
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
