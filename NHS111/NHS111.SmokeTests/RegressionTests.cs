using System;
using System.Text;
using NHS111.SmokeTest.Utils;
using NUnit.Framework;
using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using OpenQA.Selenium.Firefox;
using OpenQA.Selenium.Support.UI;

namespace NHS111.SmokeTests
{
    [TestFixture]
    public class RegressionTests
    {
        private IWebDriver _driver;

        [TestFixtureSetUp]
        public void InitTests()
        {
            _driver = new ChromeDriver();
        }

        [TestFixtureTearDown]
        public void TeardownTest()
        {
            try
            {
                //_driver.Quit();
                //_driver.Dispose();
            }
            catch (Exception)
            {
                // Ignore errors if unable to close the browser
            }
        }

         
        [Test]
        public void PT8JumpToDx35()
        {
            var questionPage = TestScenerios.LaunchTriageScenerio(_driver, "Tiredness (Fatigue)", TestScenerioGender.Male, TestScenerioAgeGroups.Adult);

            questionPage.ValidateQuestion("Have you got a raised temperature now or have you had one at any time since the tiredness started?");
            var outcomePage = questionPage
                .AnswerSuccessiveByOrder(3, 4)
                .Answer(4)
                .Answer(4)
                .Answer(3)
                .Answer(4)
                .Answer(3)
                .AnswerSuccessiveByOrder(5, 2)
                .AnswerSuccessiveByOrder(3, 4)
                .AnswerForDispostion("Alcohol");

            outcomePage.VerifyOutcome("Your answers suggest that you need a 111 clinician to call you");
        }
        [Test]
        public void PathwayNotFound()
        {
            var questionPage = TestScenerios.LaunchTriageScenerio(_driver, "Wound Problems, Plaster Casts, Tubes and Metal Appliances", TestScenerioGender.Male, TestScenerioAgeGroups.Adult);

            questionPage.ValidateQuestion("Is the problem to do with any of these?");
            var outcomePage = questionPage
 
             .AnswerForDispostion("A tube or drain");
        
            outcomePage.VerifyPathwayNotFound();
        }


        [Test]
        public void SplitQuestionNavigateBackDisplaysCorrectCareAdvice()
        {
            var questionPage = TestScenerios.LaunchTriageScenerio(_driver, "Headache", "Female", 49);

            var outcomePage = questionPage.ValidateQuestion("Is there a chance you are pregnant?")

           .AnswerSuccessiveByOrder(3, 4)
            .Answer(1)
            .Answer(3)
            .Answer(5)
            .Answer(3)
            .Answer(4)
            .Answer(2)
            .Answer(3)
            .Answer(3)
            .Answer(3)
            .Answer(4)
            .Answer(1)
            .Answer(3)
            .Answer(4)
            .AnswerForDispostion(1);

            var newOutcome = outcomePage.NavigateBack()
            .Answer(3, false)
            .Answer(1)


             .AnswerForDispostion("Within the next 6 hours");

            newOutcome.EnterPostCodeAndSubmit("LS17 7NZ");

            newOutcome.VerifyOutcome("You should speak to your GP practice within the next 6 hours");
            newOutcome.VerifyCareAdvice(new[] { "Medication, next dose", "Medication, pain and/or fever", "Headache" });

            
        }

        [Test]
        public void SplitQuestionJourneyThroughEachRoute()
        {
            var questionPage = TestScenerios.LaunchTriageScenerio(_driver, "Headache", TestScenerioGender.Male, TestScenerioAgeGroups.Adult);

            questionPage.ValidateQuestion("Have you hurt or banged your head in the last 7 days?");
            var outcomePage = questionPage
                .Answer(3)
                .Answer(3)
                .Answer(1)
             .AnswerForDispostion("Yes - I have a rash that doesn't disappear if I press it");

            outcomePage.VerifyOutcome("Your answers suggest you should dial 999 now for an ambulance");

            TestScenerios.LaunchTriageScenerio(_driver, "Headache", "Female", 49);

            questionPage.ValidateQuestion("Is there a chance you are pregnant?")
            
           .AnswerSuccessiveByOrder(3, 4)
            .Answer(1)
            .Answer(3)
            .Answer(5)
            .Answer(3)
            .Answer(4)
            .Answer(2)
            .Answer(3)
            .Answer(3)
            .Answer(3)
            .Answer(4)
            .Answer(1)
            .Answer(3)
            .Answer(4)
                //.Answer(1)
                //.NavigateBack()
            .Answer(3)
            .Answer(1)
              

             .AnswerForDispostion("Within the next 6 hours");

            outcomePage.EnterPostCodeAndSubmit("LS17 7NZ");

            outcomePage.VerifyOutcome("You should speak to your GP practice within the next 6 hours");


            TestScenerios.LaunchTriageScenerio(_driver, "Headache", "Female", 50);

            questionPage.ValidateQuestion("Is there a chance you are pregnant?")
            
               .AnswerSuccessiveByOrder(3, 5)
                .Answer(5)
                .Answer(3)
                .Answer(4)
                .Answer(2)
                .AnswerSuccessiveByOrder(3, 3)

             .AnswerForDispostion("Yes");

            outcomePage.EnterPostCodeAndSubmit("LS17 7NZ");


            outcomePage.VerifyOutcome("You should speak to your GP practice within the next 2 hours");
        }

