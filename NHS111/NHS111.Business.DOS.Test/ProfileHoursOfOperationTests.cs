using Moq;
using NHS111.Business.DOS.Configuration;
using NHS111.Business.DOS.EndpointFilter;
using NHS111.Models.Models.Business;
using NodaTime;
using NUnit.Framework;
using System;
namespace NHS111.Business.DOS.Tests
{
    [TestFixture()]
    public class ProfileHoursOfOperationTests
    {
        private readonly Mock<IConfiguration> _mockConfiguration = new Mock<IConfiguration>();
        private ProfileHoursOfOperation _profileHoursOfOperation;
        private DentalProfileHoursOfOperation _dentalProfileHoursOfOperation;

        [SetUp]
        public void SetupConfig()
        {
            var workingDayPrimaryCareInHoursEndTime = new LocalTime(18, 0);
            var workingDayPrimaryCareInHoursShoulderEndTime = new LocalTime(9, 0);
            var workingDayPrimaryCareInHoursStartTime = new LocalTime(8, 0);
            _profileHoursOfOperation = new ProfileHoursOfOperation(workingDayPrimaryCareInHoursStartTime, workingDayPrimaryCareInHoursShoulderEndTime, workingDayPrimaryCareInHoursEndTime, _mockConfiguration.Object);

            var workingDayDentalInHoursEndTime = new LocalTime(22, 0);
            var workingDayDentalInHoursShoulderEndTime = new LocalTime(7, 30);
            var workingDayDentalInHoursStartTime = new LocalTime(7, 30);
            _dentalProfileHoursOfOperation = new DentalProfileHoursOfOperation(workingDayDentalInHoursStartTime, workingDayDentalInHoursShoulderEndTime, workingDayDentalInHoursEndTime, _mockConfiguration.Object);
        }

        [Test()]
        public void GetServiceTime_Weekend_Test()
        {
            var result = _profileHoursOfOperation.GetServiceTime(new DateTime(2016, 11, 19));
            Assert.AreEqual(ProfileServiceTimes.OutOfHours, result);
        }

        [Test()]
        public void GetServiceTime_Bank_Holiday_Test()
        {
            var result = _profileHoursOfOperation.GetServiceTime(new DateTime(2016, 12, 26));
            Assert.AreEqual(ProfileServiceTimes.OutOfHours, result);
        }

        [Test()]
        public void GetServiceTime_Weekday_In_Hours_Test()
        {
            var result = _profileHoursOfOperation.GetServiceTime(new DateTime(2016, 11, 18, 10, 30, 0));
            Assert.AreEqual(ProfileServiceTimes.InHours, result);
        }

        [Test()]
        public void GetServiceTime_Weekday_Out_Of_Hours_Before_Test()
        {
            var result = _profileHoursOfOperation.GetServiceTime(new DateTime(2016, 11, 18, 3, 30, 0));
            Assert.AreEqual(ProfileServiceTimes.OutOfHours, result);
        }

        [Test()]
        public void GetServiceTime_Weekday_Out_Of_Hours_After_Test()
        {
            var result = _profileHoursOfOperation.GetServiceTime(new DateTime(2016, 11, 18, 23, 30, 0));
            Assert.AreEqual(ProfileServiceTimes.OutOfHours, result);
        }

        [Test()]
        public void GetServiceTime_Weekday_In_Shoulder_Time_Test()
        {
            var result = _profileHoursOfOperation.GetServiceTime(new DateTime(2016, 11, 18, 8, 30, 0));
            Assert.AreEqual(ProfileServiceTimes.InHoursShoulder, result);
        }

        [Test()]
        public void GetServiceTime_Weekday_Start_In_Shoulder_Limit_Test()
        {
            var result = _profileHoursOfOperation.GetServiceTime(new DateTime(2016, 11, 18, 8, 0, 0));
            Assert.AreEqual(ProfileServiceTimes.InHoursShoulder, result);
        }

        [Test()]
        public void GetServiceTime_Weekday_Start_In_Hours_Limit_Test()
        {
            var result = _profileHoursOfOperation.GetServiceTime(new DateTime(2016, 11, 18, 9, 0, 0));
            Assert.AreEqual(ProfileServiceTimes.InHours, result);
        }

        [Test()]
        public void GetServiceTime_Weekday_End_Shoulder_Limit_Test()
        {
            var result = _profileHoursOfOperation.GetServiceTime(new DateTime(2016, 11, 18, 8, 59, 59));
            Assert.AreEqual(ProfileServiceTimes.InHoursShoulder, result);
        }

        [Test()]
        public void GetServiceTime_Weekday_End_In_Hours_Limit_Test()
        {
            var result = _profileHoursOfOperation.GetServiceTime(new DateTime(2016, 11, 18, 17, 59, 59));
            Assert.AreEqual(ProfileServiceTimes.InHours, result);
        }

        [Test()]
        public void GetServiceTime_Weekday_Start_Out_Of_Hours_Limit_Test()
        {
            var result = _profileHoursOfOperation.GetServiceTime(new DateTime(2016, 11, 18, 0, 0, 0));
            Assert.AreEqual(ProfileServiceTimes.OutOfHours, result);
        }

