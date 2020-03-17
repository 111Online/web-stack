using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NHS111.Web.Presentation.Validators;
using NUnit.Framework;

namespace NHS111.Models.Test.Models.Web.Validators
{
    [TestFixture]
    class EmailAddressValidatorTests
    {
        private void EmailAddressValidation_TestHelper(string Email, bool expected, string message)
        {
            // Arrange
            var validator = new EmailAddressValidator();

            // Act 
            var actual = validator.Validate(Email);

            // Assert
            Assert.AreEqual(expected, actual, message);
        }

        [Test]
        public void EmailAddressWithoutDomain_ReturnsFalse()
        {
            //EmailAddressValidation_TestHelper("user.com")
        }

        [Test]
        public void EmailAddressWithoutName_ReturnsFalse()
        {
            // Arrange


            // Act


            // Assert
        }

        [Test]
        public void EmailAddressWithoutAtSign_ReturnsFalse()
        {
            // Arrange


            // Act


            // Assert
        }

        [Test]
        public void EmailAddressEmpty_ReturnsFalse()
        {
            // Arrange


            // Act


            // Assert
        }

        [Test]
        public void EmailAddressNull_ReturnsFalse()
        {
            // Arrange


            // Act


            // Assert
        }

        [Test]
        public void EmailAddressWhiteSpace_ReturnsFalse()
        {
            // Arrange


            // Act


            // Assert
        }

        [Test]
        public void ValidEmailAddress_ReturnsTrue()
        {
            // Arrange


            // Act


            // Assert
        }

        [Test]
        public void ValidEmailAddressWithNumbers_ReturnsTrue()
        {
            // Arrange


            // Act


            // Assert
        }

        [Test]
        public void ValidEmailAddressWithSymbols_ReturnsTrue()
        {
            // Arrange


            // Act


            // Assert
        }
    }
}
