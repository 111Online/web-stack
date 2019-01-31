using System.Threading;
using NHS111.Web.Functional.Utils;
using NUnit.Framework;

namespace NHS111.Web.Functional.Tests
{
    using System.Linq;
    using NUnit.Framework;
    using OpenQA.Selenium;

    [TestFixture]
    public class DirectLinkingTests
        : BaseTests
    {
        
        [TestCase("/question/direct/PW755MaleAdult/24/Headache/?answers=2,2,2,4,0,1,0,2,2,2,0,2&postcode=ls17 7nz", "Dx05",  TestName = "Can reach Dx05")]
        [TestCase("/question/direct/PW1134MaleAdult/20/Eye,RedorIrritable/?answers=2,2,1,2,2,2,2,2,2,3,0&postcode=ls1 4br", "Dx28",  TestName = "Can reach Dx28")]
        [TestCase("/question/direct/PW1610MaleAdult/25/Dentalproblems/?answers=1,2,0,0,0,0,2,2&postcode=ls17 7nz&dossearchdatetime=2018-09-21 22:30", "Dx19",  TestName = "DoS SearchTimeDate doesn't break direct link")]
        public void TestOutcomes(string path, string id)
        {
            var directLink = TestScenarioPart.DirectLinking(Driver, path);
            directLink.VerifyId(id);
        }

        [Test]
        public void DateTime_Correct()
        {
            // Before 22:30 Dental Emergency UAT - Leeds, West Yorkshire (Unplanned Booking Service) should show
            var firstTime = TestScenarioPart.DirectLinking(Driver, "/question/direct/PW1610MaleAdult/25/Dentalproblems/?answers=1,2,0,0,0,0,2,2&postcode=ls17 7nz&dossearchdatetime=2019-02-28 20:30");
            firstTime.VerifyBookACall("1362471653");

            
            // After 22:30 NHS111 Dental Callback - Leeds should show
            var secondTime = TestScenarioPart.DirectLinking(Driver, "/question/direct/PW1610MaleAdult/25/Dentalproblems/?answers=1,2,0,0,0,0,2,2&postcode=ls17 7nz&dossearchdatetime=2019-02-28 23:30");
            secondTime.VerifyBookACall("1479808152");
        }
    }
}