           [Test]
        public void CategoriespresentForFemaleChild()
               //Test to show all categories are present per age and gender, and also the correct pathway opens per age and gender, when the same pathway is used for all ages/genders.
        {

                var searchPage = TestScenerios.LaunchSearchScenerio(_driver, TestScenerioGender.Female, 5);
           
           _driver.FindElement(By.Id("searchButton")).Click();

            _driver.FindElement(By.XPath("//a[@data-title= \"Abdomen injury - skin not broken\"][@data-pathway-number= 'PW500FemaleChild']"));
            _driver.FindElement(By.XPath("//a[@data-title= \"Injury to the side of the body - skin not broken\"][@data-pathway-number= 'PW500FemaleChild']"));
            _driver.FindElement(By.XPath("//a[@data-title= \"Abdomen injury with broken skin\"][@data-pathway-number= 'PW508FemaleChild']"));
            _driver.FindElement(By.XPath("//a[@data-title= \"Injury to the side of the body with broken skin\"][@data-pathway-number= 'PW508FemaleChild']"));
            _driver.FindElement(By.XPath("//a[@data-title= \"Abdominal pain\"][@data-pathway-number= 'PW517FemaleChild']"));
            _driver.FindElement(By.XPath("//a[@data-title= \"Stomach pain\"][@data-pathway-number= 'PW517FemaleChild']"));
            _driver.FindElement(By.XPath("//a[@data-title= \"Absent periods\"][@data-pathway-number= 'PW1676FemaleChild']"));
            _driver.FindElement(By.XPath("//a[@data-title= \"Missed periods\"][@data-pathway-number= 'PW1676FemaleChild']"));
            _driver.FindElement(By.XPath("//a[@data-title= \"Accidental poisoning\"][@data-pathway-number= 'PW881FemaleChild']"));
            _driver.FindElement(By.XPath("//a[@data-title= \"Breathed in something poisonous\"][@data-pathway-number= 'PW881FemaleChild']"));
            _driver.FindElement(By.XPath("//a[@data-title= \"Swallowed something poisonous\"][@data-pathway-number= 'PW881FemaleChild']"));
            _driver.FindElement(By.XPath("//a[@data-title= \"Drunk too much alcohol\"][@data-pathway-number= 'PW1552FemaleChild']"));
            _driver.FindElement(By.XPath("//a[@data-title= \"Alcohol intoxication\"][@data-pathway-number= 'PW1552FemaleChild']"));
            _driver.FindElement(By.XPath("//a[@data-title= \"Ankle injury - skin not broken\"][@data-pathway-number= 'PW1512FemaleChild']"));
            _driver.FindElement(By.XPath("//a[@data-title= \"Foot injury - skin not broken\"][@data-pathway-number= 'PW1512FemaleChild']"));
            _driver.FindElement(By.XPath("//a[@data-title= \"Ankle injury with broken skin\"][@data-pathway-number= 'PW1518FemaleChild']"));
            _driver.FindElement(By.XPath("//a[@data-title= \"Foot injury with broken skin\"][@data-pathway-number= 'PW1518FemaleChild']"));
            _driver.FindElement(By.XPath("//a[@data-title= \"Arm injury - skin not broken\"][@data-pathway-number= 'PW895FemaleChild']"));
            _driver.FindElement(By.XPath("//a[@data-title= \"Arm injury with broken skin\"][@data-pathway-number= 'PW902FemaleChild']"));
            _driver.FindElement(By.XPath("//a[@data-title= \"Painful arm\"][@data-pathway-number= 'PW1165FemaleChild']"));
            _driver.FindElement(By.XPath("//a[@data-title= \"Swollen arm\"][@data-pathway-number= 'PW1165FemaleChild']"));
            _driver.FindElement(By.XPath("//a[@data-title= \"Cold arm\"][@data-pathway-number= 'PW1733FemaleChild']"));
            _driver.FindElement(By.XPath("//a[@data-title= \"Arm changing colour\"][@data-pathway-number= 'PW1733FemaleChild']"));
            _driver.FindElement(By.XPath("//a[@data-title= \"Change in behaviour\"][@data-pathway-number= 'PW1749FemaleChild']"));
            _driver.FindElement(By.XPath("//a[@data-title= \"Bites and stings\"][@data-pathway-number= 'PW1575FemaleChild']"));
            _driver.FindElement(By.XPath("//a[@data-title= \"Bleeding from stoma\"][@data-pathway-number= 'PT507FemaleChild']"));
            _driver.FindElement(By.XPath("//a[@data-title= \"Blood in urine\"][@data-pathway-number= 'PW961FemaleChild']"));
            _driver.FindElement(By.XPath("//a[@data-title= \"Breast problems\"][@data-pathway-number= 'PW1605FemaleChild']"));
            _driver.FindElement(By.XPath("//a[@data-title= \"Breastfeeding problems\"][@data-pathway-number= 'PW1114FemaleChild']"));
            _driver.FindElement(By.XPath("//a[@data-title= \"Breathing problems\"][@data-pathway-number= 'PW557FemaleChild']"));
            _driver.FindElement(By.XPath("//a[@data-title= \"Upper back pain\"][@data-pathway-number= 'PW557FemaleChild']"));
            _driver.FindElement(By.XPath("//a[@data-title= \"Chest and upper back pain\"][@data-pathway-number= 'PW557FemaleChild']"));
            _driver.FindElement(By.XPath("//a[@data-title= \"Chest pain\"][@data-pathway-number= 'PW557FemaleChild']"));
            _driver.FindElement(By.XPath("//a[@data-title= \"Wheezing\"][@data-pathway-number= 'PW557FemaleChild']"));
            _driver.FindElement(By.XPath("//a[@data-title= \"Coughing up blood\"][@data-pathway-number= 'PW1652FemaleChild']"));
            _driver.FindElement(By.XPath("//a[@data-title= \"Bringing up blood\"][@data-pathway-number= 'PW1652FemaleChild']"));
            _driver.FindElement(By.XPath("//a[@data-title= \"Burns from chemicals\"][@data-pathway-number= 'PW564FemaleChild']"));
            _driver.FindElement(By.XPath("//a[@data-title= \"Sunburn\"][@data-pathway-number= 'PW987FemaleChild']"));
            _driver.FindElement(By.XPath("//a[@data-title= \"Burns from heat\"][@data-pathway-number= 'PW580FemaleChild']"));
            _driver.FindElement(By.XPath("//a[@data-title= \"Catheter problems\"][@data-pathway-number= 'PW1567FemaleChild']"));
            _driver.FindElement(By.XPath("//a[@data-title= \"Chest injury - skin not broken\"][@data-pathway-number= 'PW588FemaleChild']"));
            _driver.FindElement(By.XPath("//a[@data-title= \"Upper back injury - skin not broken\"][@data-pathway-number= 'PW588FemaleChild']"));
            _driver.FindElement(By.XPath("//a[@data-title= \"Chest injury with broken skin\"][@data-pathway-number= 'PW596FemaleChild']"));
            _driver.FindElement(By.XPath("//a[@data-title= \"Upper back injury with broken skin\"][@data-pathway-number= 'PW596FemaleChild']"));
            _driver.FindElement(By.XPath("//a[@data-title= \"Colds and flu\"][@data-pathway-number= 'PW1041FemaleChild']"));
            _driver.FindElement(By.XPath("//a[@data-title= \"Constipation\"][@data-pathway-number= 'PW1161FemaleChild']"));
            _driver.FindElement(By.XPath("//a[@data-title= \"Cough\"][@data-pathway-number= 'PW978FemaleChild']"));
            _driver.FindElement(By.XPath("//a[@data-title= \"Self-harm\"][@data-pathway-number= 'PW1543FemaleChild']"));
            _driver.FindElement(By.XPath("//a[@data-title= \"Dental injury\"][@data-pathway-number= 'PW620FemaleChild']"));
            _driver.FindElement(By.XPath("//a[@data-title= \"Toothache and other dental problems\"][@data-pathway-number= 'PW1611FemaleChild']"));
            _driver.FindElement(By.XPath("//a[@data-title= \"Diabetes - blood sugar problems\"][@data-pathway-number= 'PW1746FemaleChild']"));
            _driver.FindElement(By.XPath("//a[@data-title= \"Diarrhoea - no vomiting\"][@data-pathway-number= 'PW629FemaleChild']"));
            _driver.FindElement(By.XPath("//a[@data-title= \"Diarrhoea and vomiting\"][@data-pathway-number= 'PW1554FemaleChild']"));
            _driver.FindElement(By.XPath("//a[@data-title= \"Difficulty passing urine\"][@data-pathway-number= 'PW886FemaleChild']"));
            _driver.FindElement(By.XPath("//a[@data-title= \"Difficulty swallowing\"][@data-pathway-number= 'PW1496FemaleChild']"));
            _driver.FindElement(By.XPath("//a[@data-title= \"Dizziness\"][@data-pathway-number= 'PW637FemaleChild']"));
            _driver.FindElement(By.XPath("//a[@data-title= \"Vertigo\"][@data-pathway-number= 'PW637FemaleChild']"));
            _driver.FindElement(By.XPath("//a[@data-title= \"Ear discharge\"][@data-pathway-number= 'PW1702FemaleChild']"));
            _driver.FindElement(By.XPath("//a[@data-title= \"Earwax\"][@data-pathway-number= 'PW1702FemaleChild']"));
            _driver.FindElement(By.XPath("//a[@data-title= \"Earache\"][@data-pathway-number= 'PW656FemaleChild']"));
            _driver.FindElement(By.XPath("//a[@data-title= \"Eye injury - no damage to surface of eye\"][@data-pathway-number= 'PW660FemaleChild']"));
            _driver.FindElement(By.XPath("//a[@data-title= \"Eye injury with damage to surface of eye\"][@data-pathway-number= 'PW668FemaleChild']"));
            _driver.FindElement(By.XPath("//a[@data-title= \"Eye problems\"][@data-pathway-number= 'PW1628FemaleChild']"));
            _driver.FindElement(By.XPath("//a[@data-title= \"Eyelid problems\"][@data-pathway-number= 'PW1628FemaleChild']"));
            _driver.FindElement(By.XPath("//a[@data-title= \"Something in the eye\"][@data-pathway-number= 'PW1098FemaleChild']"));
            _driver.FindElement(By.XPath("//a[@data-title= \"Swollen face\"][@data-pathway-number= 'PW1548FemaleChild']"));
            _driver.FindElement(By.XPath("//a[@data-title= \"Painful face\"][@data-pathway-number= 'PW1548FemaleChild']"));
            _driver.FindElement(By.XPath("//a[@data-title= \"Swollen neck\"][@data-pathway-number= 'PW1548FemaleChild']"));
            _driver.FindElement(By.XPath("//a[@data-title= \"Painful neck\"][@data-pathway-number= 'PW1548FemaleChild']"));
            _driver.FindElement(By.XPath("//a[@data-title= \"Falling\"][@data-pathway-number= 'PW700FemaleChild']"));
            _driver.FindElement(By.XPath("//a[@data-title= \"Fainting\"][@data-pathway-number= 'PW700FemaleChild']"));
            _driver.FindElement(By.XPath("//a[@data-title= \"Passing out\"][@data-pathway-number= 'PW700FemaleChild']"));
            _driver.FindElement(By.XPath("//a[@data-title= \"Fever\"][@data-pathway-number= 'PW709FemaleChild']"));
            _driver.FindElement(By.XPath("//a[@data-title= \"High temperature\"][@data-pathway-number= 'PW709FemaleChild']"));
            _driver.FindElement(By.XPath("//a[@data-title= \"Finger injury - skin not broken\"][@data-pathway-number= 'PW1270FemaleChild']"));
            _driver.FindElement(By.XPath("//a[@data-title= \"Thumb injury - skin not broken\"][@data-pathway-number= 'PW1270FemaleChild']"));
            _driver.FindElement(By.XPath("//a[@data-title= \"Finger injury with broken skin\"][@data-pathway-number= 'PW1264FemaleChild']"));
            _driver.FindElement(By.XPath("//a[@data-title= \"Thumb injury with broken skin\"][@data-pathway-number= 'PW1264FemaleChild']"));
            _driver.FindElement(By.XPath("//a[@data-title= \"Fingernail injury\"][@data-pathway-number= 'PW1570FemaleChild']"));
            _driver.FindElement(By.XPath("//a[@data-title= \"Pain in the side of the body\"][@data-pathway-number= 'PW717FemaleChild']"));
            _driver.FindElement(By.XPath("//a[@data-title= \"Something in the ear\"][@data-pathway-number= 'PW1528FemaleChild']"));
            _driver.FindElement(By.XPath("//a[@data-title= \"Something in the nose\"][@data-pathway-number= 'PW1529FemaleChild']"));
            _driver.FindElement(By.XPath("//a[@data-title= \"Something in the bottom/back passage\"][@data-pathway-number= 'PW1531FemaleChild']"));
            _driver.FindElement(By.XPath("//a[@data-title= \"Something in the vagina\"][@data-pathway-number= 'PW1532FemaleChild']"));
            _driver.FindElement(By.XPath("//a[@data-title= \"Genital injury - skin not broken\"][@data-pathway-number= 'PW1116FemaleChild']"));
            _driver.FindElement(By.XPath("//a[@data-title= \"Genital injury with broken skin\"][@data-pathway-number= 'PW1010FemaleChild']"));
            _driver.FindElement(By.XPath("//a[@data-title= \"Swollen groin\"][@data-pathway-number= 'PW729FemaleChild']"));
            _driver.FindElement(By.XPath("//a[@data-title= \"Painful groin\"][@data-pathway-number= 'PW729FemaleChild']"));
            _driver.FindElement(By.XPath("//a[@data-title= \"Hair loss\"][@data-pathway-number= 'PW1678FemaleChild']"));
            _driver.FindElement(By.XPath("//a[@data-title= \"Hand injury - skin not broken\"][@data-pathway-number= 'PW1267FemaleChild']"));
            _driver.FindElement(By.XPath("//a[@data-title= \"Wrist injury - skin not broken\"][@data-pathway-number= 'PW1267FemaleChild']"));
            _driver.FindElement(By.XPath("//a[@data-title= \"Hand injury with broken skin\"][@data-pathway-number= 'PW1260FemaleChild']"));
            _driver.FindElement(By.XPath("//a[@data-title= \"Wrist injury with broken skin\"][@data-pathway-number= 'PW1260FemaleChild']"));
            _driver.FindElement(By.XPath("//a[@data-title= \"Head injury - skin not broken\"][@data-pathway-number= 'PW684FemaleChild']"));
            _driver.FindElement(By.XPath("//a[@data-title= \"Face injury - skin not broken\"][@data-pathway-number= 'PW684FemaleChild']"));
            _driver.FindElement(By.XPath("//a[@data-title= \"Neck injury - skin not broken\"][@data-pathway-number= 'PW684FemaleChild']"));
            _driver.FindElement(By.XPath("//a[@data-title= \"Head injury with broken skin\"][@data-pathway-number= 'PW692FemaleChild']"));
            _driver.FindElement(By.XPath("//a[@data-title= \"Face injury with broken skin\"][@data-pathway-number= 'PW692FemaleChild']"));
            _driver.FindElement(By.XPath("//a[@data-title= \"Neck injury with broken skin\"][@data-pathway-number= 'PW692FemaleChild']"));
            _driver.FindElement(By.XPath("//a[@data-title= \"Headache\"][@data-pathway-number= 'PW753FemaleChild']"));
            _driver.FindElement(By.XPath("//a[@data-title= \"Hearing problems\"][@data-pathway-number= 'PW1762FemaleChild']"));
            _driver.FindElement(By.XPath("//a[@data-title= \"Blocked ear\"][@data-pathway-number= 'PW1762FemaleChild']"));
            _driver.FindElement(By.XPath("//a[@data-title= \"Heatstroke\"][@data-pathway-number= 'PW998FemaleChild']"));
            _driver.FindElement(By.XPath("//a[@data-title= \"Heat exhaustion\"][@data-pathway-number= 'PW998FemaleChild']"));
            _driver.FindElement(By.XPath("//a[@data-title= \"Hiccups\"][@data-pathway-number= 'PW1775FemaleChild']"));
            _driver.FindElement(By.XPath("//a[@data-title= \"Leg injury - skin not broken\"][@data-pathway-number= 'PW1591FemaleChild']"));
            _driver.FindElement(By.XPath("//a[@data-title= \"Leg injury with broken skin\"][@data-pathway-number= 'PW1234FemaleChild']"));
            _driver.FindElement(By.XPath("//a[@data-title= \"Leg changing colour\"][@data-pathway-number= 'PW1734FemaleChild']"));
            _driver.FindElement(By.XPath("//a[@data-title= \"Cold leg\"][@data-pathway-number= 'PW1734FemaleChild']"));
            _driver.FindElement(By.XPath("//a[@data-title= \"Locked jaw\"][@data-pathway-number= 'PW1712FemaleChild']"));
            _driver.FindElement(By.XPath("//a[@data-title= \"Loss of bowel control\"][@data-pathway-number= 'PW1759FemaleChild']"));
            _driver.FindElement(By.XPath("//a[@data-title= \"Bowel incontinence\"][@data-pathway-number= 'PW1759FemaleChild']"));
            _driver.FindElement(By.XPath("//a[@data-title= \"Lower back injury - skin not broken\"][@data-pathway-number= 'PW1596FemaleChild']"));
            _driver.FindElement(By.XPath("//a[@data-title= \"Lower back injury with broken skin\"][@data-pathway-number= 'PW798FemaleChild']"));
            _driver.FindElement(By.XPath("//a[@data-title= \"Lower back pain\"][@data-pathway-number= 'PW783FemaleChild']"));
            _driver.FindElement(By.XPath("//a[@data-title= \"Painful legs\"][@data-pathway-number= 'PW775FemaleChild']"));
            _driver.FindElement(By.XPath("//a[@data-title= \"Swollen legs\"][@data-pathway-number= 'PW775FemaleChild']"));
            _driver.FindElement(By.XPath("//a[@data-title= \"Mental health problems\"][@data-pathway-number= 'PW1751FemaleChild']"));
            _driver.FindElement(By.XPath("//a[@data-title= \"Low mood\"][@data-pathway-number= 'PW1751FemaleChild']"));
            _driver.FindElement(By.XPath("//a[@data-title= \"Depression\"][@data-pathway-number= 'PW1751FemaleChild']"));
            _driver.FindElement(By.XPath("//a[@data-title= \"Anxiety\"][@data-pathway-number= 'PW1751FemaleChild']"));
            _driver.FindElement(By.XPath("//a[@data-title= \"Mouth ulcers\"][@data-pathway-number= 'PW1323FemaleChild']"));
            _driver.FindElement(By.XPath("//a[@data-title= \"Blocked nose\"][@data-pathway-number= 'PW984FemaleChild']"));
            _driver.FindElement(By.XPath("//a[@data-title= \"Nosebleed after an injury\"][@data-pathway-number= 'PW1713FemaleChild']"));
            _driver.FindElement(By.XPath("//a[@data-title= \"Nosebleed\"][@data-pathway-number= 'PW819FemaleChild']"));
            _driver.FindElement(By.XPath("//a[@data-title= \"Numbness\"][@data-pathway-number= 'PW1683FemaleChild']"));
            _driver.FindElement(By.XPath("//a[@data-title= \"Tingling\"][@data-pathway-number= 'PW1683FemaleChild']"));
            _driver.FindElement(By.XPath("//a[@data-title= \"Swallowed an object\"][@data-pathway-number= 'PW1034FemaleChild']"));
            _driver.FindElement(By.XPath("//a[@data-title= \"Breathed in an object\"][@data-pathway-number= 'PW1034FemaleChild']"));
            _driver.FindElement(By.XPath("//a[@data-title= \"Other symptoms\"][@data-pathway-number= 'PW1348FemaleChild']"));
            _driver.FindElement(By.XPath("//a[@data-title= \"Palpitations\"][@data-pathway-number= 'PW1029FemaleChild']"));
            _driver.FindElement(By.XPath("//a[@data-title= \"Pounding heart\"][@data-pathway-number= 'PW1029FemaleChild']"));
            _driver.FindElement(By.XPath("//a[@data-title= \"Fluttering heart\"][@data-pathway-number= 'PW1029FemaleChild']"));
            _driver.FindElement(By.XPath("//a[@data-title= \"Urinary problems\"][@data-pathway-number= 'PW645FemaleChild']"));
            _driver.FindElement(By.XPath("//a[@data-title= \"Bleeding from the bottom/back passage\"][@data-pathway-number= 'PW846FemaleChild']"));
            _driver.FindElement(By.XPath("//a[@data-title= \"Swollen bottom/back passage\"][@data-pathway-number= 'PW1091FemaleChild']"));
            _driver.FindElement(By.XPath("//a[@data-title= \"Painful bottom/back passage\"][@data-pathway-number= 'PW1091FemaleChild']"));
            _driver.FindElement(By.XPath("//a[@data-title= \"Itchy bottom/back passage\"][@data-pathway-number= 'PW1091FemaleChild']"));
            _driver.FindElement(By.XPath("//a[@data-title= \"Can't feel baby moving as much\"][@data-pathway-number= 'PW1763FemaleChild']"));
            _driver.FindElement(By.XPath("//a[@data-title= \"Grazes\"][@data-pathway-number= 'PW1590FemaleChild']"));
            _driver.FindElement(By.XPath("//a[@data-title= \"Minor wounds\"][@data-pathway-number= 'PW1590FemaleChild']"));
            _driver.FindElement(By.XPath("//a[@data-title= \"Scratches\"][@data-pathway-number= 'PW1590FemaleChild']"));
            _driver.FindElement(By.XPath("//a[@data-title= \"Sexual concerns - female\"][@data-pathway-number= 'PW1699FemaleChild']"));
            _driver.FindElement(By.XPath("//a[@data-title= \"Period concerns\"][@data-pathway-number= 'PW1699FemaleChild']"));
            _driver.FindElement(By.XPath("//a[@data-title= \"Shoulder pain\"][@data-pathway-number= 'PW1141FemaleChild']"));
            _driver.FindElement(By.XPath("//a[@data-title= \"Sinusitis\"][@data-pathway-number= 'PW1050FemaleChild']"));
            _driver.FindElement(By.XPath("//a[@data-title= \"Rashes, itching, spots, moles and other skin problems\"][@data-pathway-number= 'PW1772FemaleChild']"));
            _driver.FindElement(By.XPath("//a[@data-title= \"Glue on the skin\"][@data-pathway-number= 'PW1301FemaleChild']"));
            _driver.FindElement(By.XPath("//a[@data-title= \"Something under the skin\"][@data-pathway-number= 'PW1259FemaleChild']"));
            _driver.FindElement(By.XPath("//a[@data-title= \"Splinter\"][@data-pathway-number= 'PW1259FemaleChild']"));
            _driver.FindElement(By.XPath("//a[@data-title= \"Sleep problems\"][@data-pathway-number= 'PW1697FemaleChild']"));
            _driver.FindElement(By.XPath("//a[@data-title= \"Sore throat\"][@data-pathway-number= 'PW854FemaleChild']"));
            _driver.FindElement(By.XPath("//a[@data-title= \"Hoarse voice\"][@data-pathway-number= 'PW854FemaleChild']"));
            _driver.FindElement(By.XPath("//a[@data-title= \"Stoma problems\"][@data-pathway-number= 'PW1719FemaleChild']"));
            _driver.FindElement(By.XPath("//a[@data-title= \"Stroke symptoms\"][@data-pathway-number= 'PA171FemaleChild']"));
            _driver.FindElement(By.XPath("//a[@data-title= \"Tiredness\"][@data-pathway-number= 'PW1071FemaleChild']"));
            _driver.FindElement(By.XPath("//a[@data-title= \"Fatigue\"][@data-pathway-number= 'PW1071FemaleChild']"));
            _driver.FindElement(By.XPath("//a[@data-title= \"Toe injury - skin not broken\"][@data-pathway-number= 'PW1282FemaleChild']"));
            _driver.FindElement(By.XPath("//a[@data-title= \"Toe injury with broken skin\"][@data-pathway-number= 'PW1526FemaleChild']"));
            _driver.FindElement(By.XPath("//a[@data-title= \"Toenail injury\"][@data-pathway-number= 'PW1593FemaleChild']"));
            _driver.FindElement(By.XPath("//a[@data-title= \"Toothache after an injury\"][@data-pathway-number= 'PW572FemaleChild']"));
            _driver.FindElement(By.XPath("//a[@data-title= \"Blisters\"][@data-pathway-number= 'PW1625FemaleChild']"));
            _driver.FindElement(By.XPath("//a[@data-title= \"Trembling\"][@data-pathway-number= 'PW1764FemaleChild']"));
            _driver.FindElement(By.XPath("//a[@data-title= \"Shaking\"][@data-pathway-number= 'PW1764FemaleChild']"));
            _driver.FindElement(By.XPath("//a[@data-title= \"Bleeding from the vagina\"][@data-pathway-number= 'PW911FemaleChild']"));
            _driver.FindElement(By.XPath("//a[@data-title= \"Discharge from the vagina\"][@data-pathway-number= 'PW916FemaleChild']"));
            _driver.FindElement(By.XPath("//a[@data-title= \"Itchy vagina\"][@data-pathway-number= 'PW1560FemaleChild']"));
            _driver.FindElement(By.XPath("//a[@data-title= \"Sore vagina\"][@data-pathway-number= 'PW1560FemaleChild']"));
            _driver.FindElement(By.XPath("//a[@data-title= \"Swelling in or around the vagina\"][@data-pathway-number= 'PW1103FemaleChild']"));
            _driver.FindElement(By.XPath("//a[@data-title= \"Vomiting and nausea - no diarrhoea\"][@data-pathway-number= 'PW937FemaleChild']"));
            _driver.FindElement(By.XPath("//a[@data-title= \"Wound problems\"][@data-pathway-number= 'PW1776FemaleChild']"));
            _driver.FindElement(By.XPath("//a[@data-title= \"Wound problems - plaster casts\"][@data-pathway-number= 'PW1709FemaleChild']"));
            _driver.FindElement(By.XPath("//a[@data-title= \"Wound problems - drainage tubes\"][@data-pathway-number= 'PW1709FemaleChild']"));
            _driver.FindElement(By.XPath("//a[@data-title= \"Wound problems - metal attachments\"][@data-pathway-number= 'PW1709FemaleChild']"));
            _driver.FindElement(By.XPath("//a[@data-title= \"Swollen finger\"][@data-pathway-number= 'PW1616FemaleChild']"));
            _driver.FindElement(By.XPath("//a[@data-title= \"Painful finger\"][@data-pathway-number= 'PW1616FemaleChild']"));
            _driver.FindElement(By.XPath("//a[@data-title= \"Swollen hand\"][@data-pathway-number= 'PW1616FemaleChild']"));
            _driver.FindElement(By.XPath("//a[@data-title= \"Painful hand\"][@data-pathway-number= 'PW1616FemaleChild']"));
            _driver.FindElement(By.XPath("//a[@data-title= \"Swollen wrist\"][@data-pathway-number= 'PW1616FemaleChild']"));
            _driver.FindElement(By.XPath("//a[@data-title= \"Painful wrist\"][@data-pathway-number= 'PW1616FemaleChild']"));
            _driver.FindElement(By.XPath("//a[@data-title= 'Self-harm'][@data-pathway-number= 'PW1543FemaleChild']"));

            _driver.FindElement(By.Id("bowel-and-urinary-problems")).Click();
            _driver.FindElement(By.Id("bowel-and-urinary-problems-bowel-problems")).Click();
            var wait = new WebDriverWait(_driver, new TimeSpan(0, 0, 5));
            wait.Until(ExpectedConditions.VisibilityOfAllElementsLocatedBy((By.XPath("//a[@data-title= 'Something in the bottom/back passage']"))));
            _driver.FindElement(By.XPath("//a[@data-title= 'Something in the bottom/back passage']")).Click();

            _driver.FindElement(By.XPath("//input[@value = 'PW1531FemaleChild']"));
                  }
           [Test]
           public void CategoriespresentForFemaleAdult()
           //Test to show all categories are present per age and gender, and also the correct pathway opens per age and gender, when the same pathway is used for all ages/genders.
           {

               var searchPage = TestScenerios.LaunchSearchScenerio(_driver, TestScenerioGender.Female, 40);

               _driver.FindElement(By.Id("searchButton")).Click();

               _driver.FindElement(By.XPath("//a[@data-title=  \"Can't feel baby moving as much\"][@data-pathway-number= 'PW1763FemaleAdult']"));
               _driver.FindElement(By.XPath("//a[@data-title= 'Abdomen injury - skin not broken'][@data-pathway-number= 'PW500FemaleAdult']"));
               _driver.FindElement(By.XPath("//a[@data-title= 'Injury to the side of the body - skin not broken'][@data-pathway-number= 'PW500FemaleAdult']"));
               _driver.FindElement(By.XPath("//a[@data-title= 'Abdomen injury with broken skin'][@data-pathway-number= 'PW508FemaleAdult']"));
               _driver.FindElement(By.XPath("//a[@data-title= 'Injury to the side of the body with broken skin'][@data-pathway-number= 'PW508FemaleAdult']"));
               _driver.FindElement(By.XPath("//a[@data-title= 'Abdominal pain'][@data-pathway-number= 'PW516FemaleAdult']"));
               _driver.FindElement(By.XPath("//a[@data-title= 'Stomach pain'][@data-pathway-number= 'PW516FemaleAdult']"));
               _driver.FindElement(By.XPath("//a[@data-title= 'Absent periods'][@data-pathway-number= 'PW1676FemaleAdult']"));
               _driver.FindElement(By.XPath("//a[@data-title= 'Missed periods'][@data-pathway-number= 'PW1676FemaleAdult']"));
               _driver.FindElement(By.XPath("//a[@data-title= 'Accidental poisoning'][@data-pathway-number= 'PW881FemaleAdult']"));
               _driver.FindElement(By.XPath("//a[@data-title= 'Breathed in something poisonous'][@data-pathway-number= 'PW881FemaleAdult']"));
               _driver.FindElement(By.XPath("//a[@data-title= 'Swallowed something poisonous'][@data-pathway-number= 'PW881FemaleAdult']"));
               _driver.FindElement(By.XPath("//a[@data-title= 'Drunk too much alcohol'][@data-pathway-number= 'PW1551FemaleAdult']"));
               _driver.FindElement(By.XPath("//a[@data-title= 'Alcohol intoxication'][@data-pathway-number= 'PW1551FemaleAdult']"));
               _driver.FindElement(By.XPath("//a[@data-title= 'Ankle injury - skin not broken'][@data-pathway-number= 'PW1512FemaleAdult']"));
               _driver.FindElement(By.XPath("//a[@data-title= 'Foot injury - skin not broken'][@data-pathway-number= 'PW1512FemaleAdult']"));
               _driver.FindElement(By.XPath("//a[@data-title= 'Ankle injury with broken skin'][@data-pathway-number= 'PW1518FemaleAdult']"));
               _driver.FindElement(By.XPath("//a[@data-title= 'Foot injury with broken skin'][@data-pathway-number= 'PW1518FemaleAdult']"));
               _driver.FindElement(By.XPath("//a[@data-title= 'Arm injury - skin not broken'][@data-pathway-number= 'PW894FemaleAdult']"));
               _driver.FindElement(By.XPath("//a[@data-title= 'Arm injury with broken skin'][@data-pathway-number= 'PW902FemaleAdult']"));
               _driver.FindElement(By.XPath("//a[@data-title= 'Painful arm'][@data-pathway-number= 'PW1164FemaleAdult']"));
               _driver.FindElement(By.XPath("//a[@data-title= 'Swollen arm'][@data-pathway-number= 'PW1164FemaleAdult']"));
               _driver.FindElement(By.XPath("//a[@data-title= 'Cold arm'][@data-pathway-number= 'PW1733FemaleAdult']"));
               _driver.FindElement(By.XPath("//a[@data-title= 'Arm changing colour'][@data-pathway-number= 'PW1733FemaleAdult']"));
               _driver.FindElement(By.XPath("//a[@data-title= 'Change in behaviour'][@data-pathway-number= 'PW1749FemaleAdult']"));
               _driver.FindElement(By.XPath("//a[@data-title= 'Bites and stings'][@data-pathway-number= 'PW1575FemaleAdult']"));
               _driver.FindElement(By.XPath("//a[@data-title= 'Bleeding from stoma'][@data-pathway-number= 'PT507FemaleAdult']"));
               _driver.FindElement(By.XPath("//a[@data-title= 'Blood in urine'][@data-pathway-number= 'PW961FemaleAdult']"));
               _driver.FindElement(By.XPath("//a[@data-title= 'Breast problems'][@data-pathway-number= 'PW1603FemaleAdult']"));
               _driver.FindElement(By.XPath("//a[@data-title= 'Breastfeeding problems'][@data-pathway-number= 'PW1114FemaleAdult']"));
               _driver.FindElement(By.XPath("//a[@data-title= 'Breathing problems'][@data-pathway-number= 'PW556FemaleAdult']"));
               _driver.FindElement(By.XPath("//a[@data-title= 'Upper back pain'][@data-pathway-number= 'PW556FemaleAdult']"));
               _driver.FindElement(By.XPath("//a[@data-title= 'Chest and upper back pain'][@data-pathway-number= 'PW556FemaleAdult']"));
               _driver.FindElement(By.XPath("//a[@data-title= 'Chest pain'][@data-pathway-number= 'PW556FemaleAdult']"));
               _driver.FindElement(By.XPath("//a[@data-title= 'Wheezing'][@data-pathway-number= 'PW556FemaleAdult']"));
               _driver.FindElement(By.XPath("//a[@data-title= 'Coughing up blood'][@data-pathway-number= 'PW1651FemaleAdult']"));
               _driver.FindElement(By.XPath("//a[@data-title= 'Bringing up blood'][@data-pathway-number= 'PW1651FemaleAdult']"));
               _driver.FindElement(By.XPath("//a[@data-title= 'Burns from chemicals'][@data-pathway-number= 'PW564FemaleAdult']"));
               _driver.FindElement(By.XPath("//a[@data-title= 'Sunburn'][@data-pathway-number= 'PW987FemaleAdult']"));
               _driver.FindElement(By.XPath("//a[@data-title= 'Burns from heat'][@data-pathway-number= 'PW580FemaleAdult']"));
               _driver.FindElement(By.XPath("//a[@data-title= 'Catheter problems'][@data-pathway-number= 'PW1567FemaleAdult']"));
               _driver.FindElement(By.XPath("//a[@data-title= 'Chest injury - skin not broken'][@data-pathway-number= 'PW588FemaleAdult']"));
               _driver.FindElement(By.XPath("//a[@data-title= 'Upper back injury - skin not broken'][@data-pathway-number= 'PW588FemaleAdult']"));
               _driver.FindElement(By.XPath("//a[@data-title= 'Chest injury with broken skin'][@data-pathway-number= 'PW596FemaleAdult']"));
               _driver.FindElement(By.XPath("//a[@data-title= 'Upper back injury with broken skin'][@data-pathway-number= 'PW596FemaleAdult']"));
               _driver.FindElement(By.XPath("//a[@data-title= 'Colds and flu'][@data-pathway-number= 'PW1040FemaleAdult']"));
               _driver.FindElement(By.XPath("//a[@data-title= 'Constipation'][@data-pathway-number= 'PW1158FemaleAdult']"));
               _driver.FindElement(By.XPath("//a[@data-title= 'Cough'][@data-pathway-number= 'PW975FemaleAdult']"));
               _driver.FindElement(By.XPath("//a[@data-title= 'Self-harm'][@data-pathway-number= 'PW1543FemaleAdult']"));
               _driver.FindElement(By.XPath("//a[@data-title= 'Dental injury'][@data-pathway-number= 'PW620FemaleAdult']"));
               _driver.FindElement(By.XPath("//a[@data-title= 'Toothache and other dental problems'][@data-pathway-number= 'PW1610FemaleAdult']"));
               _driver.FindElement(By.XPath("//a[@data-title= 'Diabetes - blood sugar problems'][@data-pathway-number= 'PW1746FemaleAdult']"));
               _driver.FindElement(By.XPath("//a[@data-title= 'Diarrhoea - no vomiting'][@data-pathway-number= 'PW628FemaleAdult']"));
               _driver.FindElement(By.XPath("//a[@data-title= 'Diarrhoea and vomiting'][@data-pathway-number= 'PW1553FemaleAdult']"));
               _driver.FindElement(By.XPath("//a[@data-title= 'Difficulty passing urine'][@data-pathway-number= 'PW886FemaleAdult']"));
               _driver.FindElement(By.XPath("//a[@data-title= 'Difficulty swallowing'][@data-pathway-number= 'PW1496FemaleAdult']"));
               _driver.FindElement(By.XPath("//a[@data-title= 'Dizziness'][@data-pathway-number= 'PW636FemaleAdult']"));
               _driver.FindElement(By.XPath("//a[@data-title= 'Vertigo'][@data-pathway-number= 'PW636FemaleAdult']"));
               _driver.FindElement(By.XPath("//a[@data-title= 'Ear discharge'][@data-pathway-number= 'PW1702FemaleAdult']"));
               _driver.FindElement(By.XPath("//a[@data-title= 'Earwax'][@data-pathway-number= 'PW1702FemaleAdult']"));
               _driver.FindElement(By.XPath("//a[@data-title= 'Earache'][@data-pathway-number= 'PW655FemaleAdult']"));
               _driver.FindElement(By.XPath("//a[@data-title= 'Eye injury - no damage to surface of eye'][@data-pathway-number= 'PW660FemaleAdult']"));
               _driver.FindElement(By.XPath("//a[@data-title= 'Eye injury with damage to surface of eye'][@data-pathway-number= 'PW668FemaleAdult']"));
               _driver.FindElement(By.XPath("//a[@data-title= 'Eye problems'][@data-pathway-number= 'PW1627FemaleAdult']"));
               _driver.FindElement(By.XPath("//a[@data-title= 'Eyelid problems'][@data-pathway-number= 'PW1627FemaleAdult']"));
               _driver.FindElement(By.XPath("//a[@data-title= 'Something in the eye'][@data-pathway-number= 'PW1098FemaleAdult']"));
               _driver.FindElement(By.XPath("//a[@data-title= 'Swollen face'][@data-pathway-number= 'PW1544FemaleAdult']"));
               _driver.FindElement(By.XPath("//a[@data-title= 'Painful face'][@data-pathway-number= 'PW1544FemaleAdult']"));
               _driver.FindElement(By.XPath("//a[@data-title= 'Swollen neck'][@data-pathway-number= 'PW1544FemaleAdult']"));
               _driver.FindElement(By.XPath("//a[@data-title= 'Painful neck'][@data-pathway-number= 'PW1544FemaleAdult']"));
               _driver.FindElement(By.XPath("//a[@data-title= 'Falling'][@data-pathway-number= 'PW700FemaleAdult']"));
               _driver.FindElement(By.XPath("//a[@data-title= 'Fainting'][@data-pathway-number= 'PW700FemaleAdult']"));
               _driver.FindElement(By.XPath("//a[@data-title= 'Passing out'][@data-pathway-number= 'PW700FemaleAdult']"));
               _driver.FindElement(By.XPath("//a[@data-title= 'Fever'][@data-pathway-number= 'PW708FemaleAdult']"));
               _driver.FindElement(By.XPath("//a[@data-title= 'High temperature'][@data-pathway-number= 'PW708FemaleAdult']"));
               _driver.FindElement(By.XPath("//a[@data-title= 'Finger injury - skin not broken'][@data-pathway-number= 'PW1270FemaleAdult']"));
               _driver.FindElement(By.XPath("//a[@data-title= 'Thumb injury - skin not broken'][@data-pathway-number= 'PW1270FemaleAdult']"));
               _driver.FindElement(By.XPath("//a[@data-title= 'Finger injury with broken skin'][@data-pathway-number= 'PW1264FemaleAdult']"));
               _driver.FindElement(By.XPath("//a[@data-title= 'Thumb injury with broken skin'][@data-pathway-number= 'PW1264FemaleAdult']"));
               _driver.FindElement(By.XPath("//a[@data-title= 'Fingernail injury'][@data-pathway-number= 'PW1570FemaleAdult']"));
               _driver.FindElement(By.XPath("//a[@data-title= 'Pain in the side of the body'][@data-pathway-number= 'PW716FemaleAdult']"));
               _driver.FindElement(By.XPath("//a[@data-title= 'Something in the ear'][@data-pathway-number= 'PW1528FemaleAdult']"));
               _driver.FindElement(By.XPath("//a[@data-title= 'Something in the nose'][@data-pathway-number= 'PW1529FemaleAdult']"));
               _driver.FindElement(By.XPath("//a[@data-title= 'Something in the bottom/back passage'][@data-pathway-number= 'PW1531FemaleAdult']"));
               _driver.FindElement(By.XPath("//a[@data-title= 'Something in the vagina'][@data-pathway-number= 'PW1532FemaleAdult']"));
               _driver.FindElement(By.XPath("//a[@data-title= 'Genital injury - skin not broken'][@data-pathway-number= 'PW1116FemaleAdult']"));
               _driver.FindElement(By.XPath("//a[@data-title= 'Genital injury with broken skin'][@data-pathway-number= 'PW1010FemaleAdult']"));
               _driver.FindElement(By.XPath("//a[@data-title= 'Swollen groin'][@data-pathway-number= 'PW728FemaleAdult']"));
               _driver.FindElement(By.XPath("//a[@data-title= 'Painful groin'][@data-pathway-number= 'PW728FemaleAdult']"));
               _driver.FindElement(By.XPath("//a[@data-title= 'Hair loss'][@data-pathway-number= 'PW1678FemaleAdult']"));
               _driver.FindElement(By.XPath("//a[@data-title= 'Hand injury - skin not broken'][@data-pathway-number= 'PW1267FemaleAdult']"));
               _driver.FindElement(By.XPath("//a[@data-title= 'Wrist injury - skin not broken'][@data-pathway-number= 'PW1267FemaleAdult']"));
               _driver.FindElement(By.XPath("//a[@data-title= 'Hand injury with broken skin'][@data-pathway-number= 'PW1260FemaleAdult']"));
               _driver.FindElement(By.XPath("//a[@data-title= 'Wrist injury with broken skin'][@data-pathway-number= 'PW1260FemaleAdult']"));
               _driver.FindElement(By.XPath("//a[@data-title= 'Head injury - skin not broken'][@data-pathway-number= 'PW684FemaleAdult']"));
               _driver.FindElement(By.XPath("//a[@data-title= 'Face injury - skin not broken'][@data-pathway-number= 'PW684FemaleAdult']"));
               _driver.FindElement(By.XPath("//a[@data-title= 'Neck injury - skin not broken'][@data-pathway-number= 'PW684FemaleAdult']"));
               _driver.FindElement(By.XPath("//a[@data-title= 'Head injury with broken skin'][@data-pathway-number= 'PW692FemaleAdult']"));
               _driver.FindElement(By.XPath("//a[@data-title= 'Face injury with broken skin'][@data-pathway-number= 'PW692FemaleAdult']"));
               _driver.FindElement(By.XPath("//a[@data-title= 'Neck injury with broken skin'][@data-pathway-number= 'PW692FemaleAdult']"));
               _driver.FindElement(By.XPath("//a[@data-title= 'Headache'][@data-pathway-number= 'PW752FemaleAdult']"));
               _driver.FindElement(By.XPath("//a[@data-title= 'Hearing problems'][@data-pathway-number= 'PW1762FemaleAdult']"));
               _driver.FindElement(By.XPath("//a[@data-title= 'Blocked ear'][@data-pathway-number= 'PW1762FemaleAdult']"));
               _driver.FindElement(By.XPath("//a[@data-title= 'Heatstroke'][@data-pathway-number= 'PW998FemaleAdult']"));
               _driver.FindElement(By.XPath("//a[@data-title= 'Heat exhaustion'][@data-pathway-number= 'PW998FemaleAdult']"));
               _driver.FindElement(By.XPath("//a[@data-title= 'Hiccups'][@data-pathway-number= 'PW1775FemaleAdult']"));
               _driver.FindElement(By.XPath("//a[@data-title= 'Leg injury - skin not broken'][@data-pathway-number= 'PW1240FemaleAdult']"));
               _driver.FindElement(By.XPath("//a[@data-title= 'Leg injury with broken skin'][@data-pathway-number= 'PW1234FemaleAdult']"));
               _driver.FindElement(By.XPath("//a[@data-title= 'Leg changing colour'][@data-pathway-number= 'PW1734FemaleAdult']"));
               _driver.FindElement(By.XPath("//a[@data-title= 'Cold leg'][@data-pathway-number= 'PW1734FemaleAdult']"));
               _driver.FindElement(By.XPath("//a[@data-title= 'Locked jaw'][@data-pathway-number= 'PW1712FemaleAdult']"));
               _driver.FindElement(By.XPath("//a[@data-title= 'Loss of bowel control'][@data-pathway-number= 'PW1759FemaleAdult']"));
               _driver.FindElement(By.XPath("//a[@data-title= 'Bowel incontinence'][@data-pathway-number= 'PW1759FemaleAdult']"));
               _driver.FindElement(By.XPath("//a[@data-title= 'Lower back injury - skin not broken'][@data-pathway-number= 'PW790FemaleAdult']"));
               _driver.FindElement(By.XPath("//a[@data-title= 'Lower back injury with broken skin'][@data-pathway-number= 'PW798FemaleAdult']"));
               _driver.FindElement(By.XPath("//a[@data-title= 'Lower back pain'][@data-pathway-number= 'PW782FemaleAdult']"));
               _driver.FindElement(By.XPath("//a[@data-title= 'Painful legs'][@data-pathway-number= 'PW774FemaleAdult']"));
               _driver.FindElement(By.XPath("//a[@data-title= 'Swollen legs'][@data-pathway-number= 'PW774FemaleAdult']"));
               _driver.FindElement(By.XPath("//a[@data-title= 'Mental health problems'][@data-pathway-number= 'PW1751FemaleAdult']"));
               _driver.FindElement(By.XPath("//a[@data-title= 'Low mood'][@data-pathway-number= 'PW1751FemaleAdult']"));
               _driver.FindElement(By.XPath("//a[@data-title= 'Depression'][@data-pathway-number= 'PW1751FemaleAdult']"));
               _driver.FindElement(By.XPath("//a[@data-title= 'Anxiety'][@data-pathway-number= 'PW1751FemaleAdult']"));
               _driver.FindElement(By.XPath("//a[@data-title= 'Mouth ulcers'][@data-pathway-number= 'PW1323FemaleAdult']"));
               _driver.FindElement(By.XPath("//a[@data-title= 'Blocked nose'][@data-pathway-number= 'PW981FemaleAdult']"));
               _driver.FindElement(By.XPath("//a[@data-title= 'Nosebleed after an injury'][@data-pathway-number= 'PW1713FemaleAdult']"));
               _driver.FindElement(By.XPath("//a[@data-title= 'Nosebleed'][@data-pathway-number= 'PW818FemaleAdult']"));
               _driver.FindElement(By.XPath("//a[@data-title= 'Numbness'][@data-pathway-number= 'PW1683FemaleAdult']"));
               _driver.FindElement(By.XPath("//a[@data-title= 'Tingling'][@data-pathway-number= 'PW1683FemaleAdult']"));
               _driver.FindElement(By.XPath("//a[@data-title= 'Swallowed an object'][@data-pathway-number= 'PW1034FemaleAdult']"));
               _driver.FindElement(By.XPath("//a[@data-title= 'Breathed in an object'][@data-pathway-number= 'PW1034FemaleAdult']"));
               _driver.FindElement(By.XPath("//a[@data-title= 'Other symptoms'][@data-pathway-number= 'PW1345FemaleAdult']"));
               _driver.FindElement(By.XPath("//a[@data-title= 'Palpitations'][@data-pathway-number= 'PW1028FemaleAdult']"));
               _driver.FindElement(By.XPath("//a[@data-title= 'Pounding heart'][@data-pathway-number= 'PW1028FemaleAdult']"));
               _driver.FindElement(By.XPath("//a[@data-title= 'Fluttering heart'][@data-pathway-number= 'PW1028FemaleAdult']"));
               _driver.FindElement(By.XPath("//a[@data-title= 'Urinary problems'][@data-pathway-number= 'PW644FemaleAdult']"));
               _driver.FindElement(By.XPath("//a[@data-title= 'Bleeding from the bottom/back passage'][@data-pathway-number= 'PW846FemaleAdult']"));
               _driver.FindElement(By.XPath("//a[@data-title= 'Swollen bottom/back passage'][@data-pathway-number= 'PW1091FemaleAdult']"));
               _driver.FindElement(By.XPath("//a[@data-title= 'Painful bottom/back passage'][@data-pathway-number= 'PW1091FemaleAdult']"));
               _driver.FindElement(By.XPath("//a[@data-title= 'Itchy bottom/back passage'][@data-pathway-number= 'PW1091FemaleAdult']"));
               _driver.FindElement(By.XPath("//a[@data-title= 'Grazes'][@data-pathway-number= 'PW1590FemaleAdult']"));
               _driver.FindElement(By.XPath("//a[@data-title= 'Minor wounds'][@data-pathway-number= 'PW1590FemaleAdult']"));
               _driver.FindElement(By.XPath("//a[@data-title= 'Scratches'][@data-pathway-number= 'PW1590FemaleAdult']"));
               _driver.FindElement(By.XPath("//a[@data-title= 'Sexual concerns - female'][@data-pathway-number= 'PW1684FemaleAdult']"));
               _driver.FindElement(By.XPath("//a[@data-title= 'Period concerns'][@data-pathway-number= 'PW1684FemaleAdult']"));
               _driver.FindElement(By.XPath("//a[@data-title= 'Shoulder pain'][@data-pathway-number= 'PW1140FemaleAdult']"));
               _driver.FindElement(By.XPath("//a[@data-title= 'Sinusitis'][@data-pathway-number= 'PW1046FemaleAdult']"));
               _driver.FindElement(By.XPath("//a[@data-title= 'Rashes, itching, spots, moles and other skin problems'][@data-pathway-number= 'PW1771FemaleAdult']"));
               _driver.FindElement(By.XPath("//a[@data-title= 'Glue on the skin'][@data-pathway-number= 'PW1301FemaleAdult']"));
               _driver.FindElement(By.XPath("//a[@data-title= 'Something under the skin'][@data-pathway-number= 'PW1259FemaleAdult']"));
               _driver.FindElement(By.XPath("//a[@data-title= 'Splinter'][@data-pathway-number= 'PW1259FemaleAdult']"));
               _driver.FindElement(By.XPath("//a[@data-title= 'Sleep problems'][@data-pathway-number= 'PW1686FemaleAdult']"));
               _driver.FindElement(By.XPath("//a[@data-title= 'Sore throat'][@data-pathway-number= 'PW854FemaleAdult']"));
               _driver.FindElement(By.XPath("//a[@data-title= 'Hoarse voice'][@data-pathway-number= 'PW854FemaleAdult']"));
               _driver.FindElement(By.XPath("//a[@data-title= 'Stoma problems'][@data-pathway-number= 'PW1719FemaleAdult']"));
               _driver.FindElement(By.XPath("//a[@data-title= 'Tiredness'][@data-pathway-number= 'PW1070FemaleAdult']"));
               _driver.FindElement(By.XPath("//a[@data-title= 'Fatigue'][@data-pathway-number= 'PW1070FemaleAdult']"));
               _driver.FindElement(By.XPath("//a[@data-title= 'Toe injury - skin not broken'][@data-pathway-number= 'PW1282FemaleAdult']"));
               _driver.FindElement(By.XPath("//a[@data-title= 'Toe injury with broken skin'][@data-pathway-number= 'PW1526FemaleAdult']"));
               _driver.FindElement(By.XPath("//a[@data-title= 'Toenail injury'][@data-pathway-number= 'PW1593FemaleAdult']"));
               _driver.FindElement(By.XPath("//a[@data-title= 'Toothache after an injury'][@data-pathway-number= 'PW572FemaleAdult']"));
               _driver.FindElement(By.XPath("//a[@data-title= 'Blisters'][@data-pathway-number= 'PW1625FemaleAdult']"));
               _driver.FindElement(By.XPath("//a[@data-title= 'Trembling'][@data-pathway-number= 'PW1764FemaleAdult']"));
               _driver.FindElement(By.XPath("//a[@data-title= 'Shaking'][@data-pathway-number= 'PW1764FemaleAdult']"));
               _driver.FindElement(By.XPath("//a[@data-title= 'Bleeding from the vagina'][@data-pathway-number= 'PW910FemaleAdult']"));
               _driver.FindElement(By.XPath("//a[@data-title= 'Discharge from the vagina'][@data-pathway-number= 'PW915FemaleAdult']"));
               _driver.FindElement(By.XPath("//a[@data-title= 'Itchy vagina'][@data-pathway-number= 'PW1559FemaleAdult']"));
               _driver.FindElement(By.XPath("//a[@data-title= 'Sore vagina'][@data-pathway-number= 'PW1559FemaleAdult']"));
               _driver.FindElement(By.XPath("//a[@data-title= 'Swelling in or around the vagina'][@data-pathway-number= 'PW1102FemaleAdult']"));
               _driver.FindElement(By.XPath("//a[@data-title= 'Vomiting and nausea - no diarrhoea'][@data-pathway-number= 'PW936FemaleAdult']"));
               _driver.FindElement(By.XPath("//a[@data-title= 'Wound problems'][@data-pathway-number= 'PW1776FemaleAdult']"));
               _driver.FindElement(By.XPath("//a[@data-title= 'Wound problems - plaster casts'][@data-pathway-number= 'PW1709FemaleAdult']"));
               _driver.FindElement(By.XPath("//a[@data-title= 'Wound problems - drainage tubes'][@data-pathway-number= 'PW1709FemaleAdult']"));
               _driver.FindElement(By.XPath("//a[@data-title= 'Wound problems - metal attachments'][@data-pathway-number= 'PW1709FemaleAdult']"));
               _driver.FindElement(By.XPath("//a[@data-title= 'Swollen finger'][@data-pathway-number= 'PW1614FemaleAdult']"));
               _driver.FindElement(By.XPath("//a[@data-title= 'Painful finger'][@data-pathway-number= 'PW1614FemaleAdult']"));
               _driver.FindElement(By.XPath("//a[@data-title= 'Swollen hand'][@data-pathway-number= 'PW1614FemaleAdult']"));
               _driver.FindElement(By.XPath("//a[@data-title= 'Painful hand'][@data-pathway-number= 'PW1614FemaleAdult']"));
               _driver.FindElement(By.XPath("//a[@data-title= 'Swollen wrist'][@data-pathway-number= 'PW1614FemaleAdult']"));
               _driver.FindElement(By.XPath("//a[@data-title= 'Painful wrist'][@data-pathway-number= 'PW1614FemaleAdult']"));
               _driver.FindElement(By.XPath("//a[@data-title= 'Self-harm'][@data-pathway-number= 'PW1543FemaleAdult']"));

               _driver.FindElement(By.Id("bowel-and-urinary-problems")).Click();
               _driver.FindElement(By.Id("bowel-and-urinary-problems-bowel-problems")).Click();
               var wait = new WebDriverWait(_driver, new TimeSpan(0, 0, 5));
               wait.Until(ExpectedConditions.VisibilityOfAllElementsLocatedBy((By.XPath("//a[@data-title= 'Something in the bottom/back passage']"))));
               _driver.FindElement(By.XPath("//a[@data-title= 'Something in the bottom/back passage']")).Click();

               _driver.FindElement(By.XPath("//input[@value = 'PW1531FemaleAdult']"));
           }

