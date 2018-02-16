using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NHS111.Models;
using NHS111.Models.Models.Web;
using NUnit.Framework;

namespace NHS111.Models.Test.Models.Web
{
    [TestFixture]
    public class AddressInfoViewModelTests
    {
        private NHS111.Models.Models.Web.AddressInfoViewModel _addressInfoViewModel = new AddressInfoViewModel();

        [Test]
        public void FormattedPostcode_5char_code_returns_Valid_Format()
        {
            var testPostcode = "W11ft";
            _addressInfoViewModel.Postcode = testPostcode;

            Assert.AreEqual("W1 1FT", _addressInfoViewModel.FormattedPostcode);
        }
        [Test]
        public void FormattedPostcode_6char_code_returns_Valid_Format()
        {
            var testPostcode = "Ab11th";
            _addressInfoViewModel.Postcode = testPostcode;

            Assert.AreEqual("AB1 1TH", _addressInfoViewModel.FormattedPostcode);
        }

        [Test]
        public void FormattedPostcode_7char_code_returns_Valid_Format()
        {
            var testPostcode = "Ab131th";
            _addressInfoViewModel.Postcode = testPostcode;

            Assert.AreEqual("AB13 1TH", _addressInfoViewModel.FormattedPostcode);
        }

        [Test]
        public void FormattedPostcode_7char_code_with_spaces_returns_Valid_Format()
        {
            var testPostcode = "Ab  131th";
            _addressInfoViewModel.Postcode = testPostcode;

            Assert.AreEqual("AB13 1TH", _addressInfoViewModel.FormattedPostcode);
        }

        [Test]
        public void FormattedPostcode_NULL_returns_NULL()
        {
            _addressInfoViewModel.Postcode = null;
            Assert.IsNull(_addressInfoViewModel.FormattedPostcode);
        }

        [Test]
        public void FormattedPostcode_empty_returns_Valid_Format()
        {
            var testPostcode = "";
            _addressInfoViewModel.Postcode = testPostcode;
            Assert.AreEqual("", _addressInfoViewModel.FormattedPostcode);
        }
    }
}
