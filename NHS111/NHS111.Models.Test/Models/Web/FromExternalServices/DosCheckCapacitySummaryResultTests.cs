
namespace NHS111.Models.Test.Models.Web.FromExternalServices
{
    using System.Collections.Generic;
    using NHS111.Models.Models.Web;
    using NHS111.Models.Models.Web.FromExternalServices;
    using NUnit.Framework;

    [TestFixture]
    public class DosCheckCapacitySummaryResultTests
    {
        [Test]
        public void ResultListEmpty_WithoutErrorOrSuccess_ReturnsTrue() {
            var sut = new DosCheckCapacitySummaryResult();
            Assert.True(sut.ResultListEmpty);
        }

        [Test]
        public void ResultListEmpty_WithSuccessObjectOnly_ReturnsTrue() {
            var sut = new DosCheckCapacitySummaryResult {
                Success = new SuccessObject<ServiceViewModel>()
            };
            Assert.True(sut.ResultListEmpty);
        }

        [Test]
        public void ResultListEmpty_WithEmptyServiceList_ReturnsTrue() {
            var sut = new DosCheckCapacitySummaryResult {
                Success = new SuccessObject<ServiceViewModel>() {
                    Services = new List<ServiceViewModel>()
                }
            };
            Assert.True(sut.ResultListEmpty);
        }

        [Test]
        public void ResultListEmpty_WithError_ReturnsTrue() {
            var sut = new DosCheckCapacitySummaryResult {
                Error = new ErrorObject()
            };
            Assert.True(sut.ResultListEmpty);
        }


    }
}