           [Test]
           public void CategoriespresentForMaleChild()

                   //Test to show all categories are present per age and gender, and also the correct pathway opens per age and gender, when the same pathway is used for all ages/genders.
           {

               var searchPage = TestScenerios.LaunchSearchScenerio(_driver, TestScenerioGender.Male, 5);

               _driver.FindElement(By.Id("searchButton")).Click();

               _driver.FindElement(By.XPath("//a[@data-title= 'Abdomen injury - skin not broken'][@data-pathway-number= 'PW503MaleChild']"));
               _driver.FindElement(By.XPath("//a[@data-title= 'Injury to the side of the body - skin not broken'][@data-pathway-number= 'PW503MaleChild']"));
               _driver.FindElement(By.XPath("//a[@data-title= 'Abdomen injury with broken skin'][@data-pathway-number= 'PW511MaleChild']"));
               _driver.FindElement(By.XPath("//a[@data-title= 'Injury to the side of the body with broken skin'][@data-pathway-number= 'PW511MaleChild']"));
               _driver.FindElement(By.XPath("//a[@data-title= 'Abdominal pain'][@data-pathway-number= 'PW520MaleChild']"));
               _driver.FindElement(By.XPath("//a[@data-title= 'Stomach pain'][@data-pathway-number= 'PW520MaleChild']"));
               _driver.FindElement(By.XPath("//a[@data-title= 'Accidental poisoning'][@data-pathway-number= 'PW881MaleChild']"));
               _driver.FindElement(By.XPath("//a[@data-title= 'Breathed in something poisonous'][@data-pathway-number= 'PW881MaleChild']"));
               _driver.FindElement(By.XPath("//a[@data-title= 'Swallowed something poisonous'][@data-pathway-number= 'PW881MaleChild']"));
               _driver.FindElement(By.XPath("//a[@data-title= 'Drunk too much alcohol'][@data-pathway-number= 'PW1552MaleChild']"));
               _driver.FindElement(By.XPath("//a[@data-title= 'Alcohol intoxication'][@data-pathway-number= 'PW1552MaleChild']"));
               _driver.FindElement(By.XPath("//a[@data-title= 'Ankle injury - skin not broken'][@data-pathway-number= 'PW1512MaleChild']"));
               _driver.FindElement(By.XPath("//a[@data-title= 'Foot injury - skin not broken'][@data-pathway-number= 'PW1512MaleChild']"));
               _driver.FindElement(By.XPath("//a[@data-title= 'Ankle injury with broken skin'][@data-pathway-number= 'PW1518MaleChild']"));
               _driver.FindElement(By.XPath("//a[@data-title= 'Foot injury with broken skin'][@data-pathway-number= 'PW1518MaleChild']"));
               _driver.FindElement(By.XPath("//a[@data-title= 'Arm injury - skin not broken'][@data-pathway-number= 'PW895MaleChild']"));
               _driver.FindElement(By.XPath("//a[@data-title= 'Arm injury with broken skin'][@data-pathway-number= 'PW902MaleChild']"));
               _driver.FindElement(By.XPath("//a[@data-title= 'Painful arm'][@data-pathway-number= 'PW1167MaleChild']"));
               _driver.FindElement(By.XPath("//a[@data-title= 'Swollen arm'][@data-pathway-number= 'PW1167MaleChild']"));
               _driver.FindElement(By.XPath("//a[@data-title= 'Cold arm'][@data-pathway-number= 'PW1733MaleChild']"));
               _driver.FindElement(By.XPath("//a[@data-title= 'Arm changing colour'][@data-pathway-number= 'PW1733MaleChild']"));
               _driver.FindElement(By.XPath("//a[@data-title= 'Change in behaviour'][@data-pathway-number= 'PW1749MaleChild']"));
               _driver.FindElement(By.XPath("//a[@data-title= 'Bites and stings'][@data-pathway-number= 'PW1575MaleChild']"));
               _driver.FindElement(By.XPath("//a[@data-title= 'Bleeding from stoma'][@data-pathway-number= 'PT507MaleChild']"));
               _driver.FindElement(By.XPath("//a[@data-title= 'Blood in urine'][@data-pathway-number= 'PW962MaleChild']"));
               _driver.FindElement(By.XPath("//a[@data-title= 'Breast problems'][@data-pathway-number= 'PW1606MaleChild']"));
               _driver.FindElement(By.XPath("//a[@data-title= 'Breathing problems'][@data-pathway-number= 'PW560MaleChild']"));
               _driver.FindElement(By.XPath("//a[@data-title= 'Upper back pain'][@data-pathway-number= 'PW560MaleChild']"));
               _driver.FindElement(By.XPath("//a[@data-title= 'Chest and upper back pain'][@data-pathway-number= 'PW560MaleChild']"));
               _driver.FindElement(By.XPath("//a[@data-title= 'Chest pain'][@data-pathway-number= 'PW560MaleChild']"));
               _driver.FindElement(By.XPath("//a[@data-title= 'Wheezing'][@data-pathway-number= 'PW560MaleChild']"));
               _driver.FindElement(By.XPath("//a[@data-title= 'Coughing up blood'][@data-pathway-number= 'PW1654MaleChild']"));
               _driver.FindElement(By.XPath("//a[@data-title= 'Bringing up blood'][@data-pathway-number= 'PW1654MaleChild']"));
               _driver.FindElement(By.XPath("//a[@data-title= 'Burns from chemicals'][@data-pathway-number= 'PW564MaleChild']"));
               _driver.FindElement(By.XPath("//a[@data-title= 'Sunburn'][@data-pathway-number= 'PW987MaleChild']"));
               _driver.FindElement(By.XPath("//a[@data-title= 'Burns from heat'][@data-pathway-number= 'PW580MaleChild']"));
               _driver.FindElement(By.XPath("//a[@data-title= 'Catheter problems'][@data-pathway-number= 'PW1567MaleChild']"));
               _driver.FindElement(By.XPath("//a[@data-title= 'Chest injury - skin not broken'][@data-pathway-number= 'PW588MaleChild']"));
               _driver.FindElement(By.XPath("//a[@data-title= 'Upper back injury - skin not broken'][@data-pathway-number= 'PW588MaleChild']"));
               _driver.FindElement(By.XPath("//a[@data-title= 'Chest injury with broken skin'][@data-pathway-number= 'PW596MaleChild']"));
               _driver.FindElement(By.XPath("//a[@data-title= 'Upper back injury with broken skin'][@data-pathway-number= 'PW596MaleChild']"));
               _driver.FindElement(By.XPath("//a[@data-title= 'Colds and flu'][@data-pathway-number= 'PW1043MaleChild']"));
               _driver.FindElement(By.XPath("//a[@data-title= 'Constipation'][@data-pathway-number= 'PW1162MaleChild']"));
               _driver.FindElement(By.XPath("//a[@data-title= 'Cough'][@data-pathway-number= 'PW979MaleChild']"));
               _driver.FindElement(By.XPath("//a[@data-title= 'Self-harm'][@data-pathway-number= 'PW1543MaleChild']"));
               _driver.FindElement(By.XPath("//a[@data-title= 'Dental injury'][@data-pathway-number= 'PW620MaleChild']"));
               _driver.FindElement(By.XPath("//a[@data-title= 'Toothache and other dental problems'][@data-pathway-number= 'PW1611MaleChild']"));
               _driver.FindElement(By.XPath("//a[@data-title= 'Diabetes - blood sugar problems'][@data-pathway-number= 'PW1746MaleChild']"));
               _driver.FindElement(By.XPath("//a[@data-title= 'Diarrhoea - no vomiting'][@data-pathway-number= 'PW632MaleChild']"));
               _driver.FindElement(By.XPath("//a[@data-title= 'Diarrhoea and vomiting'][@data-pathway-number= 'PW1556MaleChild']"));
               _driver.FindElement(By.XPath("//a[@data-title= 'Difficulty passing urine'][@data-pathway-number= 'PW886MaleChild']"));
               _driver.FindElement(By.XPath("//a[@data-title= 'Difficulty swallowing'][@data-pathway-number= 'PW1496MaleChild']"));
               _driver.FindElement(By.XPath("//a[@data-title= 'Dizziness'][@data-pathway-number= 'PW640MaleChild']"));
               _driver.FindElement(By.XPath("//a[@data-title= 'Vertigo'][@data-pathway-number= 'PW640MaleChild']"));
               _driver.FindElement(By.XPath("//a[@data-title= 'Ear discharge'][@data-pathway-number= 'PW1702MaleChild']"));
               _driver.FindElement(By.XPath("//a[@data-title= 'Earwax'][@data-pathway-number= 'PW1702MaleChild']"));
               _driver.FindElement(By.XPath("//a[@data-title= 'Earache'][@data-pathway-number= 'PW656MaleChild']"));
               _driver.FindElement(By.XPath("//a[@data-title= 'Eye injury - no damage to surface of eye'][@data-pathway-number= 'PW660MaleChild']"));
               _driver.FindElement(By.XPath("//a[@data-title= 'Eye injury with damage to surface of eye'][@data-pathway-number= 'PW668MaleChild']"));
               _driver.FindElement(By.XPath("//a[@data-title= 'Eye problems'][@data-pathway-number= 'PW1630MaleChild']"));
               _driver.FindElement(By.XPath("//a[@data-title= 'Eyelid problems'][@data-pathway-number= 'PW1630MaleChild']"));
               _driver.FindElement(By.XPath("//a[@data-title= 'Something in the eye'][@data-pathway-number= 'PW1098MaleChild']"));
               _driver.FindElement(By.XPath("//a[@data-title= 'Swollen face'][@data-pathway-number= 'PW1549MaleChild']"));
               _driver.FindElement(By.XPath("//a[@data-title= 'Painful face'][@data-pathway-number= 'PW1549MaleChild']"));
               _driver.FindElement(By.XPath("//a[@data-title= 'Swollen neck'][@data-pathway-number= 'PW1549MaleChild']"));
               _driver.FindElement(By.XPath("//a[@data-title= 'Painful neck'][@data-pathway-number= 'PW1549MaleChild']"));
               _driver.FindElement(By.XPath("//a[@data-title= 'Falling'][@data-pathway-number= 'PW700MaleChild']"));
               _driver.FindElement(By.XPath("//a[@data-title= 'Fainting'][@data-pathway-number= 'PW700MaleChild']"));
               _driver.FindElement(By.XPath("//a[@data-title= 'Passing out'][@data-pathway-number= 'PW700MaleChild']"));
               _driver.FindElement(By.XPath("//a[@data-title= 'Fever'][@data-pathway-number= 'PW712MaleChild']"));
               _driver.FindElement(By.XPath("//a[@data-title= 'High temperature'][@data-pathway-number= 'PW712MaleChild']"));
               _driver.FindElement(By.XPath("//a[@data-title= 'Finger injury - skin not broken'][@data-pathway-number= 'PW1270MaleChild']"));
               _driver.FindElement(By.XPath("//a[@data-title= 'Thumb injury - skin not broken'][@data-pathway-number= 'PW1270MaleChild']"));
               _driver.FindElement(By.XPath("//a[@data-title= 'Finger injury with broken skin'][@data-pathway-number= 'PW1264MaleChild']"));
               _driver.FindElement(By.XPath("//a[@data-title= 'Thumb injury with broken skin'][@data-pathway-number= 'PW1264MaleChild']"));
               _driver.FindElement(By.XPath("//a[@data-title= 'Fingernail injury'][@data-pathway-number= 'PW1570MaleChild']"));
               _driver.FindElement(By.XPath("//a[@data-title= 'Pain in the side of the body'][@data-pathway-number= 'PW720MaleChild']"));
               _driver.FindElement(By.XPath("//a[@data-title= 'Something in the ear'][@data-pathway-number= 'PW1528MaleChild']"));
               _driver.FindElement(By.XPath("//a[@data-title= 'Something in the nose'][@data-pathway-number= 'PW1529MaleChild']"));
               _driver.FindElement(By.XPath("//a[@data-title= 'Something in the penis'][@data-pathway-number= 'PW1530MaleChild']"));
               _driver.FindElement(By.XPath("//a[@data-title= 'Something in the bottom/back passage'][@data-pathway-number= 'PW1531MaleChild']"));
               _driver.FindElement(By.XPath("//a[@data-title= 'Genital injury - skin not broken'][@data-pathway-number= 'PW1118MaleChild']"));
               _driver.FindElement(By.XPath("//a[@data-title= 'Genital injury with broken skin'][@data-pathway-number= 'PW1012MaleChild']"));
               _driver.FindElement(By.XPath("//a[@data-title= 'Genital problems'][@data-pathway-number= 'PW1565MaleChild']"));
               _driver.FindElement(By.XPath("//a[@data-title= 'Swollen groin'][@data-pathway-number= 'PW732MaleChild']"));
               _driver.FindElement(By.XPath("//a[@data-title= 'Painful groin'][@data-pathway-number= 'PW732MaleChild']"));
               _driver.FindElement(By.XPath("//a[@data-title= 'Hair loss'][@data-pathway-number= 'PW1678MaleChild']"));
               _driver.FindElement(By.XPath("//a[@data-title= 'Hand injury - skin not broken'][@data-pathway-number= 'PW1267MaleChild']"));
               _driver.FindElement(By.XPath("//a[@data-title= 'Wrist injury - skin not broken'][@data-pathway-number= 'PW1267MaleChild']"));
               _driver.FindElement(By.XPath("//a[@data-title= 'Hand injury with broken skin'][@data-pathway-number= 'PW1260MaleChild']"));
               _driver.FindElement(By.XPath("//a[@data-title= 'Wrist injury with broken skin'][@data-pathway-number= 'PW1260MaleChild']"));
               _driver.FindElement(By.XPath("//a[@data-title= 'Head injury - skin not broken'][@data-pathway-number= 'PW684MaleChild']"));
               _driver.FindElement(By.XPath("//a[@data-title= 'Face injury - skin not broken'][@data-pathway-number= 'PW684MaleChild']"));
               _driver.FindElement(By.XPath("//a[@data-title= 'Neck injury - skin not broken'][@data-pathway-number= 'PW684MaleChild']"));
               _driver.FindElement(By.XPath("//a[@data-title= 'Head injury with broken skin'][@data-pathway-number= 'PW692MaleChild']"));
               _driver.FindElement(By.XPath("//a[@data-title= 'Face injury with broken skin'][@data-pathway-number= 'PW692MaleChild']"));
               _driver.FindElement(By.XPath("//a[@data-title= 'Neck injury with broken skin'][@data-pathway-number= 'PW692MaleChild']"));
               _driver.FindElement(By.XPath("//a[@data-title= 'Headache'][@data-pathway-number= 'PW756MaleChild']"));
               _driver.FindElement(By.XPath("//a[@data-title= 'Hearing problems'][@data-pathway-number= 'PW1762MaleChild']"));
               _driver.FindElement(By.XPath("//a[@data-title= 'Blocked ear'][@data-pathway-number= 'PW1762MaleChild']"));
               _driver.FindElement(By.XPath("//a[@data-title= 'Heatstroke'][@data-pathway-number= 'PW998MaleChild']"));
               _driver.FindElement(By.XPath("//a[@data-title= 'Heat exhaustion'][@data-pathway-number= 'PW998MaleChild']"));
               _driver.FindElement(By.XPath("//a[@data-title= 'Hiccups'][@data-pathway-number= 'PW1775MaleChild']"));
               _driver.FindElement(By.XPath("//a[@data-title= 'Leg injury - skin not broken'][@data-pathway-number= 'PW1591MaleChild']"));
               _driver.FindElement(By.XPath("//a[@data-title= 'Leg injury with broken skin'][@data-pathway-number= 'PW1234MaleChild']"));
               _driver.FindElement(By.XPath("//a[@data-title= 'Leg changing colour'][@data-pathway-number= 'PW1734MaleChild']"));
               _driver.FindElement(By.XPath("//a[@data-title= 'Cold leg'][@data-pathway-number= 'PW1734MaleChild']"));
               _driver.FindElement(By.XPath("//a[@data-title= 'Locked jaw'][@data-pathway-number= 'PW1712MaleChild']"));
               _driver.FindElement(By.XPath("//a[@data-title= 'Loss of bowel control'][@data-pathway-number= 'PW1759MaleChild']"));
               _driver.FindElement(By.XPath("//a[@data-title= 'Bowel incontinence'][@data-pathway-number= 'PW1759MaleChild']"));
               _driver.FindElement(By.XPath("//a[@data-title= 'Lower back injury - skin not broken'][@data-pathway-number= 'PW1597MaleChild']"));
               _driver.FindElement(By.XPath("//a[@data-title= 'Lower back injury with broken skin'][@data-pathway-number= 'PW801MaleChild']"));
               _driver.FindElement(By.XPath("//a[@data-title= 'Lower back pain'][@data-pathway-number= 'PW786MaleChild']"));
               _driver.FindElement(By.XPath("//a[@data-title= 'Painful legs'][@data-pathway-number= 'PW778MaleChild']"));
               _driver.FindElement(By.XPath("//a[@data-title= 'Swollen legs'][@data-pathway-number= 'PW778MaleChild']"));
               _driver.FindElement(By.XPath("//a[@data-title= 'Mental health problems'][@data-pathway-number= 'PW1751MaleChild']"));
               _driver.FindElement(By.XPath("//a[@data-title= 'Low mood'][@data-pathway-number= 'PW1751MaleChild']"));
               _driver.FindElement(By.XPath("//a[@data-title= 'Depression'][@data-pathway-number= 'PW1751MaleChild']"));
               _driver.FindElement(By.XPath("//a[@data-title= 'Anxiety'][@data-pathway-number= 'PW1751MaleChild']"));
               _driver.FindElement(By.XPath("//a[@data-title= 'Mouth ulcers'][@data-pathway-number= 'PW1323MaleChild']"));
               _driver.FindElement(By.XPath("//a[@data-title= 'Blocked nose'][@data-pathway-number= 'PW984MaleChild']"));
               _driver.FindElement(By.XPath("//a[@data-title= 'Nosebleed after an injury'][@data-pathway-number= 'PW1713MaleChild']"));
               _driver.FindElement(By.XPath("//a[@data-title= 'Nosebleed'][@data-pathway-number= 'PW819MaleChild']"));
               _driver.FindElement(By.XPath("//a[@data-title= 'Numbness'][@data-pathway-number= 'PW1683MaleChild']"));
               _driver.FindElement(By.XPath("//a[@data-title= 'Tingling'][@data-pathway-number= 'PW1683MaleChild']"));
               _driver.FindElement(By.XPath("//a[@data-title= 'Swallowed an object'][@data-pathway-number= 'PW1034MaleChild']"));
               _driver.FindElement(By.XPath("//a[@data-title= 'Breathed in an object'][@data-pathway-number= 'PW1034MaleChild']"));
               _driver.FindElement(By.XPath("//a[@data-title= 'Other symptoms'][@data-pathway-number= 'PW1349MaleChild']"));
               _driver.FindElement(By.XPath("//a[@data-title= 'Palpitations'][@data-pathway-number= 'PW1031MaleChild']"));
               _driver.FindElement(By.XPath("//a[@data-title= 'Pounding heart'][@data-pathway-number= 'PW1031MaleChild']"));
               _driver.FindElement(By.XPath("//a[@data-title= 'Fluttering heart'][@data-pathway-number= 'PW1031MaleChild']"));
               _driver.FindElement(By.XPath("//a[@data-title= 'Urinary problems'][@data-pathway-number= 'PW648MaleChild']"));
               _driver.FindElement(By.XPath("//a[@data-title= 'Bleeding from the bottom/back passage'][@data-pathway-number= 'PW846MaleChild']"));
               _driver.FindElement(By.XPath("//a[@data-title= 'Swollen bottom/back passage'][@data-pathway-number= 'PW1091MaleChild']"));
               _driver.FindElement(By.XPath("//a[@data-title= 'Painful bottom/back passage'][@data-pathway-number= 'PW1091MaleChild']"));
               _driver.FindElement(By.XPath("//a[@data-title= 'Itchy bottom/back passage'][@data-pathway-number= 'PW1091MaleChild']"));
               _driver.FindElement(By.XPath("//a[@data-title= 'Grazes'][@data-pathway-number= 'PW1590MaleChild']"));
               _driver.FindElement(By.XPath("//a[@data-title= 'Minor wounds'][@data-pathway-number= 'PW1590MaleChild']"));
               _driver.FindElement(By.XPath("//a[@data-title= 'Scratches'][@data-pathway-number= 'PW1590MaleChild']"));
               _driver.FindElement(By.XPath("//a[@data-title= 'Sexual concerns - male'][@data-pathway-number= 'PW1698MaleChild']"));
               _driver.FindElement(By.XPath("//a[@data-title= 'Shoulder pain'][@data-pathway-number= 'PW1143MaleChild']"));
               _driver.FindElement(By.XPath("//a[@data-title= 'Sinusitis'][@data-pathway-number= 'PW1602MaleChild']"));
               _driver.FindElement(By.XPath("//a[@data-title= 'Rashes, itching, spots, moles and other skin problems'][@data-pathway-number= 'PW1772MaleChild']"));
               _driver.FindElement(By.XPath("//a[@data-title= 'Glue on the skin'][@data-pathway-number= 'PW1301MaleChild']"));
               _driver.FindElement(By.XPath("//a[@data-title= 'Something under the skin'][@data-pathway-number= 'PW1754MaleChild']"));
               _driver.FindElement(By.XPath("//a[@data-title= 'Splinter'][@data-pathway-number= 'PW1754MaleChild']"));
               _driver.FindElement(By.XPath("//a[@data-title= 'Sleep problems'][@data-pathway-number= 'PW1697MaleChild']"));
               _driver.FindElement(By.XPath("//a[@data-title= 'Sore throat'][@data-pathway-number= 'PW854MaleChild']"));
               _driver.FindElement(By.XPath("//a[@data-title= 'Hoarse voice'][@data-pathway-number= 'PW854MaleChild']"));
               _driver.FindElement(By.XPath("//a[@data-title= 'Stoma problems'][@data-pathway-number= 'PW1719MaleChild']"));
               _driver.FindElement(By.XPath("//a[@data-title= 'Stroke symptoms'][@data-pathway-number= 'PA171MaleChild']"));
               _driver.FindElement(By.XPath("//a[@data-title= 'Tiredness'][@data-pathway-number= 'PW1073MaleChild']"));
               _driver.FindElement(By.XPath("//a[@data-title= 'Fatigue'][@data-pathway-number= 'PW1073MaleChild']"));
               _driver.FindElement(By.XPath("//a[@data-title= 'Toe injury - skin not broken'][@data-pathway-number= 'PW1282MaleChild']"));
               _driver.FindElement(By.XPath("//a[@data-title= 'Toe injury with broken skin'][@data-pathway-number= 'PW1526MaleChild']"));
               _driver.FindElement(By.XPath("//a[@data-title= 'Toenail injury'][@data-pathway-number= 'PW1593MaleChild']"));
               _driver.FindElement(By.XPath("//a[@data-title= 'Toothache after an injury'][@data-pathway-number= 'PW572MaleChild']"));
               _driver.FindElement(By.XPath("//a[@data-title= 'Blisters'][@data-pathway-number= 'PW1625MaleChild']"));
               _driver.FindElement(By.XPath("//a[@data-title= 'Trembling'][@data-pathway-number= 'PW1764MaleChild']"));
               _driver.FindElement(By.XPath("//a[@data-title= 'Shaking'][@data-pathway-number= 'PW1764MaleChild']"));
               _driver.FindElement(By.XPath("//a[@data-title= 'Vomiting and nausea - no diarrhoea'][@data-pathway-number= 'PW940MaleChild']"));
               _driver.FindElement(By.XPath("//a[@data-title= 'Wound problems'][@data-pathway-number= 'PW1776MaleChild']"));
               _driver.FindElement(By.XPath("//a[@data-title= 'Wound problems - plaster casts'][@data-pathway-number= 'PW1709MaleChild']"));
               _driver.FindElement(By.XPath("//a[@data-title= 'Wound problems - drainage tubes'][@data-pathway-number= 'PW1709MaleChild']"));
               _driver.FindElement(By.XPath("//a[@data-title= 'Wound problems - metal attachments'][@data-pathway-number= 'PW1709MaleChild']"));
               _driver.FindElement(By.XPath("//a[@data-title= 'Swollen finger'][@data-pathway-number= 'PW1617MaleChild']"));
               _driver.FindElement(By.XPath("//a[@data-title= 'Painful finger'][@data-pathway-number= 'PW1617MaleChild']"));
               _driver.FindElement(By.XPath("//a[@data-title= 'Swollen hand'][@data-pathway-number= 'PW1617MaleChild']"));
               _driver.FindElement(By.XPath("//a[@data-title= 'Painful hand'][@data-pathway-number= 'PW1617MaleChild']"));
               _driver.FindElement(By.XPath("//a[@data-title= 'Swollen wrist'][@data-pathway-number= 'PW1617MaleChild']"));
               _driver.FindElement(By.XPath("//a[@data-title= 'Painful wrist'][@data-pathway-number= 'PW1617MaleChild']"));
               _driver.FindElement(By.XPath("//a[@data-title= 'Self-harm'][@data-pathway-number= 'PW1543MaleChild']"));

               _driver.FindElement(By.Id("bowel-and-urinary-problems")).Click();
               _driver.FindElement(By.Id("bowel-and-urinary-problems-bowel-problems")).Click();
               var wait = new WebDriverWait(_driver, new TimeSpan(0, 0, 5));
               wait.Until(ExpectedConditions.VisibilityOfAllElementsLocatedBy((By.XPath("//a[@data-title= 'Something in the bottom/back passage']"))));
               _driver.FindElement(By.XPath("//a[@data-title= 'Something in the bottom/back passage']")).Click();

               _driver.FindElement(By.XPath("//input[@value = 'PW1531MaleChild']"));

                          }

