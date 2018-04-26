using System;
using System.Collections.Generic;
using NHS111.SmokeTest.Utils;
using NUnit.Framework;

namespace NHS111.SmokeTests
{
    [TestFixture]
    public class SearchTermResultsTests : BaseTests
    {
        private readonly List<Tuple<string, string>> _testTerms = new List<Tuple<string, string>>()
        {
            new Tuple<string, string>("tummy pain", "Stomach Pain"), //abdo pain
            new Tuple<string, string>("pain in my tooth", "Toothache, swelling and other dental problems"), //dental
            new Tuple<string, string>("diorea", "Diarrhoea and vomiting"), // mispelling
            new Tuple<string, string>("stomach ache", "Stomach Pain"), //synonym
            new Tuple<string, string>("Head ache", "Headache or migraine"), //synonym
            new Tuple<string, string>("diarhoea", "Diarrhoea"), //misspelling not on list
            new Tuple<string, string>("vomitting", "Vomiting or nausea"), //misspelling
            new Tuple<string, string>("toothache", "Toothache, swelling and other dental problems"), //Appears in digital title only not description
            new Tuple<string, string>("swallowing", "Difficulty swallowing"), //Appears in description only not title
            new Tuple<string, string>("Choking", "Swallowed or breathed in an object"), //Appears in digital description only
            new Tuple<string, string>("Chest and upper back pain", "Breathing problems or chest pain"), //additional digital title for Chest pain PW559 MaleAdult
            new Tuple<string, string>("Breathing problems", "Breathing problems or chest pain"), //additional digital title for Chest pain PW559 MaleAdult
            
            //following content updates to improve search for bleeding, pregnancy and asthma
            new Tuple<string, string>("Wheezing", "Breathing problems or chest pain"),
            new Tuple<string, string>("Bleeding", "Bleeding from the bottom"),
            new Tuple<string, string>("Bleeding", "Toothache, swelling and other dental problems"),
            new Tuple<string, string>("Bleeding", "Nosebleed"),
            new Tuple<string, string>("asthma", "Breathing problems or chest pain")
        };

        [Test]
        public void SearchTermResults_CommonTermsReturnExpectedResult()
        {
            foreach (var testTerm in _testTerms)
            {
                var searchPage = TestScenerios.LaunchSearchScenerio(Driver, TestScenerioSex.Male, 33);
                searchPage.SearchByTerm(testTerm.Item1);
                searchPage.VerifyTermHits(testTerm.Item2, 5);
            } 
        }
    }
}
