using AutoMapper;
using NHS111.Models.Models.Web;
using NHS111.Models.Models.Web.Logging;
using NUnit.Framework;

namespace NHS111.Models.Test.Models.Web.Logging
{
    [TestFixture]
    public class AuditedDosRequestTests
    {
        [TestFixtureSetUp]
        public void InitializeJourneyViewModelMapper()
        {
            Mapper.Initialize(m => m.AddProfile<NHS111.Models.Mappers.WebMappings.AuditedModelMappers>());
        }

        [Test]
        public void AuditedModelMappers_Configuration_IsValid_Test()
        {
            Mapper.AssertConfigurationIsValid();
        }

        [Test]
        public void Postcode_with_space_returns_part_postcode()
        {
            var sut = new DosViewModel
            {
                PostCode = "SO30 2UN"
            };
            var audit = Mapper.Map<DosViewModel, AuditedDosRequest>(sut);
            Assert.AreEqual("SO30", audit.PostCode);
        }

        [Test]
        public void Postcode_with_no_space_returns_part_postcode()
        {
            var sut = new DosViewModel
            {
                PostCode = "SO302UN"
            };
            var audit = Mapper.Map<DosViewModel, AuditedDosRequest>(sut);
            Assert.AreEqual("SO30", audit.PostCode);
        }

        [Test]
        public void Postcode_mixed_case_with_space_returns_part_postcode()
        {
            var sut = new DosViewModel
            {
                PostCode = "So30 2Un"
            };
            var audit = Mapper.Map<DosViewModel, AuditedDosRequest>(sut);
            Assert.AreEqual("So30", audit.PostCode);
        }

        [Test]
        public void Postcode_mixed_case_with_no_space_returns_part_postcode()
        {
            var sut = new DosViewModel
            {
                PostCode = "So302Un"
            };
            var audit = Mapper.Map<DosViewModel, AuditedDosRequest>(sut);
            Assert.AreEqual("So30", audit.PostCode);
        }

        [Test]
        public void Postcode_multiple_spaces_returns_part_postcode()
        {
            var sut = new DosViewModel
            {
                PostCode = " So 30 2U n "
            };
            var audit = Mapper.Map<DosViewModel, AuditedDosRequest>(sut);
            Assert.AreEqual("So30", audit.PostCode);
        }

        [Test]
        public void Postcode_partial_returns_part_postcode()
        {
            var sut = new DosViewModel
            {
                PostCode = "So30"
            };
            var audit = Mapper.Map<DosViewModel, AuditedDosRequest>(sut);
            Assert.AreEqual("So30", audit.PostCode);
        }
    }
}
