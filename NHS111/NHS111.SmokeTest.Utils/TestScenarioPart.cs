using System;
using OpenQA.Selenium;

namespace NHS111.SmokeTest.Utils
{
    public static class TestScenarioPart
    {
        public static HomePage HomePage(IWebDriver driver)
        {
            var homepage = new HomePage(driver);
            homepage.Load();
            return homepage;
        }

        public static ModuleZeroPage ModuleZero(HomePage page)
        {
            return page.ClickNextButton();
        }


        public static ModuleZeroPage ModuleZero(IWebDriver driver)
        {
            var homepage = new HomePage(driver);
            homepage.Load();
            return new ModuleZeroPage(driver);
        }

        public static DemographicsPage Demographics(ModuleZeroPage page)
        {
            return page.ClickNoneApplyButton();
        }

        public static SearchPage Search(DemographicsPage page, string gender, int age)
        {
            page.SelectSexAndAge(gender, age);

            return page.NextPage();
        }

        public static CategoryPage Category(SearchPage page)
        {
            return page.TypeInvalidSearch();
        }

        public static QuestionPage Question(SearchPage page, string pathwayTopic)
        {
            return page.TypeSearchTextAndSelect(pathwayTopic);
        }
    }
}
