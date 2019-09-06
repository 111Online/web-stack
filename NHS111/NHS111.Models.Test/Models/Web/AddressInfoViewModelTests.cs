using NHS111.Models.Models.Web;
using NUnit.Framework;

namespace NHS111.Models.Test.Models.Web
{
    [TestFixture]
    public class AddressInfoViewModelTests
    {
        [Test]
        public void AddressInfoViewModel_FormatAddress_EmptyAddress()
        {
            AddressInfoViewModel addressInfoViewModel = new AddressInfoViewModel();
            Assert.AreEqual("", addressInfoViewModel.FormattedAddress);
        }

        [Test]
        public void AddressInfoViewModel_FormatAddress_AddressLine1()
        {
            AddressInfoViewModel addressInfoViewModel = new AddressInfoViewModel();
            addressInfoViewModel.AddressLine1 = "60 The Avenue";

            Assert.AreEqual(addressInfoViewModel.AddressLine1, addressInfoViewModel.FormattedAddress);
        }

        [Test]
        public void AddressInfoViewModel_FormatAddress_AddressLine2()
        {
            AddressInfoViewModel addressInfoViewModel = new AddressInfoViewModel();
            addressInfoViewModel.AddressLine1 = "Building";
            addressInfoViewModel.AddressLine2 = "60 The Avenue";

            Assert.AreEqual("Building, 60 The Avenue", addressInfoViewModel.FormattedAddress);
        }

        [Test]
        public void AddressInfoViewModel_FormatAddress_AddressLine3()
        {
            AddressInfoViewModel addressInfoViewModel = new AddressInfoViewModel();

            addressInfoViewModel.AddressLine1 = "Building";
            addressInfoViewModel.AddressLine2 = "Unit 2";
            addressInfoViewModel.AddressLine3 = "60 The Avenue";

            Assert.AreEqual("Building, Unit 2, 60 The Avenue", addressInfoViewModel.FormattedAddress);
        }

        [Test]
        public void AddressInfoViewModel_FormatAddress_HouseNumberAndThoroughfareIgnored()
        {
            AddressInfoViewModel addressInfoViewModel = new AddressInfoViewModel();
            addressInfoViewModel.HouseNumber = "60";
            addressInfoViewModel.Thoroughfare = "Allbrook";

            Assert.AreEqual("", addressInfoViewModel.FormattedAddress);
        }

        [Test]
        public void AddressInfoViewModel_FormatAddress_AllAddressLines()
        {
            AddressInfoViewModel addressInfoViewModel = new AddressInfoViewModel();
            addressInfoViewModel.AddressLine1 = "60";
            addressInfoViewModel.AddressLine2 = "The Avenue";
            addressInfoViewModel.AddressLine3 = "Hamlet";
            addressInfoViewModel.City = "Southampton";
            addressInfoViewModel.County = "Hants";
            addressInfoViewModel.Postcode = "PO5tC0D3";

            Assert.AreEqual("60, The Avenue, Hamlet, Southampton, Hants, PO5tC0D3", addressInfoViewModel.FormattedAddress);
        }

        [Test]
        public void AddressInfoViewModel_FormatAddress_AllAddressLinesAndCitySameAsCounty()
        {
            AddressInfoViewModel addressInfoViewModel = new AddressInfoViewModel();
            addressInfoViewModel.AddressLine1 = "60";
            addressInfoViewModel.AddressLine2 = "The Avenue";
            addressInfoViewModel.AddressLine3 = "Hamlet";
            addressInfoViewModel.City = "London";
            addressInfoViewModel.County = "London";
            addressInfoViewModel.Postcode = "PO5tC0D3";

            Assert.AreEqual("60, The Avenue, Hamlet, London, PO5tC0D3", addressInfoViewModel.FormattedAddress);
        }
    }
}