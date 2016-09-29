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
        private string TEST_CITY = "Some place";
        private string TEST_COUNTY = "Some time";
        private string TEST_POSTCODE = "XX1 2YY";

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
                PostTown = TEST_CITY,
                AdministrativeArea = TEST_COUNTY,
                Postcode = TEST_POSTCODE,
                StreetDescription = "Test street desc"
            };

            var result = Mapper.Map<AddressInfoViewModel>(locationResult);
            Assert.AreEqual(TEST_HOUSE_NUMBER, result.HouseNumber);
            Assert.AreEqual("Test street desc", result.AddressLine1);
            Assert.AreEqual(string.Empty, result.AddressLine2);
            Assert.AreEqual(TEST_CITY, result.City);
            Assert.AreEqual(TEST_COUNTY, result.County);
            Assert.AreEqual(TEST_POSTCODE, result.PostCode);
        }

        [Test()]
        public void FromLocationResultToAddressInfoConverter_Single_AddressLine_Test()
        {
            var locationResult = new LocationResult()
            {
                HouseNumber = TEST_HOUSE_NUMBER,
                AddressLines = new[] { TEST_ADDRESS_LINE_1 },
                PostTown = TEST_CITY,
                AdministrativeArea = TEST_COUNTY,
                Postcode = TEST_POSTCODE
            };

            var result = Mapper.Map<AddressInfoViewModel>(locationResult);
            Assert.AreEqual(TEST_HOUSE_NUMBER, result.HouseNumber);
            Assert.AreEqual(TEST_ADDRESS_LINE_1, result.AddressLine1);
            Assert.AreEqual(string.Empty, result.AddressLine2);
            Assert.AreEqual(TEST_CITY, result.City);
            Assert.AreEqual(TEST_COUNTY, result.County);
            Assert.AreEqual(TEST_POSTCODE, result.PostCode);
        }


        [Test()]
        public void FromLocationResultToAddressInfoConverter_Test()
        {
            var locationResult = new LocationResult()
            {
                HouseNumber = TEST_HOUSE_NUMBER,
                AddressLines = new [] { TEST_ADDRESS_LINE_1, TEST_ADDRESS_LINE_2 },
                PostTown = TEST_CITY,
                AdministrativeArea = TEST_COUNTY,
                Postcode = TEST_POSTCODE
            };

            var result = Mapper.Map<AddressInfoViewModel>(locationResult);
            Assert.AreEqual(TEST_HOUSE_NUMBER, result.HouseNumber);
            Assert.AreEqual(TEST_ADDRESS_LINE_1, result.AddressLine1);
            Assert.AreEqual(TEST_ADDRESS_LINE_2, result.AddressLine2);
            Assert.AreEqual(TEST_CITY, result.City);
            Assert.AreEqual(TEST_COUNTY, result.County);
            Assert.AreEqual(TEST_POSTCODE, result.PostCode);
        }
    }
}
