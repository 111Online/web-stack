using System;
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
            var testService = new ServiceViewModel(){Address = _testAddress};

            Assert.AreEqual(4, testService.AddressLines.Count);
        }

        [Test]
        public void Service_MultipleAddressLines_Formatted_Correctly()
        {
            var testService = new ServiceViewModel() { Address = _testAddress };

            Assert.AreEqual("11 Some Street", testService.AddressLines[0]);
            Assert.AreEqual("A street", testService.AddressLines[1]);
            Assert.AreEqual("The City", testService.AddressLines[2]);
            Assert.AreEqual("TS16 7TH", testService.AddressLines[3]);
        }

        [Test]
        public void Service_MultipleAddressLines_No_Address()
        {
            var testService = new ServiceViewModel();
            Assert.AreEqual(0, testService.AddressLines.Count);
        }
    }
}
