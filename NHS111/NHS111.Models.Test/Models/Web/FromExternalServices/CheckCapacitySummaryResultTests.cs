using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using NHS111.Models.Models.Web.FromExternalServices;
using NUnit.Framework;
using System.Collections.Generic;

namespace NHS111.Models.Test.Models.Web.FromExternalServices
{
    [TestFixture]
    public class CheckCapacitySummaryResultTests
    {
        [Test]
        public void Missing_PublicNameField_MapsTo_NameField_Service()
        {
            var json = "{ \"CheckCapacitySummaryResult\": [{\"nameField\":\"ED - Emergency Department Stoke Mandeville Hospital, Bucks (CATCH ALL)\"}]}";
            var jObj = (JObject)JsonConvert.DeserializeObject(json);
            var services = jObj["CheckCapacitySummaryResult"];
            var results = services.ToObject<IList<DosService>>();
            Assert.AreEqual("ED - Emergency Department Stoke Mandeville Hospital, Bucks (CATCH ALL)", results[0].PublicName);
        }

        [Test]
        public void Null_PublicNameField_MapsTo_NameField_Service()
        {
            var json = "{ \"CheckCapacitySummaryResult\": [{\"nameField\":\"ED - Emergency Department Stoke Mandeville Hospital, Bucks (CATCH ALL)\", \"publicNameField\":null}]}";
            var jObj = (JObject)JsonConvert.DeserializeObject(json);
            var services = jObj["CheckCapacitySummaryResult"];
            var results = services.ToObject<IList<DosService>>();
            Assert.AreEqual("ED - Emergency Department Stoke Mandeville Hospital, Bucks (CATCH ALL)", results[0].PublicName);
        }

        [Test]
        public void Empty_PublicNameField_MapsTo_NameField_Service()
        {
            var json = "{ \"CheckCapacitySummaryResult\": [{\"nameField\":\"ED - Emergency Department Stoke Mandeville Hospital, Bucks (CATCH ALL)\", \"publicNameField\":\"\"}]}";
            var jObj = (JObject)JsonConvert.DeserializeObject(json);
            var services = jObj["CheckCapacitySummaryResult"];
            var results = services.ToObject<IList<DosService>>();
            Assert.AreEqual("ED - Emergency Department Stoke Mandeville Hospital, Bucks (CATCH ALL)", results[0].PublicName);
        }

        [Test]
        public void PublicNameField_MapsTo_PublicNameField_Service()
        {
            var json = "{ \"CheckCapacitySummaryResult\": [{\"publicNameField\":\"TEST NAME\"}]}";
            var jObj = (JObject)JsonConvert.DeserializeObject(json);
            var services = jObj["CheckCapacitySummaryResult"];
            var results = services.ToObject<IList<DosService>>();
            Assert.AreEqual("TEST NAME", results[0].PublicName);
        }
    }
}
