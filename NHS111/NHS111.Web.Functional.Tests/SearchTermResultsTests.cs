﻿using NHS111.Web.Functional.Utils;
using NUnit.Framework;

namespace NHS111.Web.Functional.Tests
{
    [TestFixture]
    public class SearchTermResultsTests : BaseTests
    {
        [TestCase("tummy pain", "Stomach Pain")] //abdo pain
        [TestCase("pain in my tooth", "Toothache, swelling and other dental problems")] //dental
        [TestCase("diorea", "Diarrhoea and vomiting")] // mispelling
        [TestCase("stomach ache", "Stomach Pain")] //synonym
        [TestCase("Head ache", "Headache or migraine")] //synonym
        [TestCase("diarhoea", "Diarrhoea")] //misspelling not on list
        [TestCase("vomitting", "Vomiting or nausea")] //misspelling
        [TestCase("toothache", "Toothache, swelling and other dental problems")] //Appears in digital title only not description
        [TestCase("swallowing", "Difficulty swallowing")] //Appears in description only not title
        [TestCase("Choking", "Swallowed or breathed in an object")] //Appears in digital description only
        [TestCase("Chest and upper back pain", "Chest pain")] //additional digital title for Chest pain PW559 MaleAdult
        [TestCase("Breathing problems", "Breathing problems")] //additional digital title for Chest pain PW559 MaleAdult
        //following content updates to improve search for bleeding, pregnancy and asthma
        [TestCase("Wheezing", "Breathing problems")]
        [TestCase("Bleeding", "Bleeding from the bottom")]
        [TestCase("Bleeding", "Toothache, swelling and other dental problems")]
        [TestCase("Bleeding", "Nosebleed")]
        [TestCase("asthma", "Breathing problems")]
        public void SearchTermResults_CommonTermsReturnExpectedResult(string term, string result)
        {
            var searchPage = TestScenerios.LaunchSearchScenerio(Driver, TestScenerioSex.Male, 33);
            searchPage.SearchByTerm(term);
            searchPage.VerifyTermHits(result, 5);
        }
    }
}