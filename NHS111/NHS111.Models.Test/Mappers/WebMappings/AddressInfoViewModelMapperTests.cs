using AutoMapper;
using NHS111.Models.Models.Web;
using NHS111.Models.Models.Web.FromExternalServices;
using NUnit.Framework;

namespace NHS111.Models.Test.Mappers.WebMappings
{
    [TestFixture()]
    public class AddressInfoViewModelMapperTests
    {
        private string TEST_HOUSE_NUMBER = "33";
        private string TEST_ADDRESS_LINE_1 = "Some street";
        private string TEST_ADDRESS_LINE_2 = "SomeWhere ";
        private string TEST_CITY_INPUT = "SOME PLACE";//data feed we use has all caps cities (not sure why)
        private string TEST_CITY_OUTPUT = "Some Place";
        private string TEST_COUNTY = "Some time";
        private string TEST_POSTCODE = "XX1 2YY";
        private string TEST_UDPRN = "12345";

        [TestFixtureSetUp()]
        public void InitializeAddressInfoViewModelMapper()
        {
            Mapper.Initialize(m => m.AddProfile<NHS111.Models.Mappers.WebMappings.AddressInfoViewModelMapper>());
        }

        [Test()]
        public void Configuration_IsValid_Test()
        {
            Mapper.AssertConfigurationIsValid();
        }

        [Test()]
        public void FromLocationResultToAddressInfoConverter_Null_AddressLines_Test()
        {
            var locationResult = new LocationResult()
            {
                HouseNumber = TEST_HOUSE_NUMBER,
                PostTown = TEST_CITY_INPUT,
                AdministrativeArea = TEST_COUNTY,
                Postcode = TEST_POSTCODE,
                StreetDescription = "Test street desc"
            };

            var result = Mapper.Map<AddressInfoViewModel>(locationResult);
            Assert.AreEqual(TEST_HOUSE_NUMBER, result.HouseNumber);
            Assert.AreEqual("Test street desc", result.AddressLine1);
            Assert.AreEqual(string.Empty, result.AddressLine2);
            Assert.AreEqual(TEST_CITY_OUTPUT, result.City);
            Assert.AreEqual(TEST_COUNTY, result.County);
            Assert.AreEqual(TEST_POSTCODE, result.Postcode);
        }

        [Test()]
        public void FromLocationResultToAddressInfoConverter_Single_AddressLine_Test()
        {
            var locationResult = new LocationResult()
            {
                HouseNumber = TEST_HOUSE_NUMBER,
                AddressLines = new[] { TEST_ADDRESS_LINE_1 },
                PostTown = TEST_CITY_INPUT,
                AdministrativeArea = TEST_COUNTY,
                Postcode = TEST_POSTCODE
            };

            var result = Mapper.Map<AddressInfoViewModel>(locationResult);
            Assert.AreEqual(TEST_HOUSE_NUMBER, result.HouseNumber);
            Assert.AreEqual(TEST_ADDRESS_LINE_1, result.AddressLine1);
            Assert.AreEqual(string.Empty, result.AddressLine2);
            Assert.AreEqual(TEST_CITY_OUTPUT, result.City);
            Assert.AreEqual(TEST_COUNTY, result.County);
            Assert.AreEqual(TEST_POSTCODE, result.Postcode);
        }


        [Test()]
        public void FromLocationResultToAddressInfoConverter_Test()
        {
            var locationResult = new LocationResult()
            {
                HouseNumber = TEST_HOUSE_NUMBER,
                AddressLines = new [] { TEST_ADDRESS_LINE_1, TEST_ADDRESS_LINE_2 },
                PostTown = TEST_CITY_INPUT,
                AdministrativeArea = TEST_COUNTY,
                Postcode = TEST_POSTCODE,
                UDPRN = TEST_UDPRN
            };

            var result = Mapper.Map<AddressInfoViewModel>(locationResult);
            Assert.AreEqual(TEST_HOUSE_NUMBER, result.HouseNumber);
            Assert.AreEqual(TEST_ADDRESS_LINE_1, result.AddressLine1);
            Assert.AreEqual(TEST_ADDRESS_LINE_2, result.AddressLine2);
            Assert.AreEqual(TEST_CITY_OUTPUT, result.City);
            Assert.AreEqual(TEST_COUNTY, result.County);
            Assert.AreEqual(TEST_POSTCODE, result.Postcode);
            Assert.AreEqual(TEST_UDPRN, result.UDPRN);
        }

