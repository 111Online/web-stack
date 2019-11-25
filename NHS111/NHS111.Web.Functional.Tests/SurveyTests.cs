using System.Linq;
using NHS111.Web.Functional.Utils;
using NHS111.Web.Functional.Utils.ScreenShot;
using NUnit.Framework;
using OpenQA.Selenium;

namespace NHS111.Web.Functional.Tests
{
    [TestFixture]
    public class SurveyTests : BaseTests
    {

        /*
         * These tests ensure the model is correct for passing through to the survey.
         * They ensure we don't have issues such as the Pathways Title being passed
         * through instead of the Digital Title.
         */

        [Test]
        public void DigitalTitleThroughSearch()
        {
            var questionPage = TestScenerios.LaunchTriageScenerio(Driver, "Arm Injury, Penetrating",
                TestScenerioSex.Male,
                TestScenerioAgeGroups.Adult);

            questionPage.VerifyHiddenField("PathwayTitle", "Arm Injury, Penetrating");
            questionPage.VerifyHiddenField("DigitalTitle", "Arm or shoulder injury with a cut or wound");
        }

        [Test]
        public void DigitalTitleThroughCategoryAllTopics()
        {
            var categoryPage = TestScenerios.LaunchCategoryScenerio(Driver, TestScenerioSex.Female, 64);
            var questionPage = TestScenarioPart.Question(categoryPage.SelectPathway("Arm or shoulder injury with a cut or wound"));

            questionPage.VerifyHiddenField("PathwayTitle", "Arm Injury, Penetrating");
            questionPage.VerifyHiddenField("DigitalTitle", "Arm or shoulder injury with a cut or wound");
        }

        [Test]
        public void DigitalTitleThroughDeeplink()
        {
            var questionPage = TestScenerios.LaunchDeeplinkScenerio(Driver, TestScenerioSex.Male, TestScenerioAgeGroups.Adult, "L1 2SA");

            questionPage.VerifyHiddenField("PathwayTitle", "Emergency Prescription 111 online");
            questionPage.VerifyHiddenField("DigitalTitle", "Emergency prescription");
        }

        

        [Test]
        public void InterstitialPageHasSurveyUrl()
        {
            // Run dead end journey as short/quick to get to the survey link
            var questionPage = TestScenerios.LaunchTriageScenerio(Driver, "Trauma Blisters", TestScenerioSex.Male, TestScenerioAgeGroups.Adult);
            
            var outcomePage = questionPage
                .Answer<DeadEndPage>(1);

            outcomePage.CompareAndVerify("1");  // Captures screenshot of disposition

            var surveyUrlElement = Driver.FindElement(By.CssSelector(".survey-banner [name='SurveyUrl']"));
            var surveyUrl = surveyUrlElement.GetAttribute("value");
            Assert.IsNotEmpty(surveyUrl);

            var surveyButton = Driver.FindElement(By.CssSelector(".survey-banner button"));
            surveyButton.Click();
            
            Driver.SwitchTo().Window(Driver.WindowHandles.Last()); // Handle new tab 
            var surveyInterstitialPage = new SurveyInterstitial(Driver);
            surveyInterstitialPage.VerifyHeading("Thanks for agreeing to take our survey");
            surveyInterstitialPage.VerifyUrl(surveyUrl);
            surveyInterstitialPage.CompareAndVerify("2"); // Captures screenshot of survey interstitial


        }

        [Test]
        public void InterstitialPageHasSurveyUrlViaEP()
        {
            var questionPage = TestScenerios.LaunchRecommendedServiceScenerio(Driver, "Emergency Prescription 111 online", TestScenerioSex.Male, TestScenerioAgeGroups.Adult, "PR67JY");

            questionPage.VerifyQuestion("Can you contact your GP or usual pharmacy?");
            questionPage
                .Answer(2)
                .Answer<PreOutcomePage>(1)
                .ClickShowServices();

            var surveyUrlElement = Driver.FindElement(By.CssSelector(".survey-banner [name='SurveyUrl']"));
            var surveyUrl = surveyUrlElement.GetAttribute("value");
            Assert.IsNotEmpty(surveyUrl);
            var surveyButton = Driver.FindElement(By.CssSelector(".survey-banner button"));
            surveyButton.Click();
            
            Driver.SwitchTo().Window(Driver.WindowHandles.Last()); // Handle new tab 
            var surveyInterstitialPage = new SurveyInterstitial(Driver);
            surveyInterstitialPage.VerifyHeading("Thanks for agreeing to take our survey");
            surveyInterstitialPage.VerifyUrl(surveyUrl);
            surveyInterstitialPage.CompareAndVerify("2"); // Captures screenshot of survey interstitial


        }
    }
}
