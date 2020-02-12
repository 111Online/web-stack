using System.Collections;
using System.Collections.Specialized;
using System.Web;
using System.Web.Configuration;
using Moq;
using NHS111.Models.Models.Domain;
using NHS111.Utils.Helpers;
using NHS111.Utils.Parser;
using NUnit.Framework;

namespace NHS111.Utils.Test.Helpers
{
    [TestFixture]
    public class BrowserInfoTests {
        
        [Test]
        public void CorrectBrowserInfo_Edge_Windows()
        {
            string userAgent = "Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/70.0.3538.102 Safari/537.36 Edge/18.18362";
            BrowserInfo browserInfo = getBrowserInfo(userAgent);

            Assert.IsTrue(browserInfo.Browser == "Edge");
            Assert.IsTrue(browserInfo.MajorVersionString == "18");
            Assert.IsTrue(browserInfo.Platform == "Windows");
            Assert.IsTrue(browserInfo.DeviceType == "Desktop");
        }
        
        [Test]
        public void CorrectBrowserInfo_Firefox_Windows()
        {
            string userAgent = "Mozilla/5.0 (Windows NT 10.0; Win64; x64; rv:54.0) Gecko/20100101 Firefox/54.0";
            BrowserInfo browserInfo = getBrowserInfo(userAgent);

            Assert.IsTrue(browserInfo.Browser == "Firefox");
            Assert.IsTrue(browserInfo.MajorVersionString == "54");
            Assert.IsTrue(browserInfo.Platform == "Windows");
            Assert.IsTrue(browserInfo.DeviceType == "Desktop");
        }

        [Test]
        public void CorrectBrowserInfo_Firefox_Linux()
        {
            string userAgent = "Mozilla/5.0 (X11; Linux x86_64; rv:70.0) Gecko/20100101 Firefox/70.0";
            BrowserInfo browserInfo = getBrowserInfo(userAgent);

            Assert.IsTrue(browserInfo.Browser == "Firefox");
            Assert.IsTrue(browserInfo.MajorVersionString == "70");
            Assert.IsTrue(browserInfo.Platform == "Linux");
            Assert.IsTrue(browserInfo.DeviceType == "Desktop");
        }

        [Test]
        public void CorrectBrowserInfo_Chrome_Mac()
        {
            string userAgent = "Mozilla/5.0 (Macintosh; Intel Mac OS X 10_14_5) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/78.0.3904.97 Safari/537.36";
            BrowserInfo browserInfo = getBrowserInfo(userAgent);

            Assert.IsTrue(browserInfo.Browser == "Chrome");
            Assert.IsTrue(browserInfo.MajorVersionString == "78");
            Assert.IsTrue(browserInfo.Platform == "Mac");
            Assert.IsTrue(browserInfo.DeviceType == "Desktop");
        }

        [Test]
        public void CorrectBrowserInfo_Safari_iPhone()
        {
            string userAgent = "Mozilla/5.0 (iPhone; CPU iPhone OS 13_2_3 like Mac OS X) AppleWebKit/605.1.15 (KHTML, like Gecko) Version/13.0.3 Mobile/15E148 Safari/604.1";
            BrowserInfo browserInfo = getBrowserInfo(userAgent);

            Assert.IsTrue(browserInfo.Browser == "Safari");
            Assert.IsTrue(browserInfo.MajorVersionString == "13");
            Assert.IsTrue(browserInfo.Platform == "iOS");
            Assert.IsTrue(browserInfo.DeviceType == "Mobile");
        }

        [Test]
        public void CorrectBrowserInfo_Safari_iPad()
        {
            string userAgent = "Mozilla/5.0 (iPad; CPU OS 12_1_4 like Mac OS X) AppleWebKit/605.1.15 (KHTML, like Gecko) Version/12.0 Mobile/15E148 Safari/604.1";
            BrowserInfo browserInfo = getBrowserInfo(userAgent);

            Assert.IsTrue(browserInfo.Browser == "Safari");
            Assert.IsTrue(browserInfo.MajorVersionString == "12");
            Assert.IsTrue(browserInfo.Platform == "iOS");
            Assert.IsTrue(browserInfo.DeviceType == "Tablet");
        }

