using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NHS111.SmokeTest.Utils;
using NUnit.Framework;
using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;

namespace NHS111.SmokeTests
{
    [TestFixture]
    public class SearchTermScenerios
    {
        private IWebDriver _driver;
        private List<Tuple<string, string>> _testTerms;

        [TestFixtureSetUp]
        public void InitTests()
        {
            _driver = new ChromeDriver();
            _testTerms = new List<Tuple<string, string>>()
            {
                new Tuple<string, string>("tummy pain","Abdominal Pain"),
                new Tuple<string, string>("pain in my tooth","Dental Problems"),
                new Tuple<string, string>("diorea","Diarrhoea and vomiting")
            };
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
        public void SearchTermYieldsHits()
        {
            var searchPage = TestScenerios.LaunchSearchScenerio(_driver, TestScenerioGender.Male, 33);

            foreach (var testTerm in _testTerms)
            {
                searchPage.SearchByTerm(testTerm.Item1);
                searchPage.VerifyTermHits(testTerm.Item2, 3);
            } 
        }


    }
}