           [Test]
           public void CategoriespresentForMaleAdult()

                   //Test to show all categories are present per age and gender, and also the correct pathway opens per age and gender, when the same pathway is used for all ages/genders.
           {

               var searchPage = TestScenerios.LaunchSearchScenerio(_driver, TestScenerioGender.Male, 40);

               _driver.FindElement(By.Id("searchButton")).Click();

               _driver.FindElement(By.XPath("//a[@data-title= 'Abdomen injury - skin not broken'][@data-pathway-number= 'PW503MaleAdult']")); 
               _driver.FindElement(By.XPath("//a[@data-title= 'Injury to the side of the body - skin not broken'][@data-pathway-number= 'PW503MaleAdult']"));
               _driver.FindElement(By.XPath("//a[@data-title= 'Abdomen injury with broken skin'][@data-pathway-number= 'PW511MaleAdult']"));
               _driver.FindElement(By.XPath("//a[@data-title= 'Injury to the side of the body with broken skin'][@data-pathway-number= 'PW511MaleAdult']"));
               _driver.FindElement(By.XPath("//a[@data-title= 'Abdominal pain'][@data-pathway-number= 'PW519MaleAdult']"));
               _driver.FindElement(By.XPath("//a[@data-title= 'Stomach pain'][@data-pathway-number= 'PW519MaleAdult']"));
               _driver.FindElement(By.XPath("//a[@data-title= 'Accidental poisoning'][@data-pathway-number= 'PW881MaleAdult']"));
               _driver.FindElement(By.XPath("//a[@data-title= 'Breathed in something poisonous'][@data-pathway-number= 'PW881MaleAdult']"));
               _driver.FindElement(By.XPath("//a[@data-title= 'Swallowed something poisonous'][@data-pathway-number= 'PW881MaleAdult']"));
               _driver.FindElement(By.XPath("//a[@data-title= 'Drunk too much alcohol'][@data-pathway-number= 'PW1551MaleAdult']"));
               _driver.FindElement(By.XPath("//a[@data-title= 'Alcohol intoxication'][@data-pathway-number= 'PW1551MaleAdult']"));
               _driver.FindElement(By.XPath("//a[@data-title= 'Ankle injury - skin not broken'][@data-pathway-number= 'PW1512MaleAdult']"));
               _driver.FindElement(By.XPath("//a[@data-title= 'Foot injury - skin not broken'][@data-pathway-number= 'PW1512MaleAdult']"));
               _driver.FindElement(By.XPath("//a[@data-title= 'Ankle injury with broken skin'][@data-pathway-number= 'PW1518MaleAdult']"));
               _driver.FindElement(By.XPath("//a[@data-title= 'Foot injury with broken skin'][@data-pathway-number= 'PW1518MaleAdult']"));
               _driver.FindElement(By.XPath("//a[@data-title= 'Arm injury - skin not broken'][@data-pathway-number= 'PW897MaleAdult']"));
               _driver.FindElement(By.XPath("//a[@data-title= 'Arm injury with broken skin'][@data-pathway-number= 'PW902MaleAdult']"));
               _driver.FindElement(By.XPath("//a[@data-title= 'Painful arm'][@data-pathway-number= 'PW1166MaleAdult']"));
               _driver.FindElement(By.XPath("//a[@data-title= 'Swollen arm'][@data-pathway-number= 'PW1166MaleAdult']"));
               _driver.FindElement(By.XPath("//a[@data-title= 'Cold arm'][@data-pathway-number= 'PW1733MaleAdult']"));
               _driver.FindElement(By.XPath("//a[@data-title= 'Arm changing colour'][@data-pathway-number= 'PW1733MaleAdult']"));
               _driver.FindElement(By.XPath("//a[@data-title= 'Change in behaviour'][@data-pathway-number= 'PW1749MaleAdult']"));
               _driver.FindElement(By.XPath("//a[@data-title= 'Bites and stings'][@data-pathway-number= 'PW1575MaleAdult']"));
               _driver.FindElement(By.XPath("//a[@data-title= 'Bleeding from stoma'][@data-pathway-number= 'PT507MaleAdult']"));
               _driver.FindElement(By.XPath("//a[@data-title= 'Blood in urine'][@data-pathway-number= 'PW962MaleAdult']"));
               _driver.FindElement(By.XPath("//a[@data-title= 'Breast problems'][@data-pathway-number= 'PW1604MaleAdult']"));
               _driver.FindElement(By.XPath("//a[@data-title= 'Breathing problems'][@data-pathway-number= 'PW559MaleAdult']"));
               _driver.FindElement(By.XPath("//a[@data-title= 'Upper back pain'][@data-pathway-number= 'PW559MaleAdult']"));
               _driver.FindElement(By.XPath("//a[@data-title= 'Chest and upper back pain'][@data-pathway-number= 'PW559MaleAdult']"));
               _driver.FindElement(By.XPath("//a[@data-title= 'Chest pain'][@data-pathway-number= 'PW559MaleAdult']"));
               _driver.FindElement(By.XPath("//a[@data-title= 'Wheezing'][@data-pathway-number= 'PW559MaleAdult']"));
               _driver.FindElement(By.XPath("//a[@data-title= 'Coughing up blood'][@data-pathway-number= 'PW1653MaleAdult']"));
               _driver.FindElement(By.XPath("//a[@data-title= 'Bringing up blood'][@data-pathway-number= 'PW1653MaleAdult']"));
               _driver.FindElement(By.XPath("//a[@data-title= 'Burns from chemicals'][@data-pathway-number= 'PW564MaleAdult']"));
               _driver.FindElement(By.XPath("//a[@data-title= 'Sunburn'][@data-pathway-number= 'PW987MaleAdult']"));
               _driver.FindElement(By.XPath("//a[@data-title= 'Burns from heat'][@data-pathway-number= 'PW580MaleAdult']"));
               _driver.FindElement(By.XPath("//a[@data-title= 'Catheter problems'][@data-pathway-number= 'PW1567MaleAdult']"));
               _driver.FindElement(By.XPath("//a[@data-title= 'Chest injury - skin not broken'][@data-pathway-number= 'PW588MaleAdult']"));
               _driver.FindElement(By.XPath("//a[@data-title= 'Upper back injury - skin not broken'][@data-pathway-number= 'PW588MaleAdult']"));
               _driver.FindElement(By.XPath("//a[@data-title= 'Chest injury with broken skin'][@data-pathway-number= 'PW596MaleAdult']"));
               _driver.FindElement(By.XPath("//a[@data-title= 'Upper back injury with broken skin'][@data-pathway-number= 'PW596MaleAdult']"));
               _driver.FindElement(By.XPath("//a[@data-title= 'Colds and flu'][@data-pathway-number= 'PW1042MaleAdult']"));
               _driver.FindElement(By.XPath("//a[@data-title= 'Constipation'][@data-pathway-number= 'PW1159MaleAdult']"));
               _driver.FindElement(By.XPath("//a[@data-title= 'Cough'][@data-pathway-number= 'PW976MaleAdult']"));
               _driver.FindElement(By.XPath("//a[@data-title= 'Self-harm'][@data-pathway-number= 'PW1543MaleAdult']"));
               _driver.FindElement(By.XPath("//a[@data-title= 'Dental injury'][@data-pathway-number= 'PW620MaleAdult']"));
               _driver.FindElement(By.XPath("//a[@data-title= 'Toothache and other dental problems'][@data-pathway-number= 'PW1610MaleAdult']"));
               _driver.FindElement(By.XPath("//a[@data-title= 'Diabetes - blood sugar problems'][@data-pathway-number= 'PW1746MaleAdult']"));
               _driver.FindElement(By.XPath("//a[@data-title= 'Diarrhoea - no vomiting'][@data-pathway-number= 'PW631MaleAdult']"));
               _driver.FindElement(By.XPath("//a[@data-title= 'Diarrhoea and vomiting'][@data-pathway-number= 'PW1555MaleAdult']"));
               _driver.FindElement(By.XPath("//a[@data-title= 'Difficulty passing urine'][@data-pathway-number= 'PW886MaleAdult']"));
               _driver.FindElement(By.XPath("//a[@data-title= 'Difficulty swallowing'][@data-pathway-number= 'PW1496MaleAdult']"));
               _driver.FindElement(By.XPath("//a[@data-title= 'Dizziness'][@data-pathway-number= 'PW639MaleAdult']"));
               _driver.FindElement(By.XPath("//a[@data-title= 'Vertigo'][@data-pathway-number= 'PW639MaleAdult']"));
               _driver.FindElement(By.XPath("//a[@data-title= 'Ear discharge'][@data-pathway-number= 'PW1702MaleAdult']"));
               _driver.FindElement(By.XPath("//a[@data-title= 'Earwax'][@data-pathway-number= 'PW1702MaleAdult']"));
               _driver.FindElement(By.XPath("//a[@data-title= 'Earache'][@data-pathway-number= 'PW655MaleAdult']"));
               _driver.FindElement(By.XPath("//a[@data-title= 'Eye injury - no damage to surface of eye'][@data-pathway-number= 'PW660MaleAdult']"));
               _driver.FindElement(By.XPath("//a[@data-title= 'Eye injury with damage to surface of eye'][@data-pathway-number= 'PW668MaleAdult']"));
               _driver.FindElement(By.XPath("//a[@data-title= 'Eye problems'][@data-pathway-number= 'PW1629MaleAdult']"));
               _driver.FindElement(By.XPath("//a[@data-title= 'Eyelid problems'][@data-pathway-number= 'PW1629MaleAdult']"));
               _driver.FindElement(By.XPath("//a[@data-title= 'Something in the eye'][@data-pathway-number= 'PW1098MaleAdult']"));
               _driver.FindElement(By.XPath("//a[@data-title= 'Swollen face'][@data-pathway-number= 'PW1545MaleAdult']"));
               _driver.FindElement(By.XPath("//a[@data-title= 'Painful face'][@data-pathway-number= 'PW1545MaleAdult']"));
               _driver.FindElement(By.XPath("//a[@data-title= 'Swollen neck'][@data-pathway-number= 'PW1545MaleAdult']"));
               _driver.FindElement(By.XPath("//a[@data-title= 'Painful neck'][@data-pathway-number= 'PW1545MaleAdult']"));
               _driver.FindElement(By.XPath("//a[@data-title= 'Falling'][@data-pathway-number= 'PW700MaleAdult']"));
               _driver.FindElement(By.XPath("//a[@data-title= 'Fainting'][@data-pathway-number= 'PW700MaleAdult']"));
               _driver.FindElement(By.XPath("//a[@data-title= 'Passing out'][@data-pathway-number= 'PW700MaleAdult']"));
               _driver.FindElement(By.XPath("//a[@data-title= 'Fever'][@data-pathway-number= 'PW711MaleAdult']"));
               _driver.FindElement(By.XPath("//a[@data-title= 'High temperature'][@data-pathway-number= 'PW711MaleAdult']"));
               _driver.FindElement(By.XPath("//a[@data-title= 'Finger injury - skin not broken'][@data-pathway-number= 'PW1270MaleAdult']"));
               _driver.FindElement(By.XPath("//a[@data-title= 'Thumb injury - skin not broken'][@data-pathway-number= 'PW1270MaleAdult']"));
               _driver.FindElement(By.XPath("//a[@data-title= 'Finger injury with broken skin'][@data-pathway-number= 'PW1264MaleAdult']"));
               _driver.FindElement(By.XPath("//a[@data-title= 'Thumb injury with broken skin'][@data-pathway-number= 'PW1264MaleAdult']"));
               _driver.FindElement(By.XPath("//a[@data-title= 'Fingernail injury'][@data-pathway-number= 'PW1570MaleAdult']"));
               _driver.FindElement(By.XPath("//a[@data-title= 'Pain in the side of the body'][@data-pathway-number= 'PW719MaleAdult']"));
               _driver.FindElement(By.XPath("//a[@data-title= 'Something in the ear'][@data-pathway-number= 'PW1528MaleAdult']"));
               _driver.FindElement(By.XPath("//a[@data-title= 'Something in the nose'][@data-pathway-number= 'PW1529MaleAdult']"));
               _driver.FindElement(By.XPath("//a[@data-title= 'Something in the penis'][@data-pathway-number= 'PW1530MaleAdult']"));
               _driver.FindElement(By.XPath("//a[@data-title= 'Something in the bottom/back passage'][@data-pathway-number= 'PW1531MaleAdult']"));
               _driver.FindElement(By.XPath("//a[@data-title= 'Genital injury - skin not broken'][@data-pathway-number= 'PW1118MaleAdult']"));
               _driver.FindElement(By.XPath("//a[@data-title= 'Genital injury with broken skin'][@data-pathway-number= 'PW1012MaleAdult']"));
               _driver.FindElement(By.XPath("//a[@data-title= 'Genital problems'][@data-pathway-number= 'PW1564MaleAdult']"));
               _driver.FindElement(By.XPath("//a[@data-title= 'Swollen groin'][@data-pathway-number= 'PW731MaleAdult']"));
               _driver.FindElement(By.XPath("//a[@data-title= 'Painful groin'][@data-pathway-number= 'PW731MaleAdult']"));
               _driver.FindElement(By.XPath("//a[@data-title= 'Hair loss'][@data-pathway-number= 'PW1678MaleAdult']"));
               _driver.FindElement(By.XPath("//a[@data-title= 'Hand injury - skin not broken'][@data-pathway-number= 'PW1267MaleAdult']"));
               _driver.FindElement(By.XPath("//a[@data-title= 'Wrist injury - skin not broken'][@data-pathway-number= 'PW1267MaleAdult']"));
               _driver.FindElement(By.XPath("//a[@data-title= 'Hand injury with broken skin'][@data-pathway-number= 'PW1260MaleAdult']"));
               _driver.FindElement(By.XPath("//a[@data-title= 'Wrist injury with broken skin'][@data-pathway-number= 'PW1260MaleAdult']"));
               _driver.FindElement(By.XPath("//a[@data-title= 'Head injury - skin not broken'][@data-pathway-number= 'PW684MaleAdult']"));
               _driver.FindElement(By.XPath("//a[@data-title= 'Face injury - skin not broken'][@data-pathway-number= 'PW684MaleAdult']"));
               _driver.FindElement(By.XPath("//a[@data-title= 'Neck injury - skin not broken'][@data-pathway-number= 'PW684MaleAdult']"));
               _driver.FindElement(By.XPath("//a[@data-title= 'Head injury with broken skin'][@data-pathway-number= 'PW692MaleAdult']"));
               _driver.FindElement(By.XPath("//a[@data-title= 'Face injury with broken skin'][@data-pathway-number= 'PW692MaleAdult']"));
               _driver.FindElement(By.XPath("//a[@data-title= 'Neck injury with broken skin'][@data-pathway-number= 'PW692MaleAdult']"));
               _driver.FindElement(By.XPath("//a[@data-title= 'Headache'][@data-pathway-number= 'PW755MaleAdult']"));
               _driver.FindElement(By.XPath("//a[@data-title= 'Hearing problems'][@data-pathway-number= 'PW1762MaleAdult']"));
               _driver.FindElement(By.XPath("//a[@data-title= 'Blocked ear'][@data-pathway-number= 'PW1762MaleAdult']"));
               _driver.FindElement(By.XPath("//a[@data-title= 'Heatstroke'][@data-pathway-number= 'PW998MaleAdult']"));
               _driver.FindElement(By.XPath("//a[@data-title= 'Heat exhaustion'][@data-pathway-number= 'PW998MaleAdult']"));
               _driver.FindElement(By.XPath("//a[@data-title= 'Hiccups'][@data-pathway-number= 'PW1775MaleAdult']"));
               _driver.FindElement(By.XPath("//a[@data-title= 'Leg injury - skin not broken'][@data-pathway-number= 'PW1241MaleAdult']"));
               _driver.FindElement(By.XPath("//a[@data-title= 'Leg injury with broken skin'][@data-pathway-number= 'PW1234MaleAdult']"));
               _driver.FindElement(By.XPath("//a[@data-title= 'Leg changing colour'][@data-pathway-number= 'PW1734MaleAdult']"));
               _driver.FindElement(By.XPath("//a[@data-title= 'Cold leg'][@data-pathway-number= 'PW1734MaleAdult']"));
               _driver.FindElement(By.XPath("//a[@data-title= 'Locked jaw'][@data-pathway-number= 'PW1712MaleAdult']"));
               _driver.FindElement(By.XPath("//a[@data-title= 'Loss of bowel control'][@data-pathway-number= 'PW1759MaleAdult']"));
               _driver.FindElement(By.XPath("//a[@data-title= 'Bowel incontinence'][@data-pathway-number= 'PW1759MaleAdult']"));
               _driver.FindElement(By.XPath("//a[@data-title= 'Lower back injury - skin not broken'][@data-pathway-number= 'PW793MaleAdult']"));
               _driver.FindElement(By.XPath("//a[@data-title= 'Lower back injury with broken skin'][@data-pathway-number= 'PW801MaleAdult']"));
               _driver.FindElement(By.XPath("//a[@data-title= 'Lower back pain'][@data-pathway-number= 'PW785MaleAdult']"));
               _driver.FindElement(By.XPath("//a[@data-title= 'Painful legs'][@data-pathway-number= 'PW777MaleAdult']"));
               _driver.FindElement(By.XPath("//a[@data-title= 'Swollen legs'][@data-pathway-number= 'PW777MaleAdult']"));
               _driver.FindElement(By.XPath("//a[@data-title= 'Mental health problems'][@data-pathway-number= 'PW1751MaleAdult']"));
               _driver.FindElement(By.XPath("//a[@data-title= 'Low mood'][@data-pathway-number= 'PW1751MaleAdult']"));
               _driver.FindElement(By.XPath("//a[@data-title= 'Depression'][@data-pathway-number= 'PW1751MaleAdult']"));
               _driver.FindElement(By.XPath("//a[@data-title= 'Anxiety'][@data-pathway-number= 'PW1751MaleAdult']"));
               _driver.FindElement(By.XPath("//a[@data-title= 'Mouth ulcers'][@data-pathway-number= 'PW1323MaleAdult']"));
               _driver.FindElement(By.XPath("//a[@data-title= 'Blocked nose'][@data-pathway-number= 'PW981MaleAdult']"));
               _driver.FindElement(By.XPath("//a[@data-title= 'Nosebleed after an injury'][@data-pathway-number= 'PW1713MaleAdult']"));
               _driver.FindElement(By.XPath("//a[@data-title= 'Nosebleed'][@data-pathway-number= 'PW818MaleAdult']"));
               _driver.FindElement(By.XPath("//a[@data-title= 'Numbness'][@data-pathway-number= 'PW1683MaleAdult']"));
               _driver.FindElement(By.XPath("//a[@data-title= 'Tingling'][@data-pathway-number= 'PW1683MaleAdult']"));
               _driver.FindElement(By.XPath("//a[@data-title= 'Swallowed an object'][@data-pathway-number= 'PW1034MaleAdult']"));
               _driver.FindElement(By.XPath("//a[@data-title= 'Breathed in an object'][@data-pathway-number= 'PW1034MaleAdult']"));
               _driver.FindElement(By.XPath("//a[@data-title= 'Other symptoms'][@data-pathway-number= 'PW1346MaleAdult']"));
               _driver.FindElement(By.XPath("//a[@data-title= 'Palpitations'][@data-pathway-number= 'PW1030MaleAdult']"));
               _driver.FindElement(By.XPath("//a[@data-title= 'Pounding heart'][@data-pathway-number= 'PW1030MaleAdult']"));
               _driver.FindElement(By.XPath("//a[@data-title= 'Fluttering heart'][@data-pathway-number= 'PW1030MaleAdult']"));
               _driver.FindElement(By.XPath("//a[@data-title= 'Urinary problems'][@data-pathway-number= 'PW647MaleAdult']"));
               _driver.FindElement(By.XPath("//a[@data-title= 'Bleeding from the bottom/back passage'][@data-pathway-number= 'PW846MaleAdult']"));
               _driver.FindElement(By.XPath("//a[@data-title= 'Swollen bottom/back passage'][@data-pathway-number= 'PW1091MaleAdult']"));
               _driver.FindElement(By.XPath("//a[@data-title= 'Painful bottom/back passage'][@data-pathway-number= 'PW1091MaleAdult']"));
               _driver.FindElement(By.XPath("//a[@data-title= 'Itchy bottom/back passage'][@data-pathway-number= 'PW1091MaleAdult']"));
               _driver.FindElement(By.XPath("//a[@data-title= 'Grazes'][@data-pathway-number= 'PW1590MaleAdult']"));
               _driver.FindElement(By.XPath("//a[@data-title= 'Minor wounds'][@data-pathway-number= 'PW1590MaleAdult']"));
               _driver.FindElement(By.XPath("//a[@data-title= 'Scratches'][@data-pathway-number= 'PW1590MaleAdult']"));
               _driver.FindElement(By.XPath("//a[@data-title= 'Sexual concerns - male'][@data-pathway-number= 'PW1685MaleAdult']"));
               _driver.FindElement(By.XPath("//a[@data-title= 'Shoulder pain'][@data-pathway-number= 'PW1140MaleAdult']"));
               _driver.FindElement(By.XPath("//a[@data-title= 'Sinusitis'][@data-pathway-number= 'PW1048MaleAdult']"));
               _driver.FindElement(By.XPath("//a[@data-title= 'Rashes, itching, spots, moles and other skin problems'][@data-pathway-number= 'PW1771MaleAdult']"));
               _driver.FindElement(By.XPath("//a[@data-title= 'Glue on the skin'][@data-pathway-number= 'PW1301MaleAdult']"));
               _driver.FindElement(By.XPath("//a[@data-title= 'Something under the skin'][@data-pathway-number= 'PW1754MaleAdult']"));
               _driver.FindElement(By.XPath("//a[@data-title= 'Splinter'][@data-pathway-number= 'PW1754MaleAdult']"));
               _driver.FindElement(By.XPath("//a[@data-title= 'Sleep problems'][@data-pathway-number= 'PW1686MaleAdult']"));
               _driver.FindElement(By.XPath("//a[@data-title= 'Sore throat'][@data-pathway-number= 'PW854MaleAdult']"));
               _driver.FindElement(By.XPath("//a[@data-title= 'Hoarse voice'][@data-pathway-number= 'PW854MaleAdult']"));
               _driver.FindElement(By.XPath("//a[@data-title= 'Stoma problems'][@data-pathway-number= 'PW1719MaleAdult']"));
               _driver.FindElement(By.XPath("//a[@data-title= 'Tiredness'][@data-pathway-number= 'PW1072MaleAdult']"));
               _driver.FindElement(By.XPath("//a[@data-title= 'Fatigue'][@data-pathway-number= 'PW1072MaleAdult']"));
               _driver.FindElement(By.XPath("//a[@data-title= 'Toe injury - skin not broken'][@data-pathway-number= 'PW1282MaleAdult']"));
               _driver.FindElement(By.XPath("//a[@data-title= 'Toe injury with broken skin'][@data-pathway-number= 'PW1526MaleAdult']"));
               _driver.FindElement(By.XPath("//a[@data-title= 'Toenail injury'][@data-pathway-number= 'PW1593MaleAdult']"));
               _driver.FindElement(By.XPath("//a[@data-title= 'Toothache after an injury'][@data-pathway-number= 'PW572MaleAdult']"));
               _driver.FindElement(By.XPath("//a[@data-title= 'Blisters'][@data-pathway-number= 'PW1625MaleAdult']"));
               _driver.FindElement(By.XPath("//a[@data-title= 'Trembling'][@data-pathway-number= 'PW1764MaleAdult']"));
               _driver.FindElement(By.XPath("//a[@data-title= 'Shaking'][@data-pathway-number= 'PW1764MaleAdult']"));
               _driver.FindElement(By.XPath("//a[@data-title= 'Vomiting and nausea - no diarrhoea'][@data-pathway-number= 'PW939MaleAdult']"));
               _driver.FindElement(By.XPath("//a[@data-title= 'Wound problems'][@data-pathway-number= 'PW1776MaleAdult']"));
               _driver.FindElement(By.XPath("//a[@data-title= 'Wound problems - plaster casts'][@data-pathway-number= 'PW1709MaleAdult']"));
               _driver.FindElement(By.XPath("//a[@data-title= 'Wound problems - drainage tubes'][@data-pathway-number= 'PW1709MaleAdult']"));
               _driver.FindElement(By.XPath("//a[@data-title= 'Wound problems - metal attachments'][@data-pathway-number= 'PW1709MaleAdult']"));
               _driver.FindElement(By.XPath("//a[@data-title= 'Swollen finger'][@data-pathway-number= 'PW1615MaleAdult']"));
               _driver.FindElement(By.XPath("//a[@data-title= 'Painful finger'][@data-pathway-number= 'PW1615MaleAdult']"));
               _driver.FindElement(By.XPath("//a[@data-title= 'Swollen hand'][@data-pathway-number= 'PW1615MaleAdult']"));
               _driver.FindElement(By.XPath("//a[@data-title= 'Painful hand'][@data-pathway-number= 'PW1615MaleAdult']"));
               _driver.FindElement(By.XPath("//a[@data-title= 'Swollen wrist'][@data-pathway-number= 'PW1615MaleAdult']"));
               _driver.FindElement(By.XPath("//a[@data-title= 'Painful wrist'][@data-pathway-number= 'PW1615MaleAdult']"));
               _driver.FindElement(By.XPath("//a[@data-title= 'Self-harm'][@data-pathway-number= 'PW1543MaleAdult']"));

               _driver.FindElement(By.Id("bowel-and-urinary-problems")).Click();
               _driver.FindElement(By.Id("bowel-and-urinary-problems-bowel-problems")).Click();
               var wait = new WebDriverWait(_driver, new TimeSpan(0, 0, 5));
              wait.Until(ExpectedConditions.VisibilityOfAllElementsLocatedBy((By.XPath("//a[@data-title= 'Something in the bottom/back passage']"))));
              _driver.FindElement(By.XPath("//a[@data-title= 'Something in the bottom/back passage']")).Click();
                       
               _driver.FindElement(By.XPath("//input[@value = 'PW1531MaleAdult']"));

           }

    }
}