        [Test]
        public void CorrectBrowserInfo_Chrome_Samsung_S8()
        {
            string userAgent = "Mozilla/5.0 (Linux; Android 7.0; SM-G950F) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/78.0.3904.62 Mobile Safari/537.36";
            BrowserInfo browserInfo = getBrowserInfo(userAgent);

            Assert.IsTrue(browserInfo.Browser == "Chrome");
            Assert.IsTrue(browserInfo.MajorVersionString == "78");
            Assert.IsTrue(browserInfo.Platform == "Android");
            Assert.IsTrue(browserInfo.DeviceType == "Mobile");
        }

        [Test]
        public void CorrectBrowserInfo_Chrome_Nexus_7()
        {
            string userAgent = "Mozilla/5.0 (Linux; Android 4.4; Nexus 7) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/78.0.3904.62 Safari/537.36";
            BrowserInfo browserInfo = getBrowserInfo(userAgent);

            Assert.IsTrue(browserInfo.Browser == "Chrome");
            Assert.IsTrue(browserInfo.MajorVersionString == "78");
            Assert.IsTrue(browserInfo.Platform == "Android");
            Assert.IsTrue(browserInfo.DeviceType == "Tablet");
        }

        [Test]
        public void CorrectBrowserInfo_Samsung_Internet()
        {
            string userAgent = "Mozilla/5.0 (Linux; Android 9; SAMSUNG SM-G955F Build/PPR1.180610.011) AppleWebKit/537.36 (KHTML, like Gecko) SamsungBrowser/9.0 Chrome/67.0.3396.87 Mobile Safari/537.36";
            BrowserInfo browserInfo = getBrowserInfo(userAgent);

            Assert.IsTrue(browserInfo.Browser == "Samsung Internet");
            Assert.IsTrue(browserInfo.MajorVersionString == "9");
            Assert.IsTrue(browserInfo.Platform == "Android");
            Assert.IsTrue(browserInfo.DeviceType == "Mobile");
        }

        [Test]
        public void CorrectBrowserInfo_Chrome_iOS()
        {
            string userAgent = "Mozilla/5.0 (iPhone; CPU iPhone OS 13_2 like Mac OS X) AppleWebKit/605.1.15 (KHTML, like Gecko) CriOS/78.0.3904.84 Mobile/15E148 Safari/604.1";
            BrowserInfo browserInfo = getBrowserInfo(userAgent);

            Assert.IsTrue(browserInfo.Browser == "Chrome");
            Assert.IsTrue(browserInfo.MajorVersionString == "78");
            Assert.IsTrue(browserInfo.Platform == "iOS");
            Assert.IsTrue(browserInfo.DeviceType == "Mobile");
        }

        [Test]
        public void CorrectBrowserInfo_IE11_Windows_Phone()
        {
            string userAgent = "Mozilla/5.0 (Mobile; Windows Phone 8.1; Android 4.0; ARM; Trident/7.0; Touch; rv:11.0; IEMobile/11.0; Microsoft; Virtual) like iPhone OS 7_0_3 Mac OS X AppleWebKit/537 (KHTML, like Gecko) Mobile Safari/537";
            BrowserInfo browserInfo = getBrowserInfo(userAgent);

            Assert.IsTrue(browserInfo.Browser == "Internet Explorer");
            Assert.IsTrue(browserInfo.MajorVersionString == "11");
            Assert.IsTrue(browserInfo.Platform == "Windows Phone");
            Assert.IsTrue(browserInfo.DeviceType == "Mobile");
        }

        [Test]
        public void CorrectBrowserInfo_IE11_Windows()
        {
            string userAgent = "Mozilla/5.0 (Windows NT 10.0; WOW64; Trident/7.0; rv:11.0) like Gecko";
            BrowserInfo browserInfo = getBrowserInfo(userAgent);

            Assert.IsTrue(browserInfo.Browser == "Internet Explorer");
            Assert.IsTrue(browserInfo.MajorVersionString == "11");
            Assert.IsTrue(browserInfo.Platform == "Windows");
            Assert.IsTrue(browserInfo.DeviceType == "Desktop");
        }

        private BrowserInfo getBrowserInfo(string userAgent ) {
            var browser = new HttpBrowserCapabilities
            {
                Capabilities = new Hashtable { { string.Empty, userAgent } }
            };
            var factory = new BrowserCapabilitiesFactory();
            factory.ConfigureBrowserCapabilities(new NameValueCollection(), browser);


            var request = new Mock<HttpRequestBase>();
            request.Setup(b => b.UserAgent).Returns(userAgent);
            request.Setup(b => b.Browser).Returns(new HttpBrowserCapabilitiesWrapper(browser));

            return new BrowserInfo(request.Object);
        }
    }
}