using OpenQA.Selenium;

namespace NHS111.SmokeTest.Utils
{
    public static class TestScenerioSex
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
        public static SearchPage LaunchSearchScenerio(IWebDriver driver, string sex, int age)
        {
            var moduleZeroPage = TestScenarioPart.ModuleZero(driver);
            var demographicsPage = TestScenarioPart.Demographics(moduleZeroPage);
            return TestScenarioPart.Search(demographicsPage, sex, age);
        }

        public static CategoryPage LaunchCategoryScenerio(IWebDriver driver, string sex, int age)
        {
            var moduleZeroPage = TestScenarioPart.ModuleZero(driver);
            var demographicsPage = TestScenarioPart.Demographics(moduleZeroPage);
            var searchPage = TestScenarioPart.Search(demographicsPage, sex, age);
            return TestScenarioPart.Category(searchPage);
        }

        public static QuestionPage LaunchTriageScenerio(IWebDriver driver, string pathwayTopic, string sex, int age)
        {
            var moduleZeroPage = TestScenarioPart.ModuleZero(driver);
            var demographicsPage = TestScenarioPart.Demographics(moduleZeroPage);
            var searchPage = TestScenarioPart.Search(demographicsPage, sex, age);
            return TestScenarioPart.Question(searchPage, pathwayTopic);
        }
    }
}

