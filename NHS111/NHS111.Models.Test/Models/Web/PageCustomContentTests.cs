using System;
using System.Collections.Generic;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using NHS111.Models.Models.Web;
using NUnit.Framework;
using Assert = Microsoft.VisualStudio.TestTools.UnitTesting.Assert;

namespace NHS111.Models.Test.Models.Web
{
    [TestFixture]
    public class PageCustomContentTests
    {
        private const string NonCovidContent = "does not contain covid content placeholder";
        private const string CovidContent = "contain covid #{CovidContent}# content placeholder";

        [Test]
        public void PageContentDoesNotContainCovidPlaceholder()
        {
            var model = new QuestionViewModel { Content = NonCovidContent };
            var s = PageCustomContent.ReplaceCovidPlaceHolderInPageContent(model, new List<string>());
            Assert.AreEqual(NonCovidContent, s);
        }

        [Test]
        public void PageContentContainCovidPlaceholderViaGuidedSelection()
        {
            var model = new QuestionViewModel { Content = CovidContent, ViaGuidedSelection = true };
            var s = PageCustomContent.ReplaceCovidPlaceHolderInPageContent(model, new List<string>());
            Assert.AreEqual(CovidContent.Replace(PageCustomContent.CovidPlaceHolder.PlaceHolder, string.Empty), s);
        }

        [Test]
        public void PageContentContainCovidPlaceholderNotViaGuidedSelectionOrCovidPathwayEmptyList()
        {
            var model = new QuestionViewModel { Content = CovidContent, ViaGuidedSelection = false, PathwayNo = "PW123" };
            var s = PageCustomContent.ReplaceCovidPlaceHolderInPageContent(model, new List<string>());
            Assert.AreEqual(CovidContent.Replace(PageCustomContent.CovidPlaceHolder.PlaceHolder, string.Empty), s);
        }

        [Test]
        public void PageContentContainCovidPlaceholderNotViaGuidedSelectionOrCovidPathway()
        {
            var model = new QuestionViewModel { Content = CovidContent, ViaGuidedSelection = false, PathwayNo = "PW123" };
            var s = PageCustomContent.ReplaceCovidPlaceHolderInPageContent(model, new List<string>() { "PW456" });
            Assert.AreEqual(CovidContent.Replace(PageCustomContent.CovidPlaceHolder.PlaceHolder, string.Empty), s);
        }

        [Test]
        public void PageContentContainCovidPlaceholderNotViaGuidedSelectionCovidPathway()
        {
            var model = new QuestionViewModel { Content = CovidContent, ViaGuidedSelection = false, PathwayNo = "PW456" };
            var s = PageCustomContent.ReplaceCovidPlaceHolderInPageContent(model, new List<string>() { "PW456", "PW789" });
            Assert.AreEqual(CovidContent.Replace(PageCustomContent.CovidPlaceHolder.PlaceHolder, PageCustomContent.CovidPlaceHolder.Content), s);
        }
    }
}
