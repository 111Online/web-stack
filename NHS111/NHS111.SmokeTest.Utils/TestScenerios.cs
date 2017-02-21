using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using OpenQA.Selenium;

namespace NHS111.SmokeTest.Utils
{
    public static class TestScenerioGender
    {
        public static string Male = "Male";
        public static string Female = "Female";

    }

    public static class TestScenerioAgeGroups
    {
        public static int Adult = 22;
        public static int Child = 8;
        public static int Toddler = 2;
        public static int Infant = 0;
    }

    public static class TestScenerios
    {

        public static SearchPage LaunchSearchScenerio(IWebDriver driver, string gender, int age)
        {
            var homePage = new HomePage(driver);
            homePage.Load();
            homePage.Verify();


            var moduleZeroPage = homePage.ClickNextButton();
            moduleZeroPage.Verify();

            var genderPage = moduleZeroPage.ClickNoneApplyButton();
            genderPage.Verify();
            genderPage.SelectGenderAndAge(gender, age);

            var searchpage = genderPage.NextPage();
            searchpage.Verify();
            return searchpage;

        }

        public static QuestionPage LaunchTriageScenerio(IWebDriver driver, string pathwayTopic, string gender, int age)
        {
            var homePage = new HomePage(driver);
            homePage.Load();
            homePage.Verify();


            var moduleZeroPage = homePage.ClickNextButton();
            moduleZeroPage.Verify();

            var genderPage = moduleZeroPage.ClickNoneApplyButton();
            genderPage.Verify();
            genderPage.SelectGenderAndAge(gender, age);

            var searchpage = genderPage.NextPage();
            searchpage.Verify();
            return searchpage.TypeSearchTextAndSelect(pathwayTopic);
        }
    }
}
