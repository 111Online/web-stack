using Moq;
using NHS111.Business.DOS.EndpointFilter;
using NHS111.Models.Models.Business;
using NHS111.Models.Models.Business.Enums;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using IClock = NHS111.Models.Models.Web.Clock.IClock;

namespace NHS111.Business.DOS.Test
{
    [TestFixture()]
    public class ServiceAvailabilityProfileTests
    {
        private readonly IEnumerable<int> FILTERED_DOS_SERVICCE_TYPES = new List<int>();
        private readonly Mock<IProfileHoursOfOperation> _mockProfileHoursOfConfiguration = new Mock<IProfileHoursOfOperation>();
        private ServiceAvailabilityProfile _serviceAvailabilityProfile;

        public static IClock InHoursStartTime = new InHoursClock();

        public static IClock OutOfHoursClock = new OutOfHoursClock();

        private static readonly Tuple<DateTime, int> InHoursToOoHoursPeriodWeekday = new Tuple<DateTime, int>(InHoursStartTime.Now, 12 * 60);

        private static readonly Tuple<DateTime, int> OoHoursToOoHoursPeriodWeekday = new Tuple<DateTime, int>(OutOfHoursClock.Now, 60);

        private static readonly Tuple<DateTime, int> OoHoursToInHoursPeriodWeekday = new Tuple<DateTime, int>(OutOfHoursClock.Now, 12 * 60);

        private static readonly Tuple<DateTime, int> OoHoursToOoHoursTraverseInHoursPeriodWeekday = new Tuple<DateTime, int>(new DateTime(2016, 11, 17, 3, 0, 0), 18 * 60);

        [Test()]
        public void GetServiceAvailability_In_Hours_And_Timeframe_In_hours_Test()
        {
            _mockProfileHoursOfConfiguration
                .Setup(c => c.GetServiceTime(InHoursToOoHoursPeriodWeekday.Item1))
                .Returns(ProfileServiceTimes.InHours);

            _serviceAvailabilityProfile = new ServiceAvailabilityProfile(_mockProfileHoursOfConfiguration.Object, FILTERED_DOS_SERVICCE_TYPES);

            var result = _serviceAvailabilityProfile.GetServiceAvailability(InHoursToOoHoursPeriodWeekday.Item1, 60);
            Assert.AreEqual(DispositionTimePeriod.DispositionAndTimeFrameInHours, result);
        }

        [Test()]
        public void GetServiceAvailability_In_Hours_And_Timeframe_Out_of_hours_Test()
        {
            _mockProfileHoursOfConfiguration
                .Setup(c => c.GetServiceTime(InHoursToOoHoursPeriodWeekday.Item1))
                .Returns(ProfileServiceTimes.InHours);

            _mockProfileHoursOfConfiguration
                .Setup(c => c.GetServiceTime(InHoursToOoHoursPeriodWeekday.Item1.AddMinutes(InHoursToOoHoursPeriodWeekday.Item2)))
                .Returns(ProfileServiceTimes.OutOfHours);

            _serviceAvailabilityProfile = new ServiceAvailabilityProfile(_mockProfileHoursOfConfiguration.Object, FILTERED_DOS_SERVICCE_TYPES);

            var result = _serviceAvailabilityProfile.GetServiceAvailability(InHoursToOoHoursPeriodWeekday.Item1, InHoursToOoHoursPeriodWeekday.Item2);
            Assert.AreEqual(DispositionTimePeriod.DispositionInHoursTimeFrameOutOfHours, result);
        }

