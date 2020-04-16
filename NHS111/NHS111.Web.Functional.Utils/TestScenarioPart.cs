using OpenQA.Selenium;

namespace NHS111.Web.Functional.Utils
{
    public static class TestScenarioPart
    {
        public static HomePage HomePage(IWebDriver driver)
        {
            var homepage = new HomePage(driver);
            homepage.Visit();
            return homepage;
        }


        public static HomePage HomePage(IWebDriver driver, string medium)
        {
            var homepage = new HomePage(driver);
            homepage.Visit(medium);
            return homepage;
        }
        
        public static FeedbackSection FeedbackSection(LayoutPage page)
        {
            return new FeedbackSection(page.Driver);
        }
        
        public static DirectLinking DirectLinking(IWebDriver driver, string path)
        {
            var directLink = new DirectLinking(driver);
            directLink.Visit(path);
            return directLink;
        }
      
        public static LocationPage EPDeeplink(HomePage page)
        {
            return page.ClickEPDeeplink();
        }

      
        public static LocationPage Location(HomePage page)
        {
            return page.ClickStart();
        }

        public static ModuleZeroPage ModuleZero(LocationPage page, string postcode = "LS177NZ")
        {
            return page.EnterPostcode(postcode).ClickNext() as ModuleZeroPage;
        }

        public static DemographicsPage Demographics(ModuleZeroPage page)
        {
            return page.ClickNoneApplyButton();
        }
        
        // This should only be used for journeys bypassing search
        public static QuestionInfoPage QuestionInfo(DemographicsPage page, string gender, int age)
        {
            page.SelectSexAndAge(gender, age);

            return page.NextPageDeeplink();
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

        public static GetTextMessagesPage GetTextMessagesPage(IWebDriver driver, string covid19SMSRegistrationUrl)
        {
            driver.Navigate().GoToUrl(covid19SMSRegistrationUrl);
            return new GetTextMessagesPage(driver);
        }
    }
}
