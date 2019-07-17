using NHS111.Web.Functional.Utils;
using NHS111.Web.Functional.Utils.ScreenShot;

namespace NHS111.Web.Functional.Tests
{
    using System.Linq;
    using NUnit.Framework;
    using OpenQA.Selenium;

    [TestFixture]
    class PersonalDetailsTests : BaseTests
    {
        [Test]
        public void PersonalDetailsViaEP()
        {
            var questionPage = TestScenerios.LaunchRecommendedServiceScenerio(Driver, "Emergency Prescription 111 online", TestScenerioSex.Male, TestScenerioAgeGroups.Adult, "L12SA");

            questionPage.VerifyQuestion("Can you contact your GP or usual pharmacy?");
            var recommendedServicePage = questionPage
                .Answer(2)
                .Answer<PreOutcomePage>(1)
                .ClickShowServices();
            var referralExplanationPage = recommendedServicePage.ClickActionLink();
            var personalDetailsPage = referralExplanationPage.ClickButton();

            personalDetailsPage.VerifyIsPersonalDetailsPage();
        }


        [Test]
        public void PersonalDetailsCorrectSections()
        {
            var questionPage = TestScenerios.LaunchTriageScenerio(Driver, "Diabetes Blood Sugar Problem (Declared)", TestScenerioSex.Male, TestScenerioAgeGroups.Adult, "sk10 3de");
            var outcomePage = questionPage
                .Answer(1)
                .Answer(1)
                .Answer(1)
                .Answer(2)
                .Answer(1)
                .Answer(3)
                .Answer(1)
                .Answer(2)
                .Answer<OutcomePage>(4);

            var personalDetailsPage = outcomePage.ClickBookCallback();

            personalDetailsPage.VerifyIsPersonalDetailsPage();
            personalDetailsPage.VerifyNameDisplayed();
            personalDetailsPage.VerifyNumberDisplayed();
            personalDetailsPage.VerifyDateOfBirthDisplayed();
        }
    }
}
