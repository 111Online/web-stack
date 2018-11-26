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


        public static HomePage HomePage(IWebDriver driver, string medium)
        {
            var homepage = new HomePage(driver);
            homepage.Load(medium);
            return homepage;
        }


        public static ModuleZeroPage ModuleZero(IWebDriver driver)
        {
            var homepage = new HomePage(driver);
            homepage.Load();
            return new ModuleZeroPage(driver);
        }

        public static ModuleZeroPage ModuleZeroWithArgs(IWebDriver driver, string args)
        {
            var homepage = new HomePage(driver);
            homepage.ArgsQueryString = args;
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
            page.ClickCategoryLink();
            return page.ClickCategoryLink();
        }

        public static QuestionPage Question(QuestionInfoPage page)
        {
            return page.ClickIUnderstand();
        }

        public static QuestionInfoPage QuestionInfo(SearchPage page, string pathwayTopic)
        {
            return page.TypeSearchTextAndSelect(pathwayTopic);
        }

        public static PageNotFound PageNotFound(IWebDriver driver)
        {
            var pageNotFound = new PageNotFound(driver);

            return pageNotFound;
        }
    }
}
