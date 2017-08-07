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

               searchPage.ClickGoButton();

               searchPage.FindPathwayInCathgryList("Abdomen injury - skin not broken","PW500FemaleChild");
               searchPage.FindPathwayInCathgryList("Injury to the side of the body - skin not broken","PW500FemaleChild");
               searchPage.FindPathwayInCathgryList("Abdomen injury with broken skin","PW508FemaleChild");
               searchPage.FindPathwayInCathgryList("Injury to the side of the body with broken skin","PW508FemaleChild");
               searchPage.FindPathwayInCathgryList("Abdominal pain","PW517FemaleChild");
               searchPage.FindPathwayInCathgryList("Stomach pain","PW517FemaleChild");
               searchPage.FindPathwayInCathgryList("Absent periods","PW1676FemaleChild");
               searchPage.FindPathwayInCathgryList("Missed periods","PW1676FemaleChild");
               searchPage.FindPathwayInCathgryList("Accidental poisoning","PW881FemaleChild");
               searchPage.FindPathwayInCathgryList("Breathed in something poisonous","PW881FemaleChild");
               searchPage.FindPathwayInCathgryList("Swallowed something poisonous","PW881FemaleChild");
               searchPage.FindPathwayInCathgryList("Drunk too much alcohol","PW1552FemaleChild");
               searchPage.FindPathwayInCathgryList("Alcohol intoxication","PW1552FemaleChild");
               searchPage.FindPathwayInCathgryList("Ankle injury - skin not broken","PW1512FemaleChild");
               searchPage.FindPathwayInCathgryList("Foot injury - skin not broken","PW1512FemaleChild");
               searchPage.FindPathwayInCathgryList("Ankle injury with broken skin","PW1518FemaleChild");
               searchPage.FindPathwayInCathgryList("Foot injury with broken skin","PW1518FemaleChild");
               searchPage.FindPathwayInCathgryList("Arm injury - skin not broken","PW895FemaleChild");
               searchPage.FindPathwayInCathgryList("Arm injury with broken skin","PW902FemaleChild");
               searchPage.FindPathwayInCathgryList("Painful arm","PW1165FemaleChild");
               searchPage.FindPathwayInCathgryList("Swollen arm","PW1165FemaleChild");
               searchPage.FindPathwayInCathgryList("Cold arm","PW1733FemaleChild");
               searchPage.FindPathwayInCathgryList("Arm changing colour","PW1733FemaleChild");
               searchPage.FindPathwayInCathgryList("Change in behaviour","PW1749FemaleChild");
               searchPage.FindPathwayInCathgryList("Bites and stings","PW1575FemaleChild");
               searchPage.FindPathwayInCathgryList("Bleeding from stoma","PT507FemaleChild");
               searchPage.FindPathwayInCathgryList("Blood in urine","PW961FemaleChild");
               searchPage.FindPathwayInCathgryList("Breast problems","PW1605FemaleChild");
               searchPage.FindPathwayInCathgryList("Breastfeeding problems","PW1114FemaleChild");
               searchPage.FindPathwayInCathgryList("Breathing problems","PW557FemaleChild");
               searchPage.FindPathwayInCathgryList("Upper back pain","PW557FemaleChild");
               searchPage.FindPathwayInCathgryList("Chest and upper back pain","PW557FemaleChild");
               searchPage.FindPathwayInCathgryList("Chest pain","PW557FemaleChild");
               searchPage.FindPathwayInCathgryList("Wheezing","PW557FemaleChild");
               searchPage.FindPathwayInCathgryList("Coughing up blood","PW1652FemaleChild");
               searchPage.FindPathwayInCathgryList("Bringing up blood","PW1652FemaleChild");
               searchPage.FindPathwayInCathgryList("Burns from chemicals","PW564FemaleChild");
               searchPage.FindPathwayInCathgryList("Sunburn","PW987FemaleChild");
               searchPage.FindPathwayInCathgryList("Burns from heat","PW580FemaleChild");
               searchPage.FindPathwayInCathgryList("Catheter problems","PW1567FemaleChild");
               searchPage.FindPathwayInCathgryList("Chest injury - skin not broken","PW588FemaleChild");
               searchPage.FindPathwayInCathgryList("Upper back injury - skin not broken","PW588FemaleChild");
               searchPage.FindPathwayInCathgryList("Chest injury with broken skin","PW596FemaleChild");
               searchPage.FindPathwayInCathgryList("Upper back injury with broken skin","PW596FemaleChild");
               searchPage.FindPathwayInCathgryList("Colds and flu","PW1041FemaleChild");
               searchPage.FindPathwayInCathgryList("Constipation","PW1161FemaleChild");
               searchPage.FindPathwayInCathgryList("Cough","PW978FemaleChild");
               searchPage.FindPathwayInCathgryList("Self-harm","PW1543FemaleChild");
               searchPage.FindPathwayInCathgryList("Dental injury","PW620FemaleChild");
               searchPage.FindPathwayInCathgryList("Toothache and other dental problems","PW1611FemaleChild");
               searchPage.FindPathwayInCathgryList("Diabetes - blood sugar problems","PW1746FemaleChild");
               searchPage.FindPathwayInCathgryList("Diarrhoea - no vomiting","PW629FemaleChild");
               searchPage.FindPathwayInCathgryList("Diarrhoea and vomiting","PW1554FemaleChild");
               searchPage.FindPathwayInCathgryList("Difficulty passing urine","PW886FemaleChild");
               searchPage.FindPathwayInCathgryList("Difficulty swallowing","PW1496FemaleChild");
               searchPage.FindPathwayInCathgryList("Dizziness","PW637FemaleChild");
               searchPage.FindPathwayInCathgryList("Vertigo","PW637FemaleChild");
               searchPage.FindPathwayInCathgryList("Ear discharge","PW1702FemaleChild");
               searchPage.FindPathwayInCathgryList("Earwax","PW1702FemaleChild");
               searchPage.FindPathwayInCathgryList("Earache","PW656FemaleChild");
               searchPage.FindPathwayInCathgryList("Eye injury - no damage to surface of eye","PW660FemaleChild");
               searchPage.FindPathwayInCathgryList("Eye injury with damage to surface of eye","PW668FemaleChild");
               searchPage.FindPathwayInCathgryList("Eye problems","PW1628FemaleChild");
               searchPage.FindPathwayInCathgryList("Eyelid problems","PW1628FemaleChild");
               searchPage.FindPathwayInCathgryList("Something in the eye","PW1098FemaleChild");
               searchPage.FindPathwayInCathgryList("Swollen face","PW1548FemaleChild");
               searchPage.FindPathwayInCathgryList("Painful face","PW1548FemaleChild");
               searchPage.FindPathwayInCathgryList("Swollen neck","PW1548FemaleChild");
               searchPage.FindPathwayInCathgryList("Painful neck","PW1548FemaleChild");
               searchPage.FindPathwayInCathgryList("Falling","PW700FemaleChild");
               searchPage.FindPathwayInCathgryList("Fainting","PW700FemaleChild");
               searchPage.FindPathwayInCathgryList("Passing out","PW700FemaleChild");
               searchPage.FindPathwayInCathgryList("Fever","PW709FemaleChild");
               searchPage.FindPathwayInCathgryList("High temperature","PW709FemaleChild");
               searchPage.FindPathwayInCathgryList("Finger injury - skin not broken","PW1270FemaleChild");
               searchPage.FindPathwayInCathgryList("Thumb injury - skin not broken","PW1270FemaleChild");
               searchPage.FindPathwayInCathgryList("Finger injury with broken skin","PW1264FemaleChild");
               searchPage.FindPathwayInCathgryList("Thumb injury with broken skin","PW1264FemaleChild");
               searchPage.FindPathwayInCathgryList("Fingernail injury","PW1570FemaleChild");
               searchPage.FindPathwayInCathgryList("Pain in the side of the body","PW717FemaleChild");
               searchPage.FindPathwayInCathgryList("Something in the ear","PW1528FemaleChild");
               searchPage.FindPathwayInCathgryList("Something in the nose","PW1529FemaleChild");
               searchPage.FindPathwayInCathgryList("Something in the bottom/back passage","PW1531FemaleChild");
               searchPage.FindPathwayInCathgryList("Something in the vagina","PW1532FemaleChild");
               searchPage.FindPathwayInCathgryList("Genital injury - skin not broken","PW1116FemaleChild");
               searchPage.FindPathwayInCathgryList("Genital injury with broken skin","PW1010FemaleChild");
               searchPage.FindPathwayInCathgryList("Swollen groin","PW729FemaleChild");
               searchPage.FindPathwayInCathgryList("Painful groin","PW729FemaleChild");
               searchPage.FindPathwayInCathgryList("Hair loss","PW1678FemaleChild");
               searchPage.FindPathwayInCathgryList("Hand injury - skin not broken","PW1267FemaleChild");
               searchPage.FindPathwayInCathgryList("Wrist injury - skin not broken","PW1267FemaleChild");
               searchPage.FindPathwayInCathgryList("Hand injury with broken skin","PW1260FemaleChild");
               searchPage.FindPathwayInCathgryList("Wrist injury with broken skin","PW1260FemaleChild");
               searchPage.FindPathwayInCathgryList("Head injury - skin not broken","PW684FemaleChild");
               searchPage.FindPathwayInCathgryList("Face injury - skin not broken","PW684FemaleChild");
               searchPage.FindPathwayInCathgryList("Neck injury - skin not broken","PW684FemaleChild");
               searchPage.FindPathwayInCathgryList("Head injury with broken skin","PW692FemaleChild");
               searchPage.FindPathwayInCathgryList("Face injury with broken skin","PW692FemaleChild");
               searchPage.FindPathwayInCathgryList("Neck injury with broken skin","PW692FemaleChild");
               searchPage.FindPathwayInCathgryList("Headache","PW753FemaleChild");
               searchPage.FindPathwayInCathgryList("Hearing problems","PW1762FemaleChild");
               searchPage.FindPathwayInCathgryList("Blocked ear","PW1762FemaleChild");
               searchPage.FindPathwayInCathgryList("Heatstroke","PW998FemaleChild");
               searchPage.FindPathwayInCathgryList("Heat exhaustion","PW998FemaleChild");
               searchPage.FindPathwayInCathgryList("Hiccups","PW1775FemaleChild");
               searchPage.FindPathwayInCathgryList("Leg injury - skin not broken","PW1591FemaleChild");
               searchPage.FindPathwayInCathgryList("Leg injury with broken skin","PW1234FemaleChild");
               searchPage.FindPathwayInCathgryList("Leg changing colour","PW1734FemaleChild");
               searchPage.FindPathwayInCathgryList("Cold leg","PW1734FemaleChild");
               searchPage.FindPathwayInCathgryList("Locked jaw","PW1712FemaleChild");
               searchPage.FindPathwayInCathgryList("Loss of bowel control","PW1759FemaleChild");
               searchPage.FindPathwayInCathgryList("Bowel incontinence","PW1759FemaleChild");
               searchPage.FindPathwayInCathgryList("Lower back injury - skin not broken","PW1596FemaleChild");
               searchPage.FindPathwayInCathgryList("Lower back injury with broken skin","PW798FemaleChild");
               searchPage.FindPathwayInCathgryList("Lower back pain","PW783FemaleChild");
               searchPage.FindPathwayInCathgryList("Painful legs","PW775FemaleChild");
               searchPage.FindPathwayInCathgryList("Swollen legs","PW775FemaleChild");
               searchPage.FindPathwayInCathgryList("Mental health problems","PW1751FemaleChild");
               searchPage.FindPathwayInCathgryList("Low mood","PW1751FemaleChild");
               searchPage.FindPathwayInCathgryList("Depression","PW1751FemaleChild");
               searchPage.FindPathwayInCathgryList("Anxiety","PW1751FemaleChild");
               searchPage.FindPathwayInCathgryList("Mouth ulcers","PW1323FemaleChild");
               searchPage.FindPathwayInCathgryList("Blocked nose","PW984FemaleChild");
               searchPage.FindPathwayInCathgryList("Nosebleed after an injury","PW1713FemaleChild");
               searchPage.FindPathwayInCathgryList("Nosebleed","PW819FemaleChild");
               searchPage.FindPathwayInCathgryList("Numbness","PW1683FemaleChild");
               searchPage.FindPathwayInCathgryList("Tingling","PW1683FemaleChild");
               searchPage.FindPathwayInCathgryList("Swallowed an object","PW1034FemaleChild");
               searchPage.FindPathwayInCathgryList("Breathed in an object","PW1034FemaleChild");
               searchPage.FindPathwayInCathgryList("Other symptoms","PW1348FemaleChild");
               searchPage.FindPathwayInCathgryList("Palpitations","PW1029FemaleChild");
               searchPage.FindPathwayInCathgryList("Pounding heart","PW1029FemaleChild");
               searchPage.FindPathwayInCathgryList("Fluttering heart","PW1029FemaleChild");
               searchPage.FindPathwayInCathgryList("Urinary problems","PW645FemaleChild");
               searchPage.FindPathwayInCathgryList("Bleeding from the bottom/back passage","PW846FemaleChild");
               searchPage.FindPathwayInCathgryList("Swollen bottom/back passage","PW1091FemaleChild");
               searchPage.FindPathwayInCathgryList("Painful bottom/back passage","PW1091FemaleChild");
               searchPage.FindPathwayInCathgryList("Itchy bottom/back passage","PW1091FemaleChild");
               searchPage.FindPathwayInCathgryList("Can't feel baby moving as much","PW1763FemaleChild");
               searchPage.FindPathwayInCathgryList("Grazes","PW1590FemaleChild");
               searchPage.FindPathwayInCathgryList("Minor wounds","PW1590FemaleChild");
               searchPage.FindPathwayInCathgryList("Scratches","PW1590FemaleChild");
               searchPage.FindPathwayInCathgryList("Sexual concerns - female","PW1699FemaleChild");
               searchPage.FindPathwayInCathgryList("Period concerns","PW1699FemaleChild");
               searchPage.FindPathwayInCathgryList("Shoulder pain","PW1141FemaleChild");
               searchPage.FindPathwayInCathgryList("Sinusitis","PW1050FemaleChild");
               searchPage.FindPathwayInCathgryList("Rashes, itching, spots, moles and other skin problems","PW1772FemaleChild");
               searchPage.FindPathwayInCathgryList("Glue on the skin","PW1301FemaleChild");
               searchPage.FindPathwayInCathgryList("Something under the skin","PW1259FemaleChild");
               searchPage.FindPathwayInCathgryList("Splinter","PW1259FemaleChild");
               searchPage.FindPathwayInCathgryList("Sleep problems","PW1697FemaleChild");
               searchPage.FindPathwayInCathgryList("Sore throat","PW854FemaleChild");
               searchPage.FindPathwayInCathgryList("Hoarse voice","PW854FemaleChild");
               searchPage.FindPathwayInCathgryList("Stoma problems","PW1719FemaleChild");
               searchPage.FindPathwayInCathgryList("Stroke symptoms","PA171FemaleChild");
               searchPage.FindPathwayInCathgryList("Tiredness","PW1071FemaleChild");
               searchPage.FindPathwayInCathgryList("Fatigue","PW1071FemaleChild");
               searchPage.FindPathwayInCathgryList("Toe injury - skin not broken","PW1282FemaleChild");
               searchPage.FindPathwayInCathgryList("Toe injury with broken skin","PW1526FemaleChild");
               searchPage.FindPathwayInCathgryList("Toenail injury","PW1593FemaleChild");
               searchPage.FindPathwayInCathgryList("Toothache after an injury","PW572FemaleChild");
               searchPage.FindPathwayInCathgryList("Blisters","PW1625FemaleChild");
               searchPage.FindPathwayInCathgryList("Trembling","PW1764FemaleChild");
               searchPage.FindPathwayInCathgryList("Shaking","PW1764FemaleChild");
               searchPage.FindPathwayInCathgryList("Bleeding from the vagina","PW911FemaleChild");
               searchPage.FindPathwayInCathgryList("Discharge from the vagina","PW916FemaleChild");
               searchPage.FindPathwayInCathgryList("Itchy vagina","PW1560FemaleChild");
               searchPage.FindPathwayInCathgryList("Sore vagina","PW1560FemaleChild");
               searchPage.FindPathwayInCathgryList("Swelling in or around the vagina","PW1103FemaleChild");
               searchPage.FindPathwayInCathgryList("Vomiting and nausea - no diarrhoea","PW937FemaleChild");
               searchPage.FindPathwayInCathgryList("Wound problems","PW1776FemaleChild");
               searchPage.FindPathwayInCathgryList("Wound problems - plaster casts","PW1709FemaleChild");
               searchPage.FindPathwayInCathgryList("Wound problems - drainage tubes","PW1709FemaleChild");
               searchPage.FindPathwayInCathgryList("Wound problems - metal attachments","PW1709FemaleChild");
               searchPage.FindPathwayInCathgryList("Swollen finger","PW1616FemaleChild");
               searchPage.FindPathwayInCathgryList("Painful finger","PW1616FemaleChild");
               searchPage.FindPathwayInCathgryList("Swollen hand","PW1616FemaleChild");
               searchPage.FindPathwayInCathgryList("Painful hand","PW1616FemaleChild");
               searchPage.FindPathwayInCathgryList("Swollen wrist","PW1616FemaleChild");
               searchPage.FindPathwayInCathgryList("Painful wrist","PW1616FemaleChild");
               searchPage.FindPathwayInCathgryList("Self-harm","PW1543FemaleChild");

               searchPage.SelectCategory("bowel-and-urinary-problems");
               searchPage.SelectCategory("bowel-and-urinary-problems-bowel-problems");
               searchPage.SelectPathway("Something in the bottom/back passage");

            _driver.FindElement(By.XPath("//input[@value = 'PW1531FemaleChild']"));
                  }
           [Test]
           public void CategoriespresentForFemaleAdult()
           //Test to show all categories are present per age and gender, and also the correct pathway opens per age and gender, when the same pathway is used for all ages/genders.
           {

               var searchPage = TestScenerios.LaunchSearchScenerio(_driver, TestScenerioGender.Female, 40);

               _driver.FindElement(By.Id("searchButton")).Click();
               searchPage.FindPathwayInCathgryList("Can't feel baby moving as much", "PW1763FemaleAdult");
               searchPage.FindPathwayInCathgryList("Abdomen injury - skin not broken","PW500FemaleAdult");
               searchPage.FindPathwayInCathgryList("Injury to the side of the body - skin not broken","PW500FemaleAdult");
               searchPage.FindPathwayInCathgryList("Abdomen injury with broken skin","PW508FemaleAdult");
               searchPage.FindPathwayInCathgryList("Injury to the side of the body with broken skin","PW508FemaleAdult");
               searchPage.FindPathwayInCathgryList("Abdominal pain","PW516FemaleAdult");
               searchPage.FindPathwayInCathgryList("Stomach pain","PW516FemaleAdult");
               searchPage.FindPathwayInCathgryList("Absent periods","PW1676FemaleAdult");
               searchPage.FindPathwayInCathgryList("Missed periods","PW1676FemaleAdult");
               searchPage.FindPathwayInCathgryList("Accidental poisoning","PW881FemaleAdult");
               searchPage.FindPathwayInCathgryList("Breathed in something poisonous","PW881FemaleAdult");
               searchPage.FindPathwayInCathgryList("Swallowed something poisonous","PW881FemaleAdult");
               searchPage.FindPathwayInCathgryList("Drunk too much alcohol","PW1551FemaleAdult");
               searchPage.FindPathwayInCathgryList("Alcohol intoxication","PW1551FemaleAdult");
               searchPage.FindPathwayInCathgryList("Ankle injury - skin not broken","PW1512FemaleAdult");
               searchPage.FindPathwayInCathgryList("Foot injury - skin not broken","PW1512FemaleAdult");
               searchPage.FindPathwayInCathgryList("Ankle injury with broken skin","PW1518FemaleAdult");
               searchPage.FindPathwayInCathgryList("Foot injury with broken skin","PW1518FemaleAdult");
               searchPage.FindPathwayInCathgryList("Arm injury - skin not broken","PW894FemaleAdult");
               searchPage.FindPathwayInCathgryList("Arm injury with broken skin","PW902FemaleAdult");
               searchPage.FindPathwayInCathgryList("Painful arm","PW1164FemaleAdult");
               searchPage.FindPathwayInCathgryList("Swollen arm","PW1164FemaleAdult");
               searchPage.FindPathwayInCathgryList("Cold arm","PW1733FemaleAdult");
               searchPage.FindPathwayInCathgryList("Arm changing colour","PW1733FemaleAdult");
               searchPage.FindPathwayInCathgryList("Change in behaviour","PW1749FemaleAdult");
               searchPage.FindPathwayInCathgryList("Bites and stings","PW1575FemaleAdult");
               searchPage.FindPathwayInCathgryList("Bleeding from stoma","PT507FemaleAdult");
               searchPage.FindPathwayInCathgryList("Blood in urine","PW961FemaleAdult");
               searchPage.FindPathwayInCathgryList("Breast problems","PW1603FemaleAdult");
               searchPage.FindPathwayInCathgryList("Breastfeeding problems","PW1114FemaleAdult");
               searchPage.FindPathwayInCathgryList("Breathing problems","PW556FemaleAdult");
               searchPage.FindPathwayInCathgryList("Upper back pain","PW556FemaleAdult");
               searchPage.FindPathwayInCathgryList("Chest and upper back pain","PW556FemaleAdult");
               searchPage.FindPathwayInCathgryList("Chest pain","PW556FemaleAdult");
               searchPage.FindPathwayInCathgryList("Wheezing","PW556FemaleAdult");
               searchPage.FindPathwayInCathgryList("Coughing up blood","PW1651FemaleAdult");
               searchPage.FindPathwayInCathgryList("Bringing up blood","PW1651FemaleAdult");
               searchPage.FindPathwayInCathgryList("Burns from chemicals","PW564FemaleAdult");
               searchPage.FindPathwayInCathgryList("Sunburn","PW987FemaleAdult");
               searchPage.FindPathwayInCathgryList("Burns from heat","PW580FemaleAdult");
               searchPage.FindPathwayInCathgryList("Catheter problems","PW1567FemaleAdult");
               searchPage.FindPathwayInCathgryList("Chest injury - skin not broken","PW588FemaleAdult");
               searchPage.FindPathwayInCathgryList("Upper back injury - skin not broken","PW588FemaleAdult");
               searchPage.FindPathwayInCathgryList("Chest injury with broken skin","PW596FemaleAdult");
               searchPage.FindPathwayInCathgryList("Upper back injury with broken skin","PW596FemaleAdult");
               searchPage.FindPathwayInCathgryList("Colds and flu","PW1040FemaleAdult");
               searchPage.FindPathwayInCathgryList("Constipation","PW1158FemaleAdult");
               searchPage.FindPathwayInCathgryList("Cough","PW975FemaleAdult");
               searchPage.FindPathwayInCathgryList("Self-harm","PW1543FemaleAdult");
               searchPage.FindPathwayInCathgryList("Dental injury","PW620FemaleAdult");
               searchPage.FindPathwayInCathgryList("Toothache and other dental problems","PW1610FemaleAdult");
               searchPage.FindPathwayInCathgryList("Diabetes - blood sugar problems","PW1746FemaleAdult");
               searchPage.FindPathwayInCathgryList("Diarrhoea - no vomiting","PW628FemaleAdult");
               searchPage.FindPathwayInCathgryList("Diarrhoea and vomiting","PW1553FemaleAdult");
               searchPage.FindPathwayInCathgryList("Difficulty passing urine","PW886FemaleAdult");
               searchPage.FindPathwayInCathgryList("Difficulty swallowing","PW1496FemaleAdult");
               searchPage.FindPathwayInCathgryList("Dizziness","PW636FemaleAdult");
               searchPage.FindPathwayInCathgryList("Vertigo","PW636FemaleAdult");
               searchPage.FindPathwayInCathgryList("Ear discharge","PW1702FemaleAdult");
               searchPage.FindPathwayInCathgryList("Earwax","PW1702FemaleAdult");
               searchPage.FindPathwayInCathgryList("Earache","PW655FemaleAdult");
               searchPage.FindPathwayInCathgryList("Eye injury - no damage to surface of eye","PW660FemaleAdult");
               searchPage.FindPathwayInCathgryList("Eye injury with damage to surface of eye","PW668FemaleAdult");
               searchPage.FindPathwayInCathgryList("Eye problems","PW1627FemaleAdult");
               searchPage.FindPathwayInCathgryList("Eyelid problems","PW1627FemaleAdult");
               searchPage.FindPathwayInCathgryList("Something in the eye","PW1098FemaleAdult");
               searchPage.FindPathwayInCathgryList("Swollen face","PW1544FemaleAdult");
               searchPage.FindPathwayInCathgryList("Painful face","PW1544FemaleAdult");
               searchPage.FindPathwayInCathgryList("Swollen neck","PW1544FemaleAdult");
               searchPage.FindPathwayInCathgryList("Painful neck","PW1544FemaleAdult");
               searchPage.FindPathwayInCathgryList("Falling","PW700FemaleAdult");
               searchPage.FindPathwayInCathgryList("Fainting","PW700FemaleAdult");
               searchPage.FindPathwayInCathgryList("Passing out","PW700FemaleAdult");
               searchPage.FindPathwayInCathgryList("Fever","PW708FemaleAdult");
               searchPage.FindPathwayInCathgryList("High temperature","PW708FemaleAdult");
               searchPage.FindPathwayInCathgryList("Finger injury - skin not broken","PW1270FemaleAdult");
               searchPage.FindPathwayInCathgryList("Thumb injury - skin not broken","PW1270FemaleAdult");
               searchPage.FindPathwayInCathgryList("Finger injury with broken skin","PW1264FemaleAdult");
               searchPage.FindPathwayInCathgryList("Thumb injury with broken skin","PW1264FemaleAdult");
               searchPage.FindPathwayInCathgryList("Fingernail injury","PW1570FemaleAdult");
               searchPage.FindPathwayInCathgryList("Pain in the side of the body","PW716FemaleAdult");
               searchPage.FindPathwayInCathgryList("Something in the ear","PW1528FemaleAdult");
               searchPage.FindPathwayInCathgryList("Something in the nose","PW1529FemaleAdult");
               searchPage.FindPathwayInCathgryList("Something in the bottom/back passage","PW1531FemaleAdult");
               searchPage.FindPathwayInCathgryList("Something in the vagina","PW1532FemaleAdult");
               searchPage.FindPathwayInCathgryList("Genital injury - skin not broken","PW1116FemaleAdult");
               searchPage.FindPathwayInCathgryList("Genital injury with broken skin","PW1010FemaleAdult");
               searchPage.FindPathwayInCathgryList("Swollen groin","PW728FemaleAdult");
               searchPage.FindPathwayInCathgryList("Painful groin","PW728FemaleAdult");
               searchPage.FindPathwayInCathgryList("Hair loss","PW1678FemaleAdult");
               searchPage.FindPathwayInCathgryList("Hand injury - skin not broken","PW1267FemaleAdult");
               searchPage.FindPathwayInCathgryList("Wrist injury - skin not broken","PW1267FemaleAdult");
               searchPage.FindPathwayInCathgryList("Hand injury with broken skin","PW1260FemaleAdult");
               searchPage.FindPathwayInCathgryList("Wrist injury with broken skin","PW1260FemaleAdult");
               searchPage.FindPathwayInCathgryList("Head injury - skin not broken","PW684FemaleAdult");
               searchPage.FindPathwayInCathgryList("Face injury - skin not broken","PW684FemaleAdult");
               searchPage.FindPathwayInCathgryList("Neck injury - skin not broken","PW684FemaleAdult");
               searchPage.FindPathwayInCathgryList("Head injury with broken skin","PW692FemaleAdult");
               searchPage.FindPathwayInCathgryList("Face injury with broken skin","PW692FemaleAdult");
               searchPage.FindPathwayInCathgryList("Neck injury with broken skin","PW692FemaleAdult");
               searchPage.FindPathwayInCathgryList("Headache","PW752FemaleAdult");
               searchPage.FindPathwayInCathgryList("Hearing problems","PW1762FemaleAdult");
               searchPage.FindPathwayInCathgryList("Blocked ear","PW1762FemaleAdult");
               searchPage.FindPathwayInCathgryList("Heatstroke","PW998FemaleAdult");
               searchPage.FindPathwayInCathgryList("Heat exhaustion","PW998FemaleAdult");
               searchPage.FindPathwayInCathgryList("Hiccups","PW1775FemaleAdult");
               searchPage.FindPathwayInCathgryList("Leg injury - skin not broken","PW1240FemaleAdult");
               searchPage.FindPathwayInCathgryList("Leg injury with broken skin","PW1234FemaleAdult");
               searchPage.FindPathwayInCathgryList("Leg changing colour","PW1734FemaleAdult");
               searchPage.FindPathwayInCathgryList("Cold leg","PW1734FemaleAdult");
               searchPage.FindPathwayInCathgryList("Locked jaw","PW1712FemaleAdult");
               searchPage.FindPathwayInCathgryList("Loss of bowel control","PW1759FemaleAdult");
               searchPage.FindPathwayInCathgryList("Bowel incontinence","PW1759FemaleAdult");
               searchPage.FindPathwayInCathgryList("Lower back injury - skin not broken","PW790FemaleAdult");
               searchPage.FindPathwayInCathgryList("Lower back injury with broken skin","PW798FemaleAdult");
               searchPage.FindPathwayInCathgryList("Lower back pain","PW782FemaleAdult");
               searchPage.FindPathwayInCathgryList("Painful legs","PW774FemaleAdult");
               searchPage.FindPathwayInCathgryList("Swollen legs","PW774FemaleAdult");
               searchPage.FindPathwayInCathgryList("Mental health problems","PW1751FemaleAdult");
               searchPage.FindPathwayInCathgryList("Low mood","PW1751FemaleAdult");
               searchPage.FindPathwayInCathgryList("Depression","PW1751FemaleAdult");
               searchPage.FindPathwayInCathgryList("Anxiety","PW1751FemaleAdult");
               searchPage.FindPathwayInCathgryList("Mouth ulcers","PW1323FemaleAdult");
               searchPage.FindPathwayInCathgryList("Blocked nose","PW981FemaleAdult");
               searchPage.FindPathwayInCathgryList("Nosebleed after an injury","PW1713FemaleAdult");
               searchPage.FindPathwayInCathgryList("Nosebleed","PW818FemaleAdult");
               searchPage.FindPathwayInCathgryList("Numbness","PW1683FemaleAdult");
               searchPage.FindPathwayInCathgryList("Tingling","PW1683FemaleAdult");
               searchPage.FindPathwayInCathgryList("Swallowed an object","PW1034FemaleAdult");
               searchPage.FindPathwayInCathgryList("Breathed in an object","PW1034FemaleAdult");
               searchPage.FindPathwayInCathgryList("Other symptoms","PW1345FemaleAdult");
               searchPage.FindPathwayInCathgryList("Palpitations","PW1028FemaleAdult");
               searchPage.FindPathwayInCathgryList("Pounding heart","PW1028FemaleAdult");
               searchPage.FindPathwayInCathgryList("Fluttering heart","PW1028FemaleAdult");
               searchPage.FindPathwayInCathgryList("Urinary problems","PW644FemaleAdult");
               searchPage.FindPathwayInCathgryList("Bleeding from the bottom/back passage","PW846FemaleAdult");
               searchPage.FindPathwayInCathgryList("Swollen bottom/back passage","PW1091FemaleAdult");
               searchPage.FindPathwayInCathgryList("Painful bottom/back passage","PW1091FemaleAdult");
               searchPage.FindPathwayInCathgryList("Itchy bottom/back passage","PW1091FemaleAdult");
               searchPage.FindPathwayInCathgryList("Grazes","PW1590FemaleAdult");
               searchPage.FindPathwayInCathgryList("Minor wounds","PW1590FemaleAdult");
               searchPage.FindPathwayInCathgryList("Scratches","PW1590FemaleAdult");
               searchPage.FindPathwayInCathgryList("Sexual concerns - female","PW1684FemaleAdult");
               searchPage.FindPathwayInCathgryList("Period concerns","PW1684FemaleAdult");
               searchPage.FindPathwayInCathgryList("Shoulder pain","PW1140FemaleAdult");
               searchPage.FindPathwayInCathgryList("Sinusitis","PW1046FemaleAdult");
               searchPage.FindPathwayInCathgryList("Rashes, itching, spots, moles and other skin problems","PW1771FemaleAdult");
               searchPage.FindPathwayInCathgryList("Glue on the skin","PW1301FemaleAdult");
               searchPage.FindPathwayInCathgryList("Something under the skin","PW1259FemaleAdult");
               searchPage.FindPathwayInCathgryList("Splinter","PW1259FemaleAdult");
               searchPage.FindPathwayInCathgryList("Sleep problems","PW1686FemaleAdult");
               searchPage.FindPathwayInCathgryList("Sore throat","PW854FemaleAdult");
               searchPage.FindPathwayInCathgryList("Hoarse voice","PW854FemaleAdult");
               searchPage.FindPathwayInCathgryList("Stoma problems","PW1719FemaleAdult");
               searchPage.FindPathwayInCathgryList("Tiredness","PW1070FemaleAdult");
               searchPage.FindPathwayInCathgryList("Fatigue","PW1070FemaleAdult");
               searchPage.FindPathwayInCathgryList("Toe injury - skin not broken","PW1282FemaleAdult");
               searchPage.FindPathwayInCathgryList("Toe injury with broken skin","PW1526FemaleAdult");
               searchPage.FindPathwayInCathgryList("Toenail injury","PW1593FemaleAdult");
               searchPage.FindPathwayInCathgryList("Toothache after an injury","PW572FemaleAdult");
               searchPage.FindPathwayInCathgryList("Blisters","PW1625FemaleAdult");
               searchPage.FindPathwayInCathgryList("Trembling","PW1764FemaleAdult");
               searchPage.FindPathwayInCathgryList("Shaking","PW1764FemaleAdult");
               searchPage.FindPathwayInCathgryList("Bleeding from the vagina","PW910FemaleAdult");
               searchPage.FindPathwayInCathgryList("Discharge from the vagina","PW915FemaleAdult");
               searchPage.FindPathwayInCathgryList("Itchy vagina","PW1559FemaleAdult");
               searchPage.FindPathwayInCathgryList("Sore vagina","PW1559FemaleAdult");
               searchPage.FindPathwayInCathgryList("Swelling in or around the vagina","PW1102FemaleAdult");
               searchPage.FindPathwayInCathgryList("Vomiting and nausea - no diarrhoea","PW936FemaleAdult");
               searchPage.FindPathwayInCathgryList("Wound problems","PW1776FemaleAdult");
               searchPage.FindPathwayInCathgryList("Wound problems - plaster casts","PW1709FemaleAdult");
               searchPage.FindPathwayInCathgryList("Wound problems - drainage tubes","PW1709FemaleAdult");
               searchPage.FindPathwayInCathgryList("Wound problems - metal attachments","PW1709FemaleAdult");
               searchPage.FindPathwayInCathgryList("Swollen finger","PW1614FemaleAdult");
               searchPage.FindPathwayInCathgryList("Painful finger","PW1614FemaleAdult");
               searchPage.FindPathwayInCathgryList("Swollen hand","PW1614FemaleAdult");
               searchPage.FindPathwayInCathgryList("Painful hand","PW1614FemaleAdult");
               searchPage.FindPathwayInCathgryList("Swollen wrist","PW1614FemaleAdult");
               searchPage.FindPathwayInCathgryList("Painful wrist","PW1614FemaleAdult");
               searchPage.FindPathwayInCathgryList("Self-harm","PW1543FemaleAdult");


               searchPage.SelectCategory("bowel-and-urinary-problems");
               searchPage.SelectCategory("bowel-and-urinary-problems-bowel-problems");
               searchPage.SelectPathway("Something in the bottom/back passage");

               _driver.FindElement(By.XPath("//input[@value = 'PW1531FemaleAdult']"));
           }

           [Test]
           public void CategoriespresentForMaleChild()

                   //Test to show all categories are present per age and gender, and also the correct pathway opens per age and gender, when the same pathway is used for all ages/genders.
           {

               var searchPage = TestScenerios.LaunchSearchScenerio(_driver, TestScenerioGender.Male, 5);

               searchPage.ClickGoButton();

               searchPage.FindPathwayInCathgryList("Abdomen injury - skin not broken", "PW503MaleChild");
               searchPage.FindPathwayInCathgryList("Injury to the side of the body - skin not broken", "PW503MaleChild");
               searchPage.FindPathwayInCathgryList("Abdomen injury with broken skin", "PW511MaleChild");
               searchPage.FindPathwayInCathgryList("Injury to the side of the body with broken skin", "PW511MaleChild");
               searchPage.FindPathwayInCathgryList("Abdominal pain", "PW520MaleChild");
               searchPage.FindPathwayInCathgryList("Stomach pain", "PW520MaleChild");
               searchPage.FindPathwayInCathgryList("Accidental poisoning", "PW881MaleChild");
               searchPage.FindPathwayInCathgryList("Breathed in something poisonous", "PW881MaleChild");
               searchPage.FindPathwayInCathgryList("Swallowed something poisonous", "PW881MaleChild");
               searchPage.FindPathwayInCathgryList("Drunk too much alcohol", "PW1552MaleChild");
               searchPage.FindPathwayInCathgryList("Alcohol intoxication", "PW1552MaleChild");
               searchPage.FindPathwayInCathgryList("Ankle injury - skin not broken", "PW1512MaleChild");
               searchPage.FindPathwayInCathgryList("Foot injury - skin not broken", "PW1512MaleChild");
               searchPage.FindPathwayInCathgryList("Ankle injury with broken skin", "PW1518MaleChild");
               searchPage.FindPathwayInCathgryList("Foot injury with broken skin", "PW1518MaleChild");
               searchPage.FindPathwayInCathgryList("Arm injury - skin not broken", "PW895MaleChild");
               searchPage.FindPathwayInCathgryList("Arm injury with broken skin", "PW902MaleChild");
               searchPage.FindPathwayInCathgryList("Painful arm", "PW1167MaleChild");
               searchPage.FindPathwayInCathgryList("Swollen arm", "PW1167MaleChild");
               searchPage.FindPathwayInCathgryList("Cold arm", "PW1733MaleChild");
               searchPage.FindPathwayInCathgryList("Arm changing colour", "PW1733MaleChild");
               searchPage.FindPathwayInCathgryList("Change in behaviour", "PW1749MaleChild");
               searchPage.FindPathwayInCathgryList("Bites and stings", "PW1575MaleChild");
               searchPage.FindPathwayInCathgryList("Bleeding from stoma", "PT507MaleChild");
               searchPage.FindPathwayInCathgryList("Blood in urine", "PW962MaleChild");
               searchPage.FindPathwayInCathgryList("Breast problems", "PW1606MaleChild");
               searchPage.FindPathwayInCathgryList("Breathing problems", "PW560MaleChild");
               searchPage.FindPathwayInCathgryList("Upper back pain", "PW560MaleChild");
               searchPage.FindPathwayInCathgryList("Chest and upper back pain", "PW560MaleChild");
               searchPage.FindPathwayInCathgryList("Chest pain", "PW560MaleChild");
               searchPage.FindPathwayInCathgryList("Wheezing", "PW560MaleChild");
               searchPage.FindPathwayInCathgryList("Coughing up blood", "PW1654MaleChild");
               searchPage.FindPathwayInCathgryList("Bringing up blood", "PW1654MaleChild");
               searchPage.FindPathwayInCathgryList("Burns from chemicals", "PW564MaleChild");
               searchPage.FindPathwayInCathgryList("Sunburn", "PW987MaleChild");
               searchPage.FindPathwayInCathgryList("Burns from heat", "PW580MaleChild");
               searchPage.FindPathwayInCathgryList("Catheter problems", "PW1567MaleChild");
               searchPage.FindPathwayInCathgryList("Chest injury - skin not broken", "PW588MaleChild");
               searchPage.FindPathwayInCathgryList("Upper back injury - skin not broken", "PW588MaleChild");
               searchPage.FindPathwayInCathgryList("Chest injury with broken skin", "PW596MaleChild");
               searchPage.FindPathwayInCathgryList("Upper back injury with broken skin", "PW596MaleChild");
               searchPage.FindPathwayInCathgryList("Colds and flu", "PW1043MaleChild");
               searchPage.FindPathwayInCathgryList("Constipation", "PW1162MaleChild");
               searchPage.FindPathwayInCathgryList("Cough", "PW979MaleChild");
               searchPage.FindPathwayInCathgryList("Self-harm", "PW1543MaleChild");
               searchPage.FindPathwayInCathgryList("Dental injury", "PW620MaleChild");
               searchPage.FindPathwayInCathgryList("Toothache and other dental problems", "PW1611MaleChild");
               searchPage.FindPathwayInCathgryList("Diabetes - blood sugar problems", "PW1746MaleChild");
               searchPage.FindPathwayInCathgryList("Diarrhoea - no vomiting", "PW632MaleChild");
               searchPage.FindPathwayInCathgryList("Diarrhoea and vomiting", "PW1556MaleChild");
               searchPage.FindPathwayInCathgryList("Difficulty passing urine", "PW886MaleChild");
               searchPage.FindPathwayInCathgryList("Difficulty swallowing", "PW1496MaleChild");
               searchPage.FindPathwayInCathgryList("Dizziness", "PW640MaleChild");
               searchPage.FindPathwayInCathgryList("Vertigo", "PW640MaleChild");
               searchPage.FindPathwayInCathgryList("Ear discharge", "PW1702MaleChild");
               searchPage.FindPathwayInCathgryList("Earwax", "PW1702MaleChild");
               searchPage.FindPathwayInCathgryList("Earache", "PW656MaleChild");
               searchPage.FindPathwayInCathgryList("Eye injury - no damage to surface of eye", "PW660MaleChild");
               searchPage.FindPathwayInCathgryList("Eye injury with damage to surface of eye", "PW668MaleChild");
               searchPage.FindPathwayInCathgryList("Eye problems", "PW1630MaleChild");
               searchPage.FindPathwayInCathgryList("Eyelid problems", "PW1630MaleChild");
               searchPage.FindPathwayInCathgryList("Something in the eye", "PW1098MaleChild");
               searchPage.FindPathwayInCathgryList("Swollen face", "PW1549MaleChild");
               searchPage.FindPathwayInCathgryList("Painful face", "PW1549MaleChild");
               searchPage.FindPathwayInCathgryList("Swollen neck", "PW1549MaleChild");
               searchPage.FindPathwayInCathgryList("Painful neck", "PW1549MaleChild");
               searchPage.FindPathwayInCathgryList("Falling", "PW700MaleChild");
               searchPage.FindPathwayInCathgryList("Fainting", "PW700MaleChild");
               searchPage.FindPathwayInCathgryList("Passing out", "PW700MaleChild");
               searchPage.FindPathwayInCathgryList("Fever", "PW712MaleChild");
               searchPage.FindPathwayInCathgryList("High temperature", "PW712MaleChild");
               searchPage.FindPathwayInCathgryList("Finger injury - skin not broken", "PW1270MaleChild");
               searchPage.FindPathwayInCathgryList("Thumb injury - skin not broken", "PW1270MaleChild");
               searchPage.FindPathwayInCathgryList("Finger injury with broken skin", "PW1264MaleChild");
               searchPage.FindPathwayInCathgryList("Thumb injury with broken skin", "PW1264MaleChild");
               searchPage.FindPathwayInCathgryList("Fingernail injury", "PW1570MaleChild");
               searchPage.FindPathwayInCathgryList("Pain in the side of the body", "PW720MaleChild");
               searchPage.FindPathwayInCathgryList("Something in the ear", "PW1528MaleChild");
               searchPage.FindPathwayInCathgryList("Something in the nose", "PW1529MaleChild");
               searchPage.FindPathwayInCathgryList("Something in the penis", "PW1530MaleChild");
               searchPage.FindPathwayInCathgryList("Something in the bottom/back passage", "PW1531MaleChild");
               searchPage.FindPathwayInCathgryList("Genital injury - skin not broken", "PW1118MaleChild");
               searchPage.FindPathwayInCathgryList("Genital injury with broken skin", "PW1012MaleChild");
               searchPage.FindPathwayInCathgryList("Genital problems", "PW1565MaleChild");
               searchPage.FindPathwayInCathgryList("Swollen groin", "PW732MaleChild");
               searchPage.FindPathwayInCathgryList("Painful groin", "PW732MaleChild");
               searchPage.FindPathwayInCathgryList("Hair loss", "PW1678MaleChild");
               searchPage.FindPathwayInCathgryList("Hand injury - skin not broken", "PW1267MaleChild");
               searchPage.FindPathwayInCathgryList("Wrist injury - skin not broken", "PW1267MaleChild");
               searchPage.FindPathwayInCathgryList("Hand injury with broken skin", "PW1260MaleChild");
               searchPage.FindPathwayInCathgryList("Wrist injury with broken skin", "PW1260MaleChild");
               searchPage.FindPathwayInCathgryList("Head injury - skin not broken", "PW684MaleChild");
               searchPage.FindPathwayInCathgryList("Face injury - skin not broken", "PW684MaleChild");
               searchPage.FindPathwayInCathgryList("Neck injury - skin not broken", "PW684MaleChild");
               searchPage.FindPathwayInCathgryList("Head injury with broken skin", "PW692MaleChild");
               searchPage.FindPathwayInCathgryList("Face injury with broken skin", "PW692MaleChild");
               searchPage.FindPathwayInCathgryList("Neck injury with broken skin", "PW692MaleChild");
               searchPage.FindPathwayInCathgryList("Headache", "PW756MaleChild");
               searchPage.FindPathwayInCathgryList("Hearing problems", "PW1762MaleChild");
               searchPage.FindPathwayInCathgryList("Blocked ear", "PW1762MaleChild");
               searchPage.FindPathwayInCathgryList("Heatstroke", "PW998MaleChild");
               searchPage.FindPathwayInCathgryList("Heat exhaustion", "PW998MaleChild");
               searchPage.FindPathwayInCathgryList("Hiccups", "PW1775MaleChild");
               searchPage.FindPathwayInCathgryList("Leg injury - skin not broken", "PW1591MaleChild");
               searchPage.FindPathwayInCathgryList("Leg injury with broken skin", "PW1234MaleChild");
               searchPage.FindPathwayInCathgryList("Leg changing colour", "PW1734MaleChild");
               searchPage.FindPathwayInCathgryList("Cold leg", "PW1734MaleChild");
               searchPage.FindPathwayInCathgryList("Locked jaw", "PW1712MaleChild");
               searchPage.FindPathwayInCathgryList("Loss of bowel control", "PW1759MaleChild");
               searchPage.FindPathwayInCathgryList("Bowel incontinence", "PW1759MaleChild");
               searchPage.FindPathwayInCathgryList("Lower back injury - skin not broken", "PW1597MaleChild");
               searchPage.FindPathwayInCathgryList("Lower back injury with broken skin", "PW801MaleChild");
               searchPage.FindPathwayInCathgryList("Lower back pain", "PW786MaleChild");
               searchPage.FindPathwayInCathgryList("Painful legs", "PW778MaleChild");
               searchPage.FindPathwayInCathgryList("Swollen legs", "PW778MaleChild");
               searchPage.FindPathwayInCathgryList("Mental health problems", "PW1751MaleChild");
               searchPage.FindPathwayInCathgryList("Low mood", "PW1751MaleChild");
               searchPage.FindPathwayInCathgryList("Depression", "PW1751MaleChild");
               searchPage.FindPathwayInCathgryList("Anxiety", "PW1751MaleChild");
               searchPage.FindPathwayInCathgryList("Mouth ulcers", "PW1323MaleChild");
               searchPage.FindPathwayInCathgryList("Blocked nose", "PW984MaleChild");
               searchPage.FindPathwayInCathgryList("Nosebleed after an injury", "PW1713MaleChild");
               searchPage.FindPathwayInCathgryList("Nosebleed", "PW819MaleChild");
               searchPage.FindPathwayInCathgryList("Numbness", "PW1683MaleChild");
               searchPage.FindPathwayInCathgryList("Tingling", "PW1683MaleChild");
               searchPage.FindPathwayInCathgryList("Swallowed an object", "PW1034MaleChild");
               searchPage.FindPathwayInCathgryList("Breathed in an object", "PW1034MaleChild");
               searchPage.FindPathwayInCathgryList("Other symptoms", "PW1349MaleChild");
               searchPage.FindPathwayInCathgryList("Palpitations", "PW1031MaleChild");
               searchPage.FindPathwayInCathgryList("Pounding heart", "PW1031MaleChild");
               searchPage.FindPathwayInCathgryList("Fluttering heart", "PW1031MaleChild");
               searchPage.FindPathwayInCathgryList("Urinary problems", "PW648MaleChild");
               searchPage.FindPathwayInCathgryList("Bleeding from the bottom/back passage", "PW846MaleChild");
               searchPage.FindPathwayInCathgryList("Swollen bottom/back passage", "PW1091MaleChild");
               searchPage.FindPathwayInCathgryList("Painful bottom/back passage", "PW1091MaleChild");
               searchPage.FindPathwayInCathgryList("Itchy bottom/back passage", "PW1091MaleChild");
               searchPage.FindPathwayInCathgryList("Grazes", "PW1590MaleChild");
               searchPage.FindPathwayInCathgryList("Minor wounds", "PW1590MaleChild");
               searchPage.FindPathwayInCathgryList("Scratches", "PW1590MaleChild");
               searchPage.FindPathwayInCathgryList("Sexual concerns - male", "PW1698MaleChild");
               searchPage.FindPathwayInCathgryList("Shoulder pain", "PW1143MaleChild");
               searchPage.FindPathwayInCathgryList("Sinusitis", "PW1602MaleChild");
               searchPage.FindPathwayInCathgryList("Rashes, itching, spots, moles and other skin problems", "PW1772MaleChild");
               searchPage.FindPathwayInCathgryList("Glue on the skin", "PW1301MaleChild");
               searchPage.FindPathwayInCathgryList("Something under the skin", "PW1754MaleChild");
               searchPage.FindPathwayInCathgryList("Splinter", "PW1754MaleChild");
               searchPage.FindPathwayInCathgryList("Sleep problems", "PW1697MaleChild");
               searchPage.FindPathwayInCathgryList("Sore throat", "PW854MaleChild");
               searchPage.FindPathwayInCathgryList("Hoarse voice", "PW854MaleChild");
               searchPage.FindPathwayInCathgryList("Stoma problems", "PW1719MaleChild");
               searchPage.FindPathwayInCathgryList("Stroke symptoms", "PA171MaleChild");
               searchPage.FindPathwayInCathgryList("Tiredness", "PW1073MaleChild");
               searchPage.FindPathwayInCathgryList("Fatigue", "PW1073MaleChild");
               searchPage.FindPathwayInCathgryList("Toe injury - skin not broken", "PW1282MaleChild");
               searchPage.FindPathwayInCathgryList("Toe injury with broken skin", "PW1526MaleChild");
               searchPage.FindPathwayInCathgryList("Toenail injury", "PW1593MaleChild");
               searchPage.FindPathwayInCathgryList("Toothache after an injury", "PW572MaleChild");
               searchPage.FindPathwayInCathgryList("Blisters", "PW1625MaleChild");
               searchPage.FindPathwayInCathgryList("Trembling", "PW1764MaleChild");
               searchPage.FindPathwayInCathgryList("Shaking", "PW1764MaleChild");
               searchPage.FindPathwayInCathgryList("Vomiting and nausea - no diarrhoea", "PW940MaleChild");
               searchPage.FindPathwayInCathgryList("Wound problems", "PW1776MaleChild");
               searchPage.FindPathwayInCathgryList("Wound problems - plaster casts", "PW1709MaleChild");
               searchPage.FindPathwayInCathgryList("Wound problems - drainage tubes", "PW1709MaleChild");
               searchPage.FindPathwayInCathgryList("Wound problems - metal attachments", "PW1709MaleChild");
               searchPage.FindPathwayInCathgryList("Swollen finger", "PW1617MaleChild");
               searchPage.FindPathwayInCathgryList("Painful finger", "PW1617MaleChild");
               searchPage.FindPathwayInCathgryList("Swollen hand", "PW1617MaleChild");
               searchPage.FindPathwayInCathgryList("Painful hand", "PW1617MaleChild");
               searchPage.FindPathwayInCathgryList("Swollen wrist", "PW1617MaleChild");
               searchPage.FindPathwayInCathgryList("Painful wrist", "PW1617MaleChild");
               searchPage.FindPathwayInCathgryList("Self-harm", "PW1543MaleChild");

               searchPage.SelectCategory("bowel-and-urinary-problems");
               searchPage.SelectCategory("bowel-and-urinary-problems-bowel-problems");
               searchPage.SelectPathway("Something in the bottom/back passage");

               _driver.FindElement(By.XPath("//input[@value = 'PW1531MaleChild']"));

                          }

           [Test]
           public void CategoriespresentForMaleAdult()

                   //Test to show all categories are present per age and gender, and also the correct pathway opens per age and gender, when the same pathway is used for all ages/genders.
           {

               var searchPage = TestScenerios.LaunchSearchScenerio(_driver, TestScenerioGender.Male, 40);

               searchPage.ClickGoButton();

               searchPage.FindPathwayInCathgryList("Abdomen injury - skin not broken","PW503MaleAdult"); 
               searchPage.FindPathwayInCathgryList("Injury to the side of the body - skin not broken","PW503MaleAdult");
               searchPage.FindPathwayInCathgryList("Abdomen injury with broken skin","PW511MaleAdult");
               searchPage.FindPathwayInCathgryList("Injury to the side of the body with broken skin","PW511MaleAdult");
               searchPage.FindPathwayInCathgryList("Abdominal pain","PW519MaleAdult");
               searchPage.FindPathwayInCathgryList("Stomach pain","PW519MaleAdult");
               searchPage.FindPathwayInCathgryList("Accidental poisoning","PW881MaleAdult");
               searchPage.FindPathwayInCathgryList("Breathed in something poisonous","PW881MaleAdult");
               searchPage.FindPathwayInCathgryList("Swallowed something poisonous","PW881MaleAdult");
               searchPage.FindPathwayInCathgryList("Drunk too much alcohol","PW1551MaleAdult");
               searchPage.FindPathwayInCathgryList("Alcohol intoxication","PW1551MaleAdult");
               searchPage.FindPathwayInCathgryList("Ankle injury - skin not broken","PW1512MaleAdult");
               searchPage.FindPathwayInCathgryList("Foot injury - skin not broken","PW1512MaleAdult");
               searchPage.FindPathwayInCathgryList("Ankle injury with broken skin","PW1518MaleAdult");
               searchPage.FindPathwayInCathgryList("Foot injury with broken skin","PW1518MaleAdult");
               searchPage.FindPathwayInCathgryList("Arm injury - skin not broken","PW897MaleAdult");
               searchPage.FindPathwayInCathgryList("Arm injury with broken skin","PW902MaleAdult");
               searchPage.FindPathwayInCathgryList("Painful arm","PW1166MaleAdult");
               searchPage.FindPathwayInCathgryList("Swollen arm","PW1166MaleAdult");
               searchPage.FindPathwayInCathgryList("Cold arm","PW1733MaleAdult");
               searchPage.FindPathwayInCathgryList("Arm changing colour","PW1733MaleAdult");
               searchPage.FindPathwayInCathgryList("Change in behaviour","PW1749MaleAdult");
               searchPage.FindPathwayInCathgryList("Bites and stings","PW1575MaleAdult");
               searchPage.FindPathwayInCathgryList("Bleeding from stoma","PT507MaleAdult");
               searchPage.FindPathwayInCathgryList("Blood in urine","PW962MaleAdult");
               searchPage.FindPathwayInCathgryList("Breast problems","PW1604MaleAdult");
               searchPage.FindPathwayInCathgryList("Breathing problems","PW559MaleAdult");
               searchPage.FindPathwayInCathgryList("Upper back pain","PW559MaleAdult");
               searchPage.FindPathwayInCathgryList("Chest and upper back pain","PW559MaleAdult");
               searchPage.FindPathwayInCathgryList("Chest pain","PW559MaleAdult");
               searchPage.FindPathwayInCathgryList("Wheezing","PW559MaleAdult");
               searchPage.FindPathwayInCathgryList("Coughing up blood","PW1653MaleAdult");
               searchPage.FindPathwayInCathgryList("Bringing up blood","PW1653MaleAdult");
               searchPage.FindPathwayInCathgryList("Burns from chemicals","PW564MaleAdult");
               searchPage.FindPathwayInCathgryList("Sunburn","PW987MaleAdult");
               searchPage.FindPathwayInCathgryList("Burns from heat","PW580MaleAdult");
               searchPage.FindPathwayInCathgryList("Catheter problems","PW1567MaleAdult");
               searchPage.FindPathwayInCathgryList("Chest injury - skin not broken","PW588MaleAdult");
               searchPage.FindPathwayInCathgryList("Upper back injury - skin not broken","PW588MaleAdult");
               searchPage.FindPathwayInCathgryList("Chest injury with broken skin","PW596MaleAdult");
               searchPage.FindPathwayInCathgryList("Upper back injury with broken skin","PW596MaleAdult");
               searchPage.FindPathwayInCathgryList("Colds and flu","PW1042MaleAdult");
               searchPage.FindPathwayInCathgryList("Constipation","PW1159MaleAdult");
               searchPage.FindPathwayInCathgryList("Cough","PW976MaleAdult");
               searchPage.FindPathwayInCathgryList("Self-harm","PW1543MaleAdult");
               searchPage.FindPathwayInCathgryList("Dental injury","PW620MaleAdult");
               searchPage.FindPathwayInCathgryList("Toothache and other dental problems","PW1610MaleAdult");
               searchPage.FindPathwayInCathgryList("Diabetes - blood sugar problems","PW1746MaleAdult");
               searchPage.FindPathwayInCathgryList("Diarrhoea - no vomiting","PW631MaleAdult");
               searchPage.FindPathwayInCathgryList("Diarrhoea and vomiting","PW1555MaleAdult");
               searchPage.FindPathwayInCathgryList("Difficulty passing urine","PW886MaleAdult");
               searchPage.FindPathwayInCathgryList("Difficulty swallowing","PW1496MaleAdult");
               searchPage.FindPathwayInCathgryList("Dizziness","PW639MaleAdult");
               searchPage.FindPathwayInCathgryList("Vertigo","PW639MaleAdult");
               searchPage.FindPathwayInCathgryList("Ear discharge","PW1702MaleAdult");
               searchPage.FindPathwayInCathgryList("Earwax","PW1702MaleAdult");
               searchPage.FindPathwayInCathgryList("Earache","PW655MaleAdult");
               searchPage.FindPathwayInCathgryList("Eye injury - no damage to surface of eye","PW660MaleAdult");
               searchPage.FindPathwayInCathgryList("Eye injury with damage to surface of eye","PW668MaleAdult");
               searchPage.FindPathwayInCathgryList("Eye problems","PW1629MaleAdult");
               searchPage.FindPathwayInCathgryList("Eyelid problems","PW1629MaleAdult");
               searchPage.FindPathwayInCathgryList("Something in the eye","PW1098MaleAdult");
               searchPage.FindPathwayInCathgryList("Swollen face","PW1545MaleAdult");
               searchPage.FindPathwayInCathgryList("Painful face","PW1545MaleAdult");
               searchPage.FindPathwayInCathgryList("Swollen neck","PW1545MaleAdult");
               searchPage.FindPathwayInCathgryList("Painful neck","PW1545MaleAdult");
               searchPage.FindPathwayInCathgryList("Falling","PW700MaleAdult");
               searchPage.FindPathwayInCathgryList("Fainting","PW700MaleAdult");
               searchPage.FindPathwayInCathgryList("Passing out","PW700MaleAdult");
               searchPage.FindPathwayInCathgryList("Fever","PW711MaleAdult");
               searchPage.FindPathwayInCathgryList("High temperature","PW711MaleAdult");
               searchPage.FindPathwayInCathgryList("Finger injury - skin not broken","PW1270MaleAdult");
               searchPage.FindPathwayInCathgryList("Thumb injury - skin not broken","PW1270MaleAdult");
               searchPage.FindPathwayInCathgryList("Finger injury with broken skin","PW1264MaleAdult");
               searchPage.FindPathwayInCathgryList("Thumb injury with broken skin","PW1264MaleAdult");
               searchPage.FindPathwayInCathgryList("Fingernail injury","PW1570MaleAdult");
               searchPage.FindPathwayInCathgryList("Pain in the side of the body","PW719MaleAdult");
               searchPage.FindPathwayInCathgryList("Something in the ear","PW1528MaleAdult");
               searchPage.FindPathwayInCathgryList("Something in the nose","PW1529MaleAdult");
               searchPage.FindPathwayInCathgryList("Something in the penis","PW1530MaleAdult");
               searchPage.FindPathwayInCathgryList("Something in the bottom/back passage","PW1531MaleAdult");
               searchPage.FindPathwayInCathgryList("Genital injury - skin not broken","PW1118MaleAdult");
               searchPage.FindPathwayInCathgryList("Genital injury with broken skin","PW1012MaleAdult");
               searchPage.FindPathwayInCathgryList("Genital problems","PW1564MaleAdult");
               searchPage.FindPathwayInCathgryList("Swollen groin","PW731MaleAdult");
               searchPage.FindPathwayInCathgryList("Painful groin","PW731MaleAdult");
               searchPage.FindPathwayInCathgryList("Hair loss","PW1678MaleAdult");
               searchPage.FindPathwayInCathgryList("Hand injury - skin not broken","PW1267MaleAdult");
               searchPage.FindPathwayInCathgryList("Wrist injury - skin not broken","PW1267MaleAdult");
               searchPage.FindPathwayInCathgryList("Hand injury with broken skin","PW1260MaleAdult");
               searchPage.FindPathwayInCathgryList("Wrist injury with broken skin","PW1260MaleAdult");
               searchPage.FindPathwayInCathgryList("Head injury - skin not broken","PW684MaleAdult");
               searchPage.FindPathwayInCathgryList("Face injury - skin not broken","PW684MaleAdult");
               searchPage.FindPathwayInCathgryList("Neck injury - skin not broken","PW684MaleAdult");
               searchPage.FindPathwayInCathgryList("Head injury with broken skin","PW692MaleAdult");
               searchPage.FindPathwayInCathgryList("Face injury with broken skin","PW692MaleAdult");
               searchPage.FindPathwayInCathgryList("Neck injury with broken skin","PW692MaleAdult");
               searchPage.FindPathwayInCathgryList("Headache","PW755MaleAdult");
               searchPage.FindPathwayInCathgryList("Hearing problems","PW1762MaleAdult");
               searchPage.FindPathwayInCathgryList("Blocked ear","PW1762MaleAdult");
               searchPage.FindPathwayInCathgryList("Heatstroke","PW998MaleAdult");
               searchPage.FindPathwayInCathgryList("Heat exhaustion","PW998MaleAdult");
               searchPage.FindPathwayInCathgryList("Hiccups","PW1775MaleAdult");
               searchPage.FindPathwayInCathgryList("Leg injury - skin not broken","PW1241MaleAdult");
               searchPage.FindPathwayInCathgryList("Leg injury with broken skin","PW1234MaleAdult");
               searchPage.FindPathwayInCathgryList("Leg changing colour","PW1734MaleAdult");
               searchPage.FindPathwayInCathgryList("Cold leg","PW1734MaleAdult");
               searchPage.FindPathwayInCathgryList("Locked jaw","PW1712MaleAdult");
               searchPage.FindPathwayInCathgryList("Loss of bowel control","PW1759MaleAdult");
               searchPage.FindPathwayInCathgryList("Bowel incontinence","PW1759MaleAdult");
               searchPage.FindPathwayInCathgryList("Lower back injury - skin not broken","PW793MaleAdult");
               searchPage.FindPathwayInCathgryList("Lower back injury with broken skin","PW801MaleAdult");
               searchPage.FindPathwayInCathgryList("Lower back pain","PW785MaleAdult");
               searchPage.FindPathwayInCathgryList("Painful legs","PW777MaleAdult");
               searchPage.FindPathwayInCathgryList("Swollen legs","PW777MaleAdult");
               searchPage.FindPathwayInCathgryList("Mental health problems","PW1751MaleAdult");
               searchPage.FindPathwayInCathgryList("Low mood","PW1751MaleAdult");
               searchPage.FindPathwayInCathgryList("Depression","PW1751MaleAdult");
               searchPage.FindPathwayInCathgryList("Anxiety","PW1751MaleAdult");
               searchPage.FindPathwayInCathgryList("Mouth ulcers","PW1323MaleAdult");
               searchPage.FindPathwayInCathgryList("Blocked nose","PW981MaleAdult");
               searchPage.FindPathwayInCathgryList("Nosebleed after an injury","PW1713MaleAdult");
               searchPage.FindPathwayInCathgryList("Nosebleed","PW818MaleAdult");
               searchPage.FindPathwayInCathgryList("Numbness","PW1683MaleAdult");
               searchPage.FindPathwayInCathgryList("Tingling","PW1683MaleAdult");
               searchPage.FindPathwayInCathgryList("Swallowed an object","PW1034MaleAdult");
               searchPage.FindPathwayInCathgryList("Breathed in an object","PW1034MaleAdult");
               searchPage.FindPathwayInCathgryList("Other symptoms","PW1346MaleAdult");
               searchPage.FindPathwayInCathgryList("Palpitations","PW1030MaleAdult");
               searchPage.FindPathwayInCathgryList("Pounding heart","PW1030MaleAdult");
               searchPage.FindPathwayInCathgryList("Fluttering heart","PW1030MaleAdult");
               searchPage.FindPathwayInCathgryList("Urinary problems","PW647MaleAdult");
               searchPage.FindPathwayInCathgryList("Bleeding from the bottom/back passage","PW846MaleAdult");
               searchPage.FindPathwayInCathgryList("Swollen bottom/back passage","PW1091MaleAdult");
               searchPage.FindPathwayInCathgryList("Painful bottom/back passage","PW1091MaleAdult");
               searchPage.FindPathwayInCathgryList("Itchy bottom/back passage","PW1091MaleAdult");
               searchPage.FindPathwayInCathgryList("Grazes","PW1590MaleAdult");
               searchPage.FindPathwayInCathgryList("Minor wounds","PW1590MaleAdult");
               searchPage.FindPathwayInCathgryList("Scratches","PW1590MaleAdult");
               searchPage.FindPathwayInCathgryList("Sexual concerns - male","PW1685MaleAdult");
               searchPage.FindPathwayInCathgryList("Shoulder pain","PW1140MaleAdult");
               searchPage.FindPathwayInCathgryList("Sinusitis","PW1048MaleAdult");
               searchPage.FindPathwayInCathgryList("Rashes, itching, spots, moles and other skin problems","PW1771MaleAdult");
               searchPage.FindPathwayInCathgryList("Glue on the skin","PW1301MaleAdult");
               searchPage.FindPathwayInCathgryList("Something under the skin","PW1754MaleAdult");
               searchPage.FindPathwayInCathgryList("Splinter","PW1754MaleAdult");
               searchPage.FindPathwayInCathgryList("Sleep problems","PW1686MaleAdult");
               searchPage.FindPathwayInCathgryList("Sore throat","PW854MaleAdult");
               searchPage.FindPathwayInCathgryList("Hoarse voice","PW854MaleAdult");
               searchPage.FindPathwayInCathgryList("Stoma problems","PW1719MaleAdult");
               searchPage.FindPathwayInCathgryList("Tiredness","PW1072MaleAdult");
               searchPage.FindPathwayInCathgryList("Fatigue","PW1072MaleAdult");
               searchPage.FindPathwayInCathgryList("Toe injury - skin not broken","PW1282MaleAdult");
               searchPage.FindPathwayInCathgryList("Toe injury with broken skin","PW1526MaleAdult");
               searchPage.FindPathwayInCathgryList("Toenail injury","PW1593MaleAdult");
               searchPage.FindPathwayInCathgryList("Toothache after an injury","PW572MaleAdult");
               searchPage.FindPathwayInCathgryList("Blisters","PW1625MaleAdult");
               searchPage.FindPathwayInCathgryList("Trembling","PW1764MaleAdult");
               searchPage.FindPathwayInCathgryList("Shaking","PW1764MaleAdult");
               searchPage.FindPathwayInCathgryList("Vomiting and nausea - no diarrhoea","PW939MaleAdult");
               searchPage.FindPathwayInCathgryList("Wound problems","PW1776MaleAdult");
               searchPage.FindPathwayInCathgryList("Wound problems - plaster casts","PW1709MaleAdult");
               searchPage.FindPathwayInCathgryList("Wound problems - drainage tubes","PW1709MaleAdult");
               searchPage.FindPathwayInCathgryList("Wound problems - metal attachments","PW1709MaleAdult");
               searchPage.FindPathwayInCathgryList("Swollen finger","PW1615MaleAdult");
               searchPage.FindPathwayInCathgryList("Painful finger","PW1615MaleAdult");
               searchPage.FindPathwayInCathgryList("Swollen hand","PW1615MaleAdult");
               searchPage.FindPathwayInCathgryList("Painful hand","PW1615MaleAdult");
               searchPage.FindPathwayInCathgryList("Swollen wrist","PW1615MaleAdult");
               searchPage.FindPathwayInCathgryList("Painful wrist","PW1615MaleAdult");
               searchPage.FindPathwayInCathgryList("Self-harm","PW1543MaleAdult");

               searchPage.SelectCategory("bowel-and-urinary-problems");
               searchPage.SelectCategory("bowel-and-urinary-problems-bowel-problems");
               searchPage.SelectPathway("Something in the bottom/back passage");
                       
               _driver.FindElement(By.XPath("//input[@value = 'PW1531MaleAdult']"));

           }

    }
}