        [Test()]
        public void FromLocationResultToAddressInfoConverter_HouseNumberWithLetters()
        {
            string houseNumber = string.Empty;
            const string buildingName = "145C";
            const string streetDescription = "The Road";
            string[] addressLines = { "145C", "145 The Road" };

            var locationResult = new LocationResult()
            {
                HouseNumber = houseNumber,
                BuildingName = buildingName,
                AddressLines = addressLines,
                StreetDescription = streetDescription,
                PostTown = TEST_CITY_INPUT,
                AdministrativeArea = TEST_COUNTY,
                Postcode = TEST_POSTCODE,
                UDPRN = TEST_UDPRN
            };

            var result = Mapper.Map<AddressInfoViewModel>(locationResult);
            Assert.AreEqual(houseNumber, result.HouseNumber);
            Assert.AreEqual(buildingName + " " + streetDescription, result.AddressLine1);
            Assert.AreEqual(string.Empty, result.AddressLine2);
            Assert.AreEqual(TEST_CITY_OUTPUT, result.City);
            Assert.AreEqual(TEST_COUNTY, result.County);
            Assert.AreEqual(TEST_POSTCODE, result.Postcode);
            Assert.AreEqual(TEST_UDPRN, result.UDPRN);
        }

        [Test()]
        public void FromLocationResultToAddressInfoConverter_HouseNumberWithLettersAndSpace()
        {
            string houseNumber = string.Empty;
            const string buildingName = "145 C";
            const string streetDescription = "The Road";
            string[] addressLines = { "145 C", "145 The Road" };

            var locationResult = new LocationResult()
            {
                HouseNumber = houseNumber,
                BuildingName = buildingName,
                AddressLines = addressLines,
                StreetDescription = streetDescription,
                PostTown = TEST_CITY_INPUT,
                AdministrativeArea = TEST_COUNTY,
                Postcode = TEST_POSTCODE,
                UDPRN = TEST_UDPRN
            };

            var result = Mapper.Map<AddressInfoViewModel>(locationResult);
            Assert.AreEqual(houseNumber, result.HouseNumber);
            Assert.AreEqual(buildingName + " " + streetDescription, result.AddressLine1);
            Assert.AreEqual(string.Empty, result.AddressLine2);
            Assert.AreEqual(TEST_CITY_OUTPUT, result.City);
            Assert.AreEqual(TEST_COUNTY, result.County);
            Assert.AreEqual(TEST_POSTCODE, result.Postcode);
            Assert.AreEqual(TEST_UDPRN, result.UDPRN);
        }

        [Test()]
        public void FromLocationResultToAddressInfoConverter_HouseNumberInBuildingNameField()
        {
            string houseNumber = string.Empty;
            const string buildingName = "145";
            const string streetDescription = "The Road";
            string[] addressLines = { "145", "145 The Road" };

            var locationResult = new LocationResult()
            {
                HouseNumber = houseNumber,
                BuildingName = buildingName,
                AddressLines = addressLines,
                StreetDescription = streetDescription,
                PostTown = TEST_CITY_INPUT,
                AdministrativeArea = TEST_COUNTY,
                Postcode = TEST_POSTCODE,
                UDPRN = TEST_UDPRN
            };

            var result = Mapper.Map<AddressInfoViewModel>(locationResult);
            Assert.AreEqual(houseNumber, result.HouseNumber);
            Assert.AreEqual(buildingName + " " + streetDescription, result.AddressLine1);
            Assert.AreEqual(string.Empty, result.AddressLine2);
            Assert.AreEqual(TEST_CITY_OUTPUT, result.City);
            Assert.AreEqual(TEST_COUNTY, result.County);
            Assert.AreEqual(TEST_POSTCODE, result.Postcode);
            Assert.AreEqual(TEST_UDPRN, result.UDPRN);
        }

        [Test()]
        public void FromLocationResultToAddressInfoConverter_HouseNumberIsRange()
        {
            string houseNumber = string.Empty;
            const string buildingName = "145-150";
            const string streetDescription = "The Road";
            string[] addressLines = { "145-150", "145-150 The Road" };

            var locationResult = new LocationResult()
            {
                HouseNumber = houseNumber,
                BuildingName = buildingName,
                AddressLines = addressLines,
                StreetDescription = streetDescription,
                PostTown = TEST_CITY_INPUT,
                AdministrativeArea = TEST_COUNTY,
                Postcode = TEST_POSTCODE,
                UDPRN = TEST_UDPRN
            };

            var result = Mapper.Map<AddressInfoViewModel>(locationResult);
            Assert.AreEqual(houseNumber, result.HouseNumber);
            Assert.AreEqual(buildingName + " " + streetDescription, result.AddressLine1);
            Assert.AreEqual(string.Empty, result.AddressLine2);
            Assert.AreEqual(TEST_CITY_OUTPUT, result.City);
            Assert.AreEqual(TEST_COUNTY, result.County);
            Assert.AreEqual(TEST_POSTCODE, result.Postcode);
            Assert.AreEqual(TEST_UDPRN, result.UDPRN);
        }

