
namespace NHS111.Models.Test.Models.Web.FromExternalServices {
    using System.Collections.Generic;
    using NHS111.Models.Models.Web;
    using NHS111.Models.Models.Web.FromExternalServices;
    using NUnit.Framework;

    [TestFixture]
    public class DosCheckCapacitySummaryResultTests {
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

        [Test]
        public void ResultListEmpty_WithResultsAndNoError_ReturnsFalse() {
            var sut = new DosCheckCapacitySummaryResult {
                Success = new SuccessObject<ServiceViewModel>() {
                    Services = new List<ServiceViewModel> {
                        new ServiceViewModel()
                    }
                }
            };
            Assert.False(sut.ResultListEmpty);
        }

        [Test]
        public void HasITKServices_WithCallbackServiceTypes_ReturnsTrue() {
            var sut = new DosCheckCapacitySummaryResult {
                Success = new SuccessObject<ServiceViewModel> {
                    Services = new List<ServiceViewModel> {
                        new ServiceViewModel {OnlineDOSServiceType = OnlineDOSServiceType.Callback},
                        new ServiceViewModel {OnlineDOSServiceType = OnlineDOSServiceType.Callback},
                        new ServiceViewModel {OnlineDOSServiceType = OnlineDOSServiceType.GoTo}
                }
                }
            };
            Assert.True(sut.HasITKServices);
        }

        [Test]
        public void HasITKServices_WithNoCallbackServiceTypes_ReturnsFalse() {
            var sut = new DosCheckCapacitySummaryResult {
                Success = new SuccessObject<ServiceViewModel> {
                    Services = new List<ServiceViewModel> {
                        new ServiceViewModel {OnlineDOSServiceType = OnlineDOSServiceType.PublicPhone}
                    }
                }
            };
            Assert.False(sut.HasITKServices);
        }

        [Test]
        public void HasITKServices_WithError_ReturnsFalse() {
            var sut = new DosCheckCapacitySummaryResult {
                Error = new ErrorObject()
            };
            Assert.False(sut.HasITKServices);
        }

        [Test]
        public void HasITKServices_WithNoServices_ReturnsFalse() {
            var sut = new DosCheckCapacitySummaryResult {
                Success = new SuccessObject<ServiceViewModel> {
                    Services = new List<ServiceViewModel> {
                        //empty list
                    }
                }
            };
            Assert.False(sut.HasITKServices);
        }
    }
}