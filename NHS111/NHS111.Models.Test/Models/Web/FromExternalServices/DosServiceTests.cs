using Newtonsoft.Json;
using NHS111.Models.Models.Web.FromExternalServices;
using NUnit.Framework;

namespace NHS111.Models.Test.Models.Web.FromExternalServices
{
    [TestFixture]
    public class DosServiceTests
    {
        [Test]
        public void Missing_PublicName_Returns_Name()
        {
            var json = "{\"nameField\":\"ED - Emergency Department Stoke Mandeville Hospital, Bucks (CATCH ALL)\"}";
            var service = JsonConvert.DeserializeObject<DosService>(json);
            Assert.AreEqual("ED - Emergency Department Stoke Mandeville Hospital, Bucks (CATCH ALL)", service.PublicName);
        }

        [Test]
        public void Null_PublicName_Returns_Name()
        {
            var json = "{\"nameField\":\"ED - Emergency Department Stoke Mandeville Hospital, Bucks (CATCH ALL)\",\"publicNameField\":null}";
            var service = JsonConvert.DeserializeObject<DosService>(json);
            Assert.AreEqual("ED - Emergency Department Stoke Mandeville Hospital, Bucks (CATCH ALL)", service.PublicName);
        }

        [Test]
        public void Empty_PublicName_Returns_Name()
        {
            var json = "{\"nameField\":\"ED - Emergency Department Stoke Mandeville Hospital, Bucks (CATCH ALL)\",\"publicNameField\":\"\"}";
            var service = JsonConvert.DeserializeObject<DosService>(json);
            Assert.AreEqual("ED - Emergency Department Stoke Mandeville Hospital, Bucks (CATCH ALL)", service.PublicName);
        }

        [Test]
        public void Value_In_PublicName_Returns_PublicName()
        {
            var json = "{\"nameField\":\"ED - Emergency Department Stoke Mandeville Hospital, Bucks (CATCH ALL)\",\"publicNameField\":\"Stoke Madeville Hospital\"}";
            var service = JsonConvert.DeserializeObject<DosService>(json);
            Assert.AreEqual("Stoke Madeville Hospital", service.PublicName);
        }
    }
}
