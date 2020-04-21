using Moq;
using NHS111.Features.Clock;
using NUnit.Framework;
using System;
using System.Collections.Specialized;
using System.Web;

namespace NHS111.Features.Test
{
    [TestFixture]
    public class DOSSpecifyDispoTimeFeatureTests
    {
        private readonly Mock<HttpRequestBase> _mockRequest = new Mock<HttpRequestBase>();
        private Mock<IClock> _mockClock;
        private readonly string _dosSearchDateTimeKeyname = "dossearchdatetime";
        private readonly DateTime _currentTime = new DateTime(2019, 1, 1, 12, 15, 0);

        [SetUp]
        public void SetUp()
        {
            _mockClock = new Mock<IClock>();
            _mockClock.SetupGet(c => c.Now).Returns(_currentTime);
        }

        [Test]
        public void Is_enabled_returns_false_by_default()
        {
            var sut = new DOSSpecifyDispoTimeFeature();
            Assert.IsFalse(sut.IsEnabled);
        }

        [Test]
        public void Missing_query_string_datetime_returns_false()
        {
            _mockRequest.SetupGet(r => r.QueryString)
                .Returns(new NameValueCollection());
            var sut = new DOSSpecifyDispoTimeFeature();
            Assert.IsFalse(sut.HasDate(_mockRequest.Object));
        }

        [Test]
        public void Empty_datetime_returns_false()
        {
            _mockRequest.SetupGet(r => r.QueryString)
                .Returns(new NameValueCollection() { { _dosSearchDateTimeKeyname, string.Empty } });
            var sut = new DOSSpecifyDispoTimeFeature();
            Assert.IsFalse(sut.HasDate(_mockRequest.Object));
        }

        [Test]
        [ExpectedException(typeof(ArgumentException))]
        public void Invalid_datetime_throws_exception()
        {
            _mockRequest.SetupGet(r => r.QueryString)
                .Returns(new NameValueCollection() { { _dosSearchDateTimeKeyname, "12345465" } });
            var sut = new DOSSpecifyDispoTimeFeature();
            sut.HasDate(_mockRequest.Object);
        }

        [Test]
        [ExpectedException(typeof(ArgumentException))]
        public void Invalid_datetime_format_throws_exception()
        {
            _mockRequest.SetupGet(r => r.QueryString)
                .Returns(new NameValueCollection() { { _dosSearchDateTimeKeyname, "01/01/2019 12:00:00" } });
            var sut = new DOSSpecifyDispoTimeFeature();
            sut.HasDate(_mockRequest.Object);
        }

        [Test]
        public void Valid_datetime_returns_true()
        {
            _mockRequest.SetupGet(r => r.QueryString)
                .Returns(new NameValueCollection() { { _dosSearchDateTimeKeyname, _currentTime.ToString("yyyy-MM-dd HH:mm") } });
            var sut = new DOSSpecifyDispoTimeFeature();
            Assert.IsTrue(sut.HasDate(_mockRequest.Object));
        }

        [Test]
        public void Valid_datetime_past_returns_same_datetime()
        {
            var date = _currentTime.AddMinutes(-1);
            _mockRequest.SetupGet(r => r.QueryString)
                .Returns(new NameValueCollection() { { _dosSearchDateTimeKeyname, date.ToString("yyyy-MM-dd HH:mm") } });
            var sut = new DOSSpecifyDispoTimeFeature(_mockClock.Object);
            Assert.AreEqual(date, sut.GetDosSearchDateTime(_mockRequest.Object));
        }

        [Test]
        public void Greater_than_one_min_in_past_datetime_returns_datetime_now()
        {
            var date = _currentTime.AddMinutes(-1).AddSeconds(-1);
            _mockRequest.SetupGet(r => r.QueryString)
                .Returns(new NameValueCollection() { { _dosSearchDateTimeKeyname, date.ToString("yyyy-MM-dd HH:mm") } });
            var sut = new DOSSpecifyDispoTimeFeature(_mockClock.Object);
            Assert.AreEqual(_mockClock.Object.Now, sut.GetDosSearchDateTime(_mockRequest.Object));
        }

        [Test]
        public void Valid_datetime_future_returns_same_datetime()
        {
            var date = _currentTime.AddYears(1);
            _mockRequest.SetupGet(r => r.QueryString)
                .Returns(new NameValueCollection() { { _dosSearchDateTimeKeyname, date.ToString("yyyy-MM-dd HH:mm") } });
            var sut = new DOSSpecifyDispoTimeFeature(_mockClock.Object);
            Assert.AreEqual(date, sut.GetDosSearchDateTime(_mockRequest.Object));
        }

        [Test]
        public void Greater_than_one_year_in_future_datetime_returns_datetime_now()
        {
            var date = _currentTime.AddYears(1).AddMinutes(1);
            _mockRequest.SetupGet(r => r.QueryString)
                .Returns(new NameValueCollection() { { _dosSearchDateTimeKeyname, date.ToString("yyyy-MM-dd HH:mm") } });
            var sut = new DOSSpecifyDispoTimeFeature(_mockClock.Object);
            Assert.AreEqual(_mockClock.Object.Now, sut.GetDosSearchDateTime(_mockRequest.Object));
        }
    }
}
