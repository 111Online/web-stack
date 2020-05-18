using NHS111.Web.Functional.Utils;
using NUnit.Framework;
using OpenQA.Selenium;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NHS111.Web.Functional.Tests.Covid
{
    [TestFixture]
    class Dx391 : BaseTests
    {
        [Test]
        public void NavigateToDispositionDx1113()
        {
            var questionPage = LaunchCovidWithLink(TestScenerioSex.Female, TestScenerioAgeGroups.Adult);

            var outcomePage = questionPage.AnswerText("SymptomsStart_Day", "6")
                .Answer(2) // no
                .Answer(1) // yes
                .Answer(3) // no
                .Answer(3) // no
                .Answer(1) // Normal,warmorhot
                .Answer<OutcomePage>(1); // Yes, I stopped doing everything what i do
            outcomePage.VerifyHiddenField("Id", "Dx1113");
        }

        private QuestionPage LaunchCovidWithLink(string sex, int age)
        {
            var homepage = TestScenarioPart.HomePage(Driver);
            var covidHomePage = homepage.ClickCovidLink();
            covidHomePage.VerifyCovidPathway();
            var locationPage = covidHomePage.ClickOnStartNow();
            var moduleZeroPage = TestScenarioPart.ModuleZero(locationPage);
            var demographicsPage = TestScenarioPart.Demographics(moduleZeroPage);
            return TestScenarioPart.Question(demographicsPage, sex, age);
        }

        [Test]
        public void NavigateToDispositionDx1113Journey2()
        {
            var questionPage = LaunchCovidWithLink(TestScenerioSex.Female, TestScenerioAgeGroups.Adult);

            var outcomePage = questionPage.AnswerText("SymptomsStart_Day", "6")
                .Answer(2) // no
                .Answer(1) // yes
                .Answer(3) // no
                .Answer(3) // no
                .Answer(1) // Normal,warmorhot
                .Answer(3) // No - I feel well enough to do most of my usual daily activities 
                .Answer<OutcomePage>(1); // Yes
            outcomePage.VerifyHiddenField("Id", "Dx1113");
            outcomePage.VerifyHasButton(buttonName: "PersonalDetails", buttonValue: "Book a call");
        }

       
        
        [Test]
        public void NavigateToDispositionDx391()
        {
            var questionPage = LaunchCovidWithLink(TestScenerioSex.Female, TestScenerioAgeGroups.Adult);

            var outcomePage = questionPage.AnswerText("SymptomsStart_Day", "6")
                .Answer(2) // no
                .Answer(1) // yes
                .Answer(3) // no
                .Answer(3) // no
                .Answer(1) // Normal,warmorhot
                .Answer(3) // No - I feel well enough to do most of my usual daily activities 
                .Answer(2) // I'm not sure
                .Answer(2) // I'm not sure
                .Answer<OutcomePage>(2); // I'm notsure
            outcomePage.VerifyHiddenField("Id", "Dx391");
        }
            }
}
