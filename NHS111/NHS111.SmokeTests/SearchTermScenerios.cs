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
                new Tuple<string, string>("tummy pain","Abdominal Pain"), //abdo pain
                new Tuple<string, string>("pain in my tooth","Toothache, swelling and other dental problems"), //dental
                new Tuple<string, string>("diorea","Diarrhoea and vomiting"), // mispelling
                new Tuple<string, string>("stomach ache","Abdominal Pain"), //synonym
                new Tuple<string, string>("Head ache","Headache and migraine"), //synonym
                new Tuple<string, string>("diarhoea","Diarrhoea"), //misspelling not on list
                new Tuple<string, string>("vomitting","Vomiting or nausea"), //misspelling
                new Tuple<string, string>("under","Something under the skin"), //Appears in digital title only not description
                new Tuple<string, string>("swallowing","Difficulty swallowing"), //Appears in description only not title
                new Tuple<string, string>("lumps","Rashes, itchy skin, spots and blisters"), //Appears in digital description only
                new Tuple<string, string>("Chest and upper back pain","Breathing problems"), //additional digital title for Chest pain PW559 MaleAdult
                new Tuple<string, string>("Breathing problems","Breathing problems"), //additional digital title for Chest pain PW559 MaleAdult
                new Tuple<string, string>("Wheezing","Wheezing and breathlessness"), //additional digital title for Chest pain PW559 MaleAdult
                
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
