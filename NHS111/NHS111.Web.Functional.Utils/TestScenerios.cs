using OpenQA.Selenium;

namespace NHS111.Web.Functional.Utils
{
    public static class TestScenerioSex
    {
        public const string Male = "Male";
        public const string Female = "Female";
    }

    public static class TestScenerioAgeGroups
    {
        public const int Adult = 22;
        public const int Adolescent = 16;
        public const int Child = 8;
        public const int Toddler = 2;
        public const int Infant = 0;
    }

    public static class TestScenerios
    {
        public static SearchPage LaunchSearchScenerio(IWebDriver driver, string sex, int age)
        {
            var homePage = TestScenarioPart.HomePage(driver);
            var locationPage = TestScenarioPart.Location(homePage);
            var moduleZeroPage = TestScenarioPart.ModuleZero(locationPage);
            var demographicsPage = TestScenarioPart.Demographics(moduleZeroPage);
            return TestScenarioPart.Search(demographicsPage, sex, age);
        }

        public static SearchPage LaunchSearchScenerio(IWebDriver driver, string sex, int age, string postcode)
        {
            var homePage = TestScenarioPart.HomePage(driver);
            var locationPage = TestScenarioPart.Location(homePage);
            var moduleZeroPage = TestScenarioPart.ModuleZero(locationPage, postcode);
            var demographicsPage = TestScenarioPart.Demographics(moduleZeroPage);
            return TestScenarioPart.Search(demographicsPage, sex, age);
        }

        public static CategoryPage LaunchCategoryScenerio(IWebDriver driver, string sex, int age)
        {
            var homePage = TestScenarioPart.HomePage(driver);
            var locationPage = TestScenarioPart.Location(homePage);
            var moduleZeroPage = TestScenarioPart.ModuleZero(locationPage);
            var demographicsPage = TestScenarioPart.Demographics(moduleZeroPage);
            var searchPage = TestScenarioPart.Search(demographicsPage, sex, age);
            return TestScenarioPart.Category(searchPage);
        }

        public static CategoryPage LaunchCategoryScenerio(IWebDriver driver, string sex, int age, string postcode)
        {
            var homePage = TestScenarioPart.HomePage(driver);
            var locationPage = TestScenarioPart.Location(homePage);
            var moduleZeroPage = TestScenarioPart.ModuleZero(locationPage, postcode);
            var demographicsPage = TestScenarioPart.Demographics(moduleZeroPage);
            var searchPage = TestScenarioPart.Search(demographicsPage, sex, age);
            return TestScenarioPart.Category(searchPage);
        }

        public static QuestionPage LaunchTriageScenerio(IWebDriver driver, string pathwayTopic, string sex, int age)
        {
            var homePage = TestScenarioPart.HomePage(driver);
            var locationPage = TestScenarioPart.Location(homePage);
            var moduleZeroPage = TestScenarioPart.ModuleZero(locationPage);
            var demographicsPage = TestScenarioPart.Demographics(moduleZeroPage);
            var searchPage = TestScenarioPart.Search(demographicsPage, sex, age);
            var questionInfoPage = TestScenarioPart.QuestionInfo(searchPage, pathwayTopic);
            return TestScenarioPart.Question(questionInfoPage);
        }

        public static QuestionPage LaunchTriageScenerio(IWebDriver driver, string pathwayTopic, string sex, int age, string postcode)
        {
            var homePage = TestScenarioPart.HomePage(driver);
            var locationPage = TestScenarioPart.Location(homePage);
            var moduleZeroPage = TestScenarioPart.ModuleZero(locationPage, postcode);
            var demographicsPage = TestScenarioPart.Demographics(moduleZeroPage);
            var searchPage = TestScenarioPart.Search(demographicsPage, sex, age);
            var questionInfoPage = TestScenarioPart.QuestionInfo(searchPage, pathwayTopic);
            return TestScenarioPart.Question(questionInfoPage);
        }

        public static QuestionPage LaunchTriageScenerio(IWebDriver driver, string pathwayTopic, string sex, int age, string postcode, string pathwayTitle)
        {
            var homePage = TestScenarioPart.HomePage(driver);
            var locationPage = TestScenarioPart.Location(homePage);
            var moduleZeroPage = TestScenarioPart.ModuleZero(locationPage, postcode);
            var demographicsPage = TestScenarioPart.Demographics(moduleZeroPage);
            var searchPage = TestScenarioPart.Search(demographicsPage, sex, age);
            var questionInfoPage = TestScenarioPart.QuestionInfo(searchPage, pathwayTopic, pathwayTitle);
            return TestScenarioPart.Question(questionInfoPage);
        }

        public static PersonalDetailsPage LaunchPersonalDetailsScenario(IWebDriver driver, string pathwayTopic, string sex, int age, string postcode)
        {
            var homePage = TestScenarioPart.HomePage(driver);
            var locationPage = TestScenarioPart.Location(homePage);
            var moduleZeroPage = TestScenarioPart.ModuleZero(locationPage, postcode);
            var demographicsPage = TestScenarioPart.Demographics(moduleZeroPage);
            var searchPage = TestScenarioPart.Search(demographicsPage, sex, age);
            var questionInfoPage = TestScenarioPart.QuestionInfo(searchPage, pathwayTopic);
            var questionPage = TestScenarioPart.Question(questionInfoPage);
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

            return outcomePage.ClickBookCallback();
        }

        public static GetTextMessagesPage LaunchCovid19SmsRegistration(IWebDriver driver, string covid19SMSRegistrationUrl)
        {
            var homePage = TestScenarioPart.HomePage(driver);
            var startRegistrationPage = TestScenarioPart.GetTextMessagesPage(driver, covid19SMSRegistrationUrl);
            return startRegistrationPage;
        }

        public static QuestionPage LaunchDeeplinkScenerio(IWebDriver driver, string sex, int age, string postcode)
        {
            var homePage = TestScenarioPart.HomePage(driver);
            var locationPage = TestScenarioPart.EPDeeplink(homePage);
            var moduleZeroPage = TestScenarioPart.ModuleZero(locationPage, postcode);
            var demographicsPage = TestScenarioPart.Demographics(moduleZeroPage);
            var questionInfoPage = TestScenarioPart.QuestionInfo(demographicsPage, sex, age);
            return TestScenarioPart.Question(questionInfoPage);
        }

        public static QuestionPage LaunchRecommendedServiceScenerio(IWebDriver driver, string pathwayTopic, string sex, int age, string postcode)
        {
            var homePage = TestScenarioPart.HomePage(driver);
            var locationPage = TestScenarioPart.Location(homePage);
            var moduleZeroPage = TestScenarioPart.ModuleZero(locationPage, postcode);
            var demographicsPage = TestScenarioPart.Demographics(moduleZeroPage);
            var searchPage = TestScenarioPart.Search(demographicsPage, sex, age);
            var questionInfoPage = TestScenarioPart.QuestionInfo(searchPage, pathwayTopic);
            return TestScenarioPart.Question(questionInfoPage);
        }
    }
}