        [Test()]
        public void GetServiceTime_Weekday_End_Out_Of_Hours_Limit_Test()
        {
            var result = _profileHoursOfOperation.GetServiceTime(new DateTime(2016, 11, 18, 23, 59, 59));
            Assert.AreEqual(ProfileServiceTimes.OutOfHours, result);
        }

        [Test()]
        public void ContainsInHoursPeriod_Weekend_False_Test()
        {
            var result = _profileHoursOfOperation.ContainsInHoursPeriod(new DateTime(2016, 11, 19, 3, 0, 0), new DateTime(2016, 11, 19, 20, 0, 0));
            Assert.IsFalse(result);
        }

        [Test()]
        public void ContainsInHoursPeriod_BankHoliday_False_Test()
        {
            var result = _profileHoursOfOperation.ContainsInHoursPeriod(new DateTime(2016, 12, 25, 8, 0, 0), new DateTime(2016, 11, 26, 15, 0, 0));
            Assert.IsFalse(result);
        }

        [Test()]
        public void ContainsInHoursPeriod_WorkingDay_False_Test()
        {
            var result = _profileHoursOfOperation.ContainsInHoursPeriod(new DateTime(2016, 11, 18, 3, 0, 0), new DateTime(2016, 11, 18, 7, 0, 0));
            Assert.IsFalse(result);
        }

        [Test()]
        public void ContainsInHoursPeriod_WorkingDay_True_Test()
        {
            var result = _profileHoursOfOperation.ContainsInHoursPeriod(new DateTime(2016, 11, 18, 3, 0, 0), new DateTime(2016, 11, 18, 20, 0, 0));
            Assert.IsTrue(result);
        }

        [Test()]
        public void ContainsInHoursPeriod_WorkingDay_To_Weekend_False_Test()
        {
            var result = _profileHoursOfOperation.ContainsInHoursPeriod(new DateTime(2016, 11, 18, 21, 0, 0), new DateTime(2016, 11, 19, 21, 0, 0));
            Assert.IsFalse(result);
        }

        [Test()]
        public void ContainsInHoursPeriod_WorkingDay_To_Weekend_True_Test()
        {
            var result = _profileHoursOfOperation.ContainsInHoursPeriod(new DateTime(2016, 11, 18, 10, 0, 0), new DateTime(2016, 11, 19, 21, 0, 0));
            Assert.IsTrue(result);
        }

        [Test()]
        public void ContainsInHoursPeriod_Weekend_To_WorkingDay_False_Test()
        {
            var result = _profileHoursOfOperation.ContainsInHoursPeriod(new DateTime(2016, 11, 19, 10, 0, 0), new DateTime(2016, 11, 21, 5, 0, 0));
            Assert.IsFalse(result);
        }

        [Test()]
        public void ContainsInHoursPeriod_Weekend_To_WorkingDay_True_Test()
        {
            var result = _profileHoursOfOperation.ContainsInHoursPeriod(new DateTime(2016, 11, 19, 10, 0, 0), new DateTime(2016, 11, 21, 21, 0, 0));
            Assert.IsTrue(result);
        }

        [Test()]
        public void ContainsInHoursPeriod_Bank_Holiday_Weekend_False_Test()
        {
            var result = _profileHoursOfOperation.ContainsInHoursPeriod(new DateTime(2016, 8, 27, 10, 0, 0), new DateTime(2016, 8, 29, 17, 0, 0));
            Assert.IsFalse(result);
        }


        [Test()]
        public void GetServiceTime_Dental_Weekend_InHours_Test()
        {
            var result = _dentalProfileHoursOfOperation.GetServiceTime(new DateTime(2016, 11, 19, 10, 0, 0));
            Assert.AreEqual(ProfileServiceTimes.InHours, result);
        }

        [Test()]
        public void GetServiceTime_Dental_Weekday_Out_Of_Hours_Before_Test()
        {
            var result = _dentalProfileHoursOfOperation.GetServiceTime(new DateTime(2016, 11, 18, 3, 30, 0));
            Assert.AreEqual(ProfileServiceTimes.OutOfHours, result);
        }

        [Test()]
        public void GetServiceTime_Dental_Weekday_End_In_Hours_Limit_Test()
        {
            var result = _dentalProfileHoursOfOperation.GetServiceTime(new DateTime(2016, 11, 18, 21, 59, 59));
            Assert.AreEqual(ProfileServiceTimes.InHours, result);
        }

        [Test()]
        public void GetServiceTime_Dental_Weekend_Out_Of_Hours_Test()
        {
            var result = _dentalProfileHoursOfOperation.GetServiceTime(new DateTime(2016, 11, 19, 07, 29, 59));
            Assert.AreEqual(ProfileServiceTimes.OutOfHours, result);
        }


        [Test()]
        public void GetServiceTime_Test_BankHoliday_Out_Of_Hours_Limit_Test()
        {
            _mockConfiguration.Setup(s => s.TestPublicHolidayDates).Returns("29-05-2018");
            var result = _profileHoursOfOperation.GetServiceTime(new DateTime(2018, 05, 29, 11, 00, 00));
            Assert.AreEqual(ProfileServiceTimes.OutOfHours, result);
        }
    }
}
