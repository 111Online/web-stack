using NHS111.Models.Models.Web;
using NUnit.Framework;


namespace NHS111.Models.Test.Models.Web.ServiceViewModelTests
{
    [TestFixture]
    public class AddressParsingTests
    {
        private string _testAddress = "11 Some Street, A street,The City, TS16 7TH";

        [Test]
        public void Service_MultipleAddressLines_Split()
        {
            var testService = new ServiceViewModel() { Address = _testAddress };

            Assert.AreEqual(4, testService.AddressLines.Count);
        }

        [Test]
        public void Service_MultipleAddressLines_Formatted_Correctly()
        {
            var testService = new ServiceViewModel() { Address = _testAddress };

            Assert.AreEqual("11 Some Street", testService.AddressLines[0]);
            Assert.AreEqual("A Street", testService.AddressLines[1]);
            Assert.AreEqual("The City", testService.AddressLines[2]);
            Assert.AreEqual("TS16 7TH", testService.AddressLines[3]);
        }

        [Test]
        public void Service_All_Caps_AddressLine_Formatted_Correctly()
        {
            var testService = new ServiceViewModel() { Address = "23 SHOUTY LANE, LOUDSVILLE , E15BH" };

            Assert.AreEqual("23 Shouty Lane", testService.AddressLines[0]);
            Assert.AreEqual("Loudsville", testService.AddressLines[1]);
            Assert.AreEqual("E15BH", testService.AddressLines[2]);
        }

        [Test]
        public void Service_MultipleAddressLines_No_Address()
        {
            var testService = new ServiceViewModel();
            Assert.AreEqual(0, testService.AddressLines.Count);
        }

        [Test]
        public void Service_MultipleAddressLines_Html_Encoding_in_Lines()
        {
            string testAdderess = "Test&#39;s Building, 11 Some Street, A street,The City, TS16 7TH";
            var testService = new ServiceViewModel() { Address = testAdderess };
            Assert.AreEqual("Test's Building", testService.AddressLines[0]);
        }

    }
}
