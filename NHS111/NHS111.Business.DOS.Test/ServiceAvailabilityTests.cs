using Moq;
using NHS111.Business.DOS.EndpointFilter;
using NHS111.Models.Models.Business.Enums;
using NUnit.Framework;
using System;

namespace NHS111.Business.DOS.Test
{
    [TestFixture()]
    public class ServiceAvailabilityTests
    {
        private readonly Mock<IServiceAvailabilityProfile> _mockServiceAvailabilityProfile = new Mock<IServiceAvailabilityProfile>();
        private ServiceAvailability _serviceAvailability;

        [Test()]
        public void IsOutOfHours_Dispo_And_Time_Frame_In_Hours_Test()
        {
            _mockServiceAvailabilityProfile
                .Setup(c => c.GetServiceAvailability(It.IsAny<DateTime>(), It.IsAny<int>()))
                .Returns(DispositionTimePeriod.DispositionAndTimeFrameInHours);

            _serviceAvailability = new ServiceAvailability(_mockServiceAvailabilityProfile.Object, It.IsAny<DateTime>(), It.IsAny<int>());

            Assert.IsFalse(_serviceAvailability.IsOutOfHours);
        }

        [Test()]
        public void IsOutOfHours_Dispo_In_Hours_And_Time_Frame_Out_Of_Hours_Test()
        {
            _mockServiceAvailabilityProfile
                .Setup(c => c.GetServiceAvailability(It.IsAny<DateTime>(), It.IsAny<int>()))
                .Returns(DispositionTimePeriod.DispositionInHoursTimeFrameOutOfHours);

            _serviceAvailability = new ServiceAvailability(_mockServiceAvailabilityProfile.Object, It.IsAny<DateTime>(), It.IsAny<int>());

            Assert.IsFalse(_serviceAvailability.IsOutOfHours);
        }

        [Test()]
        public void IsOutOfHours_Dispo_In_Hours_Shoulder_And_Time_Frame_Out_Of_Hours_Test()
        {
            _mockServiceAvailabilityProfile
                .Setup(c => c.GetServiceAvailability(It.IsAny<DateTime>(), It.IsAny<int>()))
                .Returns(DispositionTimePeriod.DispositionInShoulderTimeFrameOutOfHours);

            _serviceAvailability = new ServiceAvailability(_mockServiceAvailabilityProfile.Object, It.IsAny<DateTime>(), It.IsAny<int>());

            Assert.IsFalse(_serviceAvailability.IsOutOfHours);
        }

        [Test()]
        public void IsOutOfHours_Dispo_In_Hours_Shoulder_And_Time_Frame_In_Hours_Test()
        {
            _mockServiceAvailabilityProfile
                .Setup(c => c.GetServiceAvailability(It.IsAny<DateTime>(), It.IsAny<int>()))
                .Returns(DispositionTimePeriod.DispositionInShoulderTimeFrameInHours);

            _serviceAvailability = new ServiceAvailability(_mockServiceAvailabilityProfile.Object, It.IsAny<DateTime>(), It.IsAny<int>());

            Assert.IsFalse(_serviceAvailability.IsOutOfHours);
        }

        [Test()]
        public void IsOutOfHours_Dispo_In_Hours_Shoulder_And_Time_Frame_In_Hours_Shoulder_Test()
        {
            _mockServiceAvailabilityProfile
                .Setup(c => c.GetServiceAvailability(It.IsAny<DateTime>(), It.IsAny<int>()))
                .Returns(DispositionTimePeriod.DispositionInShoulderTimeFrameInShoulder);

            _serviceAvailability = new ServiceAvailability(_mockServiceAvailabilityProfile.Object, It.IsAny<DateTime>(), It.IsAny<int>());

            Assert.IsFalse(_serviceAvailability.IsOutOfHours);
        }

        [Test()]
        public void IsOutOfHours_Dispo_Out_Of_Hours_And_Time_Frame_In_Hours_Test()
        {
            _mockServiceAvailabilityProfile
                .Setup(c => c.GetServiceAvailability(It.IsAny<DateTime>(), It.IsAny<int>()))
                .Returns(DispositionTimePeriod.DispositionOutOfHoursTimeFrameInHours);

            _serviceAvailability = new ServiceAvailability(_mockServiceAvailabilityProfile.Object, It.IsAny<DateTime>(), It.IsAny<int>());

            Assert.IsFalse(_serviceAvailability.IsOutOfHours);
        }

        [Test()]
        public void IsOutOfHours_Dispo_Out_Of_Hours_And_Time_Frame_Out_Of_Hours_Test()
        {
            _mockServiceAvailabilityProfile
                .Setup(c => c.GetServiceAvailability(It.IsAny<DateTime>(), It.IsAny<int>()))
                .Returns(DispositionTimePeriod.DispositionAndTimeFrameOutOfHours);

            _serviceAvailability = new ServiceAvailability(_mockServiceAvailabilityProfile.Object, It.IsAny<DateTime>(), It.IsAny<int>());

            Assert.IsTrue(_serviceAvailability.IsOutOfHours);
        }

        [Test()]
        public void IsOutOfHours_Dispo_Out_Of_Hours_And_Time_Frame_Out_Of_Hours_Traverse_In_Hours_Period_Test()
        {
            _mockServiceAvailabilityProfile
                .Setup(c => c.GetServiceAvailability(It.IsAny<DateTime>(), It.IsAny<int>()))
                .Returns(DispositionTimePeriod.DispositionAndTimeFrameOutOfHoursTraversesInHours);

            _serviceAvailability = new ServiceAvailability(_mockServiceAvailabilityProfile.Object, It.IsAny<DateTime>(), It.IsAny<int>());

            Assert.IsFalse(_serviceAvailability.IsOutOfHours);
        }

        [Test()]
        public void IsOutOfHours_Dispo_Out_Of_Hours_And_Time_Frame_In_Shoulder_Test()
        {
            _mockServiceAvailabilityProfile
                .Setup(c => c.GetServiceAvailability(It.IsAny<DateTime>(), It.IsAny<int>()))
                .Returns(DispositionTimePeriod.DispositionOutOfHoursTimeFrameInShoulder);

            _serviceAvailability = new ServiceAvailability(_mockServiceAvailabilityProfile.Object, It.IsAny<DateTime>(), It.IsAny<int>());

            Assert.IsTrue(_serviceAvailability.IsOutOfHours);
        }
    }
}
