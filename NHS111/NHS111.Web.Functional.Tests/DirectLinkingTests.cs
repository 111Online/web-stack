using System.Threading;
using NHS111.Web.Functional.Utils;
using NUnit.Framework;

namespace NHS111.Web.Functional.Tests
{
    using System.Linq;
    using NUnit.Framework;
    using OpenQA.Selenium;

    [TestFixture]
    [Category("NightlyOnly")]
    public class DirectLinkingTests : BaseTests
    {
        
        [TestCase("/question/direct/PW755MaleAdult/24/Headache/ls177nz/?answers=0,2,2,2,4,0,1,0,2,2,2,0,2", "Dx05",  TestName = "Can reach Dx05")]
        [TestCase("/question/direct/PW1134MaleAdult/20/Eye,RedorIrritable/ls14br/?answers=0,2,2,1,2,2,2,2,2,2,3,0", "Dx28",  TestName = "Can reach Dx28")]
        [TestCase("/question/direct/PW1610MaleAdult/25/Dentalproblems/ls177nz/?answers=0,1,2,0,0,0,0,2,2&dossearchdatetime=2020-02-01 22:30", "Dx19",  TestName = "DoS SearchTimeDate doesn't break direct link")]
        public void CorrectPage(string path, string id)
        {
            var directLink = TestScenarioPart.DirectLinking(Driver, path);
            directLink.VerifyId(id);
        }

        [Test]
        public void DateTimeCorrect()
        {
            // Before 22:30 Dental Emergency UAT - Leeds, West Yorkshire (Unplanned Booking Service) should show
            var firstTime = TestScenarioPart.DirectLinking(Driver, "/question/direct/PW620FemaleAdult/19/Dentalinjury/LS177NZ/?answers=0,2,4,0,0,0,2,0,0,2,0&dossearchdatetime=2020-02-01 20:30");
            firstTime.VerifyBookACall("2000015238");

            
            // After 22:30 NHS111 Dental Callback - Leeds should show
            var secondTime = TestScenarioPart.DirectLinking(Driver, "/question/direct/PW620FemaleAdult/19/Dentalinjury/LS177NZ/?answers=0,2,4,0,0,0,2,0,0,2,0&dossearchdatetime=2020-02-01 00:00");
            secondTime.VerifyBookACall("2000015239");
        }
        
        [TestFixture]
        [Category("NightlyOnly")]
        public class Validation : BaseTests
        {
            [Test]
            public void Correct999Service()
            {
                // 999 Validation page shows with correct service
                var directLink = TestScenarioPart.DirectLinking(Driver, "/question/direct/PW755MaleAdult/20/Headache/AL7 4HL/?answers=0,0,0,2,0,0");
                directLink.VerifyBookACall("2000011053");
            }

            [Test]
            public void NewDx0124Validation()
            {
                // 999 Validation page shows with correct service
                var directLink = TestScenarioPart.DirectLinking(Driver, "/question/direct/PW1751MaleAdult/20/Mental Health Problems/AL7 4HL/?answers=0,0,4,0,0,0");
                directLink.VerifyBookACall("2000011053");
            }

            [Test]
            public void CorrectEDService()
            {
                // ED Validation page shows with correct service
                var directLink = TestScenarioPart.DirectLinking(Driver, "/question/direct/PW1685MaleAdult/24/Sexual Concerns/AL74HL/?answers=0,2,3,2,2,2,3,3,0,0,2&dossearchdatetime=2020-02-01 12:00");
                Assert.IsTrue(directLink.Driver.FindElement(By.Id("DosCheckCapacitySummaryResult_Success_Services_0__Id")).GetAttribute("value") == "2000005832");
            }

            [Test]
            public void Cat2DoesNotGoToValidation()
            {
                // ED Validation page shows with correct service
                var directLink = TestScenarioPart.DirectLinking(Driver, "/question/direct/PW1620FemaleAdult/22/Skin,Rash/LS17 7NZ/?answers=0,0,0,0");
                Assert.IsTrue(directLink.Driver.FindElement(By.Id("OutcomeGroup_Id")).GetAttribute("value") == "Call_999_cat_2");
            }
        }

        [TestFixture]
        [Category("NightlyOnly")]
        public class RecommendedService : BaseTests
        {
            [Test]
            public void RecommendedServicePage()
            {
                var directLink = TestScenarioPart.DirectLinking(Driver, "/question/direct/PW1827MaleAdult/33/Emergency%20Prescription%20111%20online/L12SA/?answers=0,1,1");
                directLink.VerifyServiceDetails();
            }

            [Test]
            public void OutOfAreaPage()
            {
                var directLink = TestScenarioPart.DirectLinking(Driver, "/question/direct/PW1827MaleAdult/33/Emergency%20Prescription%20111%20online/so302un/?answers=0,1,1");
                directLink.VerifyOutOfArea();
            }

            [Test]
            public void NoRecommendedServicesPage()
            {
                var directLink = TestScenarioPart.DirectLinking(Driver, "/question/direct/PW1827MaleAdult/33/Emergency%20Prescription%20111%20online/LS175DT/?answers=0,1,0&dossearchdatetime=2020-02-01 23:30");
                directLink.VerifyNoRecommendedServices();
            }

            [Test]
            public void OtherServicesPage()
            {
                var directLink = TestScenarioPart.DirectLinking(Driver, "/question/direct/PW1827MaleAdult/33/Emergency%20Prescription%20111%20online/L12SA/?answers=0,1,1&otherservices=true");
                directLink.VerifyOtherServices();
            }
        }
    }
}