        [Test()]
        public void GetServiceAvailability_Out_of_Hours_And_Timeframe_In_hours_Test()
        {
            _mockProfileHoursOfConfiguration
                .Setup(c => c.GetServiceTime(OoHoursToInHoursPeriodWeekday.Item1))
                .Returns(ProfileServiceTimes.OutOfHours);

            _mockProfileHoursOfConfiguration
                .Setup(c => c.GetServiceTime(OoHoursToInHoursPeriodWeekday.Item1.AddMinutes(OoHoursToInHoursPeriodWeekday.Item2)))
                .Returns(ProfileServiceTimes.InHours);

            _serviceAvailabilityProfile = new ServiceAvailabilityProfile(_mockProfileHoursOfConfiguration.Object, FILTERED_DOS_SERVICCE_TYPES);

            var result = _serviceAvailabilityProfile.GetServiceAvailability(OoHoursToInHoursPeriodWeekday.Item1, OoHoursToInHoursPeriodWeekday.Item2);
            Assert.AreEqual(DispositionTimePeriod.DispositionOutOfHoursTimeFrameInHours, result);
        }

        [Test()]
        public void GetServiceAvailability_Out_of_Hours_And_Timeframe_Out_of_hours_traverses_in_hours_Test()
        {
            _mockProfileHoursOfConfiguration
               .Setup(c => c.GetServiceTime(OoHoursToOoHoursTraverseInHoursPeriodWeekday.Item1))
               .Returns(ProfileServiceTimes.OutOfHours);

            _mockProfileHoursOfConfiguration
                .Setup(c => c.GetServiceTime(OoHoursToOoHoursTraverseInHoursPeriodWeekday.Item1.AddMinutes(OoHoursToOoHoursTraverseInHoursPeriodWeekday.Item2)))
                .Returns(ProfileServiceTimes.OutOfHours);

            _mockProfileHoursOfConfiguration
                .Setup(c => c.ContainsInHoursPeriod(OoHoursToOoHoursTraverseInHoursPeriodWeekday.Item1, OoHoursToOoHoursTraverseInHoursPeriodWeekday.Item1.AddMinutes(OoHoursToOoHoursTraverseInHoursPeriodWeekday.Item2)))
                .Returns(true);

            _serviceAvailabilityProfile = new ServiceAvailabilityProfile(_mockProfileHoursOfConfiguration.Object, FILTERED_DOS_SERVICCE_TYPES);

            var result = _serviceAvailabilityProfile.GetServiceAvailability(OoHoursToOoHoursTraverseInHoursPeriodWeekday.Item1, OoHoursToOoHoursTraverseInHoursPeriodWeekday.Item2);
            Assert.AreEqual(DispositionTimePeriod.DispositionAndTimeFrameOutOfHoursTraversesInHours, result);
        }

        //only out of hours if starts in ooh and ends in ooh without traversing an entire in hours period or ends in shoulder time
        [Test()]
        public void GetServiceAvailability_Out_of_Hours_And_Timeframe_Out_of_hours_Test()
        {
            _mockProfileHoursOfConfiguration
               .Setup(c => c.GetServiceTime(OoHoursToOoHoursPeriodWeekday.Item1))
               .Returns(ProfileServiceTimes.OutOfHours);

            _mockProfileHoursOfConfiguration
                .Setup(c => c.GetServiceTime(OoHoursToOoHoursPeriodWeekday.Item1.AddMinutes(OoHoursToOoHoursPeriodWeekday.Item2)))
                .Returns(ProfileServiceTimes.OutOfHours);

            _mockProfileHoursOfConfiguration
                .Setup(c => c.ContainsInHoursPeriod(OoHoursToOoHoursPeriodWeekday.Item1, OoHoursToOoHoursPeriodWeekday.Item1.AddMinutes(OoHoursToOoHoursPeriodWeekday.Item2)))
                .Returns(false);

            _serviceAvailabilityProfile = new ServiceAvailabilityProfile(_mockProfileHoursOfConfiguration.Object, FILTERED_DOS_SERVICCE_TYPES);

            var result = _serviceAvailabilityProfile.GetServiceAvailability(OoHoursToOoHoursPeriodWeekday.Item1, OoHoursToOoHoursPeriodWeekday.Item2);
            Assert.AreEqual(DispositionTimePeriod.DispositionAndTimeFrameOutOfHours, result);
        }