        [Test()]
        public void FromLocationResultToAddressInfoConverter_OfficeBuilding()
        {
            string houseNumber = string.Empty;
            const string buildingName = "Head Office 5";
            const string streetDescription = "The Road";
            string[] addressLines = { "Head Office", "145 The Road" };

            var locationResult = new LocationResult()
            {
                HouseNumber = houseNumber,
                BuildingName = buildingName,
                AddressLines = addressLines,
                StreetDescription = streetDescription,
                PostTown = TEST_CITY_INPUT,
                AdministrativeArea = TEST_COUNTY,
                Postcode = TEST_POSTCODE,
                UDPRN = TEST_UDPRN
            };

            var result = Mapper.Map<AddressInfoViewModel>(locationResult);
            Assert.AreEqual(houseNumber, result.HouseNumber);
            Assert.AreEqual(buildingName, result.AddressLine1);
            Assert.AreEqual(streetDescription, result.AddressLine2);
            Assert.AreEqual(TEST_CITY_OUTPUT, result.City);
            Assert.AreEqual(TEST_COUNTY, result.County);
            Assert.AreEqual(TEST_POSTCODE, result.Postcode);
            Assert.AreEqual(TEST_UDPRN, result.UDPRN);
        }

        [Test()]
        public void FromLocationResultToAddressInfoConverter_NoCity()
        {
            string administrativeArea = "Leeds";
            string postTown = string.Empty;

            var locationResult = new LocationResult()
            {
                HouseNumber = TEST_HOUSE_NUMBER,
                AddressLines = new[] { TEST_ADDRESS_LINE_1, TEST_ADDRESS_LINE_2 },
                PostTown = postTown,
                AdministrativeArea = administrativeArea,
                Postcode = TEST_POSTCODE,
                UDPRN = TEST_UDPRN
            };

            var result = Mapper.Map<AddressInfoViewModel>(locationResult);
            Assert.AreEqual(TEST_HOUSE_NUMBER, result.HouseNumber);
            Assert.AreEqual(TEST_ADDRESS_LINE_1, result.AddressLine1);
            Assert.AreEqual(TEST_ADDRESS_LINE_2, result.AddressLine2);
            Assert.AreEqual(administrativeArea, result.City);
            Assert.AreEqual(string.Empty, result.County);
            Assert.AreEqual(TEST_POSTCODE, result.Postcode);
            Assert.AreEqual(TEST_UDPRN, result.UDPRN);
        }

        [Test()]
        public void FromLocationResultToAddressInfoConverter_SameCityAndCounty()
        {
            string administrativeArea = "Leeds";
            string postTown = "Leeds";

            var locationResult = new LocationResult()
            {
                HouseNumber = TEST_HOUSE_NUMBER,
                AddressLines = new[] { TEST_ADDRESS_LINE_1, TEST_ADDRESS_LINE_2 },
                PostTown = postTown,
                AdministrativeArea = administrativeArea,
                Postcode = TEST_POSTCODE,
                UDPRN = TEST_UDPRN
            };

            var result = Mapper.Map<AddressInfoViewModel>(locationResult);
            Assert.AreEqual(TEST_HOUSE_NUMBER, result.HouseNumber);
            Assert.AreEqual(TEST_ADDRESS_LINE_1, result.AddressLine1);
            Assert.AreEqual(TEST_ADDRESS_LINE_2, result.AddressLine2);
            Assert.AreEqual(postTown, result.City);
            Assert.AreEqual(string.Empty, result.County);
            Assert.AreEqual(TEST_POSTCODE, result.Postcode);
            Assert.AreEqual(TEST_UDPRN, result.UDPRN);
        }

        [Test()]
        public void FromLocationResultToAddressInfoConverter_SameAddressLine2AndCity()
        {
            string administrativeArea = "Leeds";
            string addressLine2 = "Leeds";

            var locationResult = new LocationResult()
            {
                HouseNumber = TEST_HOUSE_NUMBER,
                AddressLines = new[] { TEST_ADDRESS_LINE_1, addressLine2 },
                AdministrativeArea = administrativeArea,
                Postcode = TEST_POSTCODE,
                UDPRN = TEST_UDPRN
            };

            var result = Mapper.Map<AddressInfoViewModel>(locationResult);
            Assert.AreEqual(TEST_HOUSE_NUMBER, result.HouseNumber);
            Assert.AreEqual(TEST_ADDRESS_LINE_1, result.AddressLine1);
            Assert.AreEqual(string.Empty, result.AddressLine2);
            Assert.AreEqual(administrativeArea, result.City);
            Assert.AreEqual(string.Empty, result.County);
            Assert.AreEqual(TEST_POSTCODE, result.Postcode);
            Assert.AreEqual(TEST_UDPRN, result.UDPRN);
        }

    }
}
