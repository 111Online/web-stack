using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using NHS111.Models.Models.Web.FromExternalServices;
using NUnit.Framework;

namespace NHS111.Models.Test.Models.Web.FromExternalServices
{
    [TestFixture]
    public class DosServiceTests
    {
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
            var json = "{\"idField\":1315835856,\"capacityField\":0,\"nameField\":\"ED - Emergency Department Stoke Mandeville Hospital, Bucks (CATCH ALL)\",\"publicNameField\":\"\",\"contactDetailsField\":\"01296 315664\",\"addressField\":\"Mandeville Road, Aylesbury, Bucks, HP21 8AL\",\"postcodeField\":\"HP21 8AL\",\"northingsField\":211849,\"northingsFieldSpecified\":true,\"eastingsField\":482858,\"eastingsFieldSpecified\":true,\"urlField\":\"\",\"serviceTypeField\":{\"idField\":40,\"nameField\":\"Emergency Department (ED)\",\"PropertyChanged\":null},\"odsCodeField\":\"RXQ02\",\"nonPublicTelephoneNoField\":\"\",\"faxField\":\"\",\"referralTextField\":\"You can go straight to this service. You do not need to telephone beforehand.|\",\"distanceField\":\"0.0\",\"notesField\":\"Referral Information: For minor injuries consider Wycombe MIIU.\",\"openAllHoursField\":true,\"waitTimesField\":{\"currentWaitTimeField\":\"\",\"patientsInDepartmentField\":\"\",\"patientsBeingSeenField\":\"\",\"notesField\":\"\",\"PropertyChanged\":null},\"rotaSessionsField\":[],\"openTimeSpecifiedSessionsField\":[],\"PropertyChanged\":null}";
            var service = JsonConvert.DeserializeObject<DosService>(json);
            Assert.AreEqual("ED - Emergency Department Stoke Mandeville Hospital, Bucks (CATCH ALL)", service.PublicName);
        }

        [Test]
        public void Value_In_PublicName_Returns_PublicName()
        {
            var json = "{\"idField\":1315835856,\"capacityField\":0,\"nameField\":\"ED - Emergency Department Stoke Mandeville Hospital, Bucks (CATCH ALL)\",\"publicNameField\":\"Stoke Madeville Hospital\",\"contactDetailsField\":\"01296 315664\",\"addressField\":\"Mandeville Road, Aylesbury, Bucks, HP21 8AL\",\"postcodeField\":\"HP21 8AL\",\"northingsField\":211849,\"northingsFieldSpecified\":true,\"eastingsField\":482858,\"eastingsFieldSpecified\":true,\"urlField\":\"\",\"serviceTypeField\":{\"idField\":40,\"nameField\":\"Emergency Department (ED)\",\"PropertyChanged\":null},\"odsCodeField\":\"RXQ02\",\"nonPublicTelephoneNoField\":\"\",\"faxField\":\"\",\"referralTextField\":\"You can go straight to this service. You do not need to telephone beforehand.|\",\"distanceField\":\"0.0\",\"notesField\":\"Referral Information: For minor injuries consider Wycombe MIIU.\",\"openAllHoursField\":true,\"waitTimesField\":{\"currentWaitTimeField\":\"\",\"patientsInDepartmentField\":\"\",\"patientsBeingSeenField\":\"\",\"notesField\":\"\",\"PropertyChanged\":null},\"rotaSessionsField\":[],\"openTimeSpecifiedSessionsField\":[],\"PropertyChanged\":null}";
            var service = JsonConvert.DeserializeObject<DosService>(json);
            Assert.AreEqual("Stoke Madeville Hospital", service.PublicName);
        }
    }
}