        [Test()]
        public void GetServiceAvailability_Out_of_Hours_And_Timeframe_in_shoulder_Test()
        {
            _mockProfileHoursOfConfiguration
               .Setup(c => c.GetServiceTime(OoHoursToOoHoursPeriodWeekday.Item1))
               .Returns(ProfileServiceTimes.OutOfHours);

            _mockProfileHoursOfConfiguration
                .Setup(c => c.GetServiceTime(OoHoursToOoHoursPeriodWeekday.Item1.AddMinutes(OoHoursToOoHoursPeriodWeekday.Item2)))
                .Returns(ProfileServiceTimes.InHoursShoulder);

            _serviceAvailabilityProfile = new ServiceAvailabilityProfile(_mockProfileHoursOfConfiguration.Object, FILTERED_DOS_SERVICCE_TYPES);

            var result = _serviceAvailabilityProfile.GetServiceAvailability(OoHoursToOoHoursPeriodWeekday.Item1, OoHoursToOoHoursPeriodWeekday.Item2);
            Assert.AreEqual(DispositionTimePeriod.DispositionOutOfHoursTimeFrameInShoulder, result);
        }

        [Test()]
        public void GetServiceAvailability_In_shoulder_And_Timeframe_In_hours_Test()
        {
            var inShoulderDateTime = new DateTime(2016, 11, 17, 8, 20, 0);
            _mockProfileHoursOfConfiguration
               .Setup(c => c.GetServiceTime(inShoulderDateTime))
               .Returns(ProfileServiceTimes.InHoursShoulder);

            _mockProfileHoursOfConfiguration
                .Setup(c => c.GetServiceTime(inShoulderDateTime.AddMinutes(120)))
                .Returns(ProfileServiceTimes.InHours);

            _serviceAvailabilityProfile = new ServiceAvailabilityProfile(_mockProfileHoursOfConfiguration.Object, FILTERED_DOS_SERVICCE_TYPES);

            var result = _serviceAvailabilityProfile.GetServiceAvailability(inShoulderDateTime, 120);
            Assert.AreEqual(DispositionTimePeriod.DispositionInShoulderTimeFrameInHours, result);
        }

        [Test()]
        public void GetServiceAvailability_In_shoulder_And_Timeframe_In_Shoulder_Test()
        {
            var inShoulderDateTime = new DateTime(2016, 11, 17, 8, 20, 0);
            _mockProfileHoursOfConfiguration
               .Setup(c => c.GetServiceTime(inShoulderDateTime))
               .Returns(ProfileServiceTimes.InHoursShoulder);

            _mockProfileHoursOfConfiguration
                .Setup(c => c.GetServiceTime(inShoulderDateTime.AddMinutes(1440)))
                .Returns(ProfileServiceTimes.InHoursShoulder);

            _serviceAvailabilityProfile = new ServiceAvailabilityProfile(_mockProfileHoursOfConfiguration.Object, FILTERED_DOS_SERVICCE_TYPES);

            var result = _serviceAvailabilityProfile.GetServiceAvailability(inShoulderDateTime, 1440);
            Assert.AreEqual(DispositionTimePeriod.DispositionInShoulderTimeFrameInShoulder, result);
        }

        [Test()]
        public void GetServiceAvailability_In_shoulder_And_Timeframe_Out_of_hours_Test()
        {
            var inShoulderDateTime = new DateTime(2016, 11, 17, 8, 20, 0);
            _mockProfileHoursOfConfiguration
               .Setup(c => c.GetServiceTime(inShoulderDateTime))
               .Returns(ProfileServiceTimes.InHoursShoulder);

            _mockProfileHoursOfConfiguration
                .Setup(c => c.GetServiceTime(inShoulderDateTime.AddMinutes(720)))
                .Returns(ProfileServiceTimes.OutOfHours);

            _serviceAvailabilityProfile = new ServiceAvailabilityProfile(_mockProfileHoursOfConfiguration.Object, FILTERED_DOS_SERVICCE_TYPES);

            var result = _serviceAvailabilityProfile.GetServiceAvailability(inShoulderDateTime, 720);
            Assert.AreEqual(DispositionTimePeriod.DispositionInShoulderTimeFrameOutOfHours, result);
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
