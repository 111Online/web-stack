using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NHS111.Models.Models.Web;
using NUnit.Framework;

namespace NHS111.Models.Test.Models.Web
{
    [TestFixture]
    public class SearchJourneyViewModelTest
    {
        private SearchJourneyViewModel _searchJourneyViewModel;

        [SetUp]
        public void Init()
        {
            _searchJourneyViewModel = new SearchJourneyViewModel();
        }

        [Test]
        public void SearchTermsAreReserved()
        {
            foreach (var searchTerm in SearchReservedCovidTerms.SearchTerms)
            {
                _searchJourneyViewModel.SanitisedSearchTerm = searchTerm;
                Assert.True(_searchJourneyViewModel.IsReservedCovidSearchTerm);
            }
        }

        [Test]
        public void SearchTermsAreReservedAllCaps()
        {
            foreach (var searchTerm in SearchReservedCovidTerms.SearchTerms)
            {
                _searchJourneyViewModel.SanitisedSearchTerm = searchTerm.ToUpper();
                Assert.True(_searchJourneyViewModel.IsReservedCovidSearchTerm);
            }
        }

        [Test]
        public void EmptySearchIsNotReserved()
        {
            _searchJourneyViewModel.SanitisedSearchTerm = "";
            Assert.False(_searchJourneyViewModel.IsReservedCovidSearchTerm);
        }

        [Test]
        public void NullSearchIsNotReserved()
        {
            Assert.False(_searchJourneyViewModel.IsReservedCovidSearchTerm);
        }

        [Test]
        public void SearchTermIsNotReserved()
        {
            _searchJourneyViewModel.SanitisedSearchTerm = "hellocoviddfsdkj";
            Assert.False(_searchJourneyViewModel.IsReservedCovidSearchTerm);
        }
    }
}
