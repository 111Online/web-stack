using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Moq;
using NHS111.Business.DOS;
using NHS111.Business.DOS.Configuration;
using NHS111.Models.Models.Business;
using NodaTime;
using NUnit.Framework;
namespace NHS111.Business.DOS.Tests
{
    [TestFixture()]
    public class ProfileHoursOfOperationTests
    {
        private Mock<IConfiguration> _mockConfiguration = new Mock<IConfiguration>();
        private ProfileHoursOfOperation _profileHoursOfOperation;

        [SetUp]
        private void SetupCondfig()
        {

            _mockConfiguration.SetupGet(c => c.WorkingDayInHoursEndTime).Returns(new LocalTime(18,0));
            _mockConfiguration.SetupGet(c => c.WorkingDayInHoursShoulderEndTime).Returns(new LocalTime(9, 0));
            _mockConfiguration.SetupGet(c => c.WorkingDayInHoursStartTime).Returns(new LocalTime(8, 0));
            _profileHoursOfOperation = new ProfileHoursOfOperation(_mockConfiguration.Object);
        }

        [Test()]
        public void GeServiceTime_Weekend_Test()
        {
            var result = _profileHoursOfOperation.GeServiceTime(new DateTime(2016, 11, 19));
            Assert.AreEqual(ProfileServiceTimes.OutOfHours, result);
        }
    }
}
