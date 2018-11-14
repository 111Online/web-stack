using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NHS111.SmokeTest.Utils;
using NUnit.Framework;

namespace NHS111.SmokeTests.Regression
{
    public class DosLookupTests : BaseTests
    {

        [Test]
        //PT8 via Behaviour Change Tx222027 and Tx222006
        public void Dental_Disposition_Renders_DOSServices()
        {
            var questionPage = TestScenerios.LaunchTriageScenerio(Driver, "Dental Problems", TestScenerioSex.Male, TestScenerioAgeGroups.Adult);

            var outcomePage = questionPage
                .Answer(2)
                .Answer(4)
                .AnswerSuccessiveYes(2)
                .Answer(3)
                .AnswerForDispostion<OutcomePage>("No - I've not taken any painkillers");

            outcomePage.VerifyOutcome("See your dentist urgently");
            outcomePage.VerifyPageContainsDOSResults();
            outcomePage.VerifyDOSResultGroupExists("Arrange for someone to phone you");
        }

        [Test]
        //PT8 via LowMood_Depression via operator branch
        public void Pt8ViaLowMoodDepressionViaOperatorBranch()
        {
            var questionPage = TestScenerios.LaunchTriageScenerio(Driver, "Mental Health Problems", TestScenerioSex.Male, TestScenerioAgeGroups.Adult);

            var outcomePage = questionPage
                .AnswerSuccessiveNo(2)
                .Answer(1)
                .AnswerSuccessiveNo(12)
                .AnswerForDispostion<OutcomePage>("Less than 2 weeks");

            outcomePage.VerifyOutcome(OutcomePage.BookCallBackText, OutcomePage.Call111Text);
            outcomePage.VerifyDispositionCode("Dx35");
        }

        [Test]
        //PT8 via MHP jump to PW1738 Drug_Solvent_Alcohol_Misuse then 3 strings
        public void Pt8ViaMhpJumpPw1738DrugSolventAlcoholMisuse3Strings()
        {
            var questionPage = TestScenerios.LaunchTriageScenerio(Driver, "Mental Health Problems", TestScenerioSex.Male, TestScenerioAgeGroups.Adult);

            var outcomePage = questionPage
                .AnswerSuccessiveNo(2)
                .Answer(4)
                .Answer(2)
                .AnswerSuccessiveNo(2)
                .AnswerForDispostion<OutcomePage>("Specialist help");

            outcomePage.VerifyOutcome(OutcomePage.BookCallBackText, OutcomePage.Call111Text);
            outcomePage.VerifyDispositionCode("Dx35");
        }






    }
}
