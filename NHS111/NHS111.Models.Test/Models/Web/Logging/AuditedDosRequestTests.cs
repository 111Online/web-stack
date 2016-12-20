using AutoMapper;
using NHS111.Models.Models.Web;
using NHS111.Models.Models.Web.Logging;
using NUnit.Framework;

namespace NHS111.Models.Test.Models.Web.Logging
{
    [TestFixture]
    public class AuditedDosRequestTests
    {
        [Test]
        public void Postcode_with_space_returns_part_postcode()
        {
            var sut = new AuditedDosRequest()
            {
                PostCode = "SO30 2UN"
            };
            Assert.AreEqual("SO30", sut.PostCode);
        }

        [Test]
        public void Postcode_with_no_space_returns_part_postcode()
        {
            var sut = new AuditedDosRequest()
            {
                PostCode = "SO302UN"
            };
            Assert.AreEqual("SO30", sut.PostCode);
        }

        [Test]
        public void Postcode_mixed_case_with_space_returns_part_postcode()
        {
            var sut = new AuditedDosRequest()
            {
                PostCode = "So30 2Un"
            };
            Assert.AreEqual("So30", sut.PostCode);
        }

        [Test]
        public void Postcode_mixed_case_with_no_space_returns_part_postcode()
        {
            var sut = new AuditedDosRequest()
            {
                PostCode = "So302Un"
            };
            Assert.AreEqual("So30", sut.PostCode);
        }

        [Test]
        public void Postcode_multiple_spaces_returns_part_postcode()
        {
            var sut = new AuditedDosRequest()
            {
                PostCode = " So 30 2U n "
            };
            Assert.AreEqual("So30", sut.PostCode);
        }

        [Test]
        public void Postcode_partial_returns_part_postcode()
        {
            var sut = new AuditedDosRequest()
            {
                PostCode = "So30"
            };
            Assert.AreEqual("So30", sut.PostCode);
        }
    }
}
