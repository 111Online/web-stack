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
        public void AddressInfoViewModel_FormatAddress_AddressLine1SameAsHouseNumberAndThoroughfare()
        {
            AddressInfoViewModel addressInfoViewModel = new AddressInfoViewModel();
            addressInfoViewModel.AddressLine1 = "60 The Avenue";
            addressInfoViewModel.HouseNumber = "60";
            addressInfoViewModel.Thoroughfare = "The Avenue";

            Assert.AreEqual(addressInfoViewModel.AddressLine1, addressInfoViewModel.FormattedAddress);
        }

        [Test]
        public void AddressInfoViewModel_FormatAddress_AddressLine2SameAsHouseNumberAndThoroughfare()
        {
            AddressInfoViewModel addressInfoViewModel = new AddressInfoViewModel();
            addressInfoViewModel.AddressLine1 = "Building";
            addressInfoViewModel.AddressLine2 = "60 The Avenue";
            addressInfoViewModel.HouseNumber = "60";
            addressInfoViewModel.Thoroughfare = "The Avenue";

            Assert.AreEqual("Building, 60 The Avenue", addressInfoViewModel.FormattedAddress);
        }

        [Test]
        public void AddressInfoViewModel_FormatAddress_AddressLine3SameAsHouseNumberAndThoroughfare()
        {
            AddressInfoViewModel addressInfoViewModel = new AddressInfoViewModel();

            addressInfoViewModel.AddressLine1 = "Building";
            addressInfoViewModel.AddressLine2 = "Unit 2";
            addressInfoViewModel.AddressLine3 = "60 The Avenue";
            addressInfoViewModel.HouseNumber = "60";
            addressInfoViewModel.Thoroughfare = "The Avenue";

            Assert.AreEqual("Building, 60 The Avenue, Unit 2", addressInfoViewModel.FormattedAddress);
        }

        [Test]
        public void AddressInfoViewModel_FormatAddress_WardUsedInsteadOfThoroughFare()
        {
            AddressInfoViewModel addressInfoViewModel = new AddressInfoViewModel();
            addressInfoViewModel.HouseNumber = "60";
            addressInfoViewModel.Ward = "Allbrook";

            Assert.AreEqual("60 Allbrook", addressInfoViewModel.FormattedAddress);
        }

        [Test]
        public void AddressInfoViewModel_FormatAddress_HouseNumberAndNoThoroughFare()
        {
            AddressInfoViewModel addressInfoViewModel = new AddressInfoViewModel();
            addressInfoViewModel.HouseNumber = "60";

            Assert.AreEqual("60", addressInfoViewModel.FormattedAddress);
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
        public void AddressInfoViewModel_FormatAddress_AllAddressLinesAndHouseNumberThoroughfare()
        {
            AddressInfoViewModel addressInfoViewModel = new AddressInfoViewModel();
            addressInfoViewModel.HouseNumber = "61";
            addressInfoViewModel.Thoroughfare = "The Road";
            addressInfoViewModel.AddressLine1 = "60";
            addressInfoViewModel.AddressLine2 = "The Avenue";
            addressInfoViewModel.AddressLine3 = "Hamlet";
            addressInfoViewModel.City = "Southampton";
            addressInfoViewModel.County = "Hants";
            addressInfoViewModel.Postcode = "PO5tC0D3";

            Assert.AreEqual("60, 61 The Road, The Avenue, Hamlet, Southampton, Hants, PO5tC0D3", addressInfoViewModel.FormattedAddress);
        }
    }
}