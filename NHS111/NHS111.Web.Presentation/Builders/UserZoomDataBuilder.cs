using NHS111.Models.Models.Web;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;

namespace NHS111.Web.Presentation.Builders
{
    public class UserZoomDataBuilder : IUserZoomDataBuilder
    {
        public void SetFieldsForQuestion(JourneyViewModel model)
        {
            var title = model.TitleWithoutBullets;

            SetUserZoomFields(title, GetQuestionUrl(model), model);
        }

        public void SetFieldsForOutcome(JourneyViewModel model)
        {
            UrlHelper urlHelper = new UrlHelper();

            var outcomeGroup = model.OutcomeGroup == null ? "NoGroup" : urlHelper.Encode(model.OutcomeGroup.Text);
            var url = string.Format("outcome/{0}/{1}/{2}/disposition/", urlHelper.Encode(model.PathwayNo), outcomeGroup, urlHelper.Encode(model.Id));

            SetUserZoomFields(model.Id, url, model);
        }

        public void SetFieldsForSearch(SearchJourneyViewModel model)
        {
            SetUserZoomFields("Search", "Search", model);
        }

        public void SetFieldsForSearchResults(SearchJourneyViewModel model)
        {
            SetUserZoomFields("Search Results for " + model.SanitisedSearchTerm, "SearchResults", model);
        }

        public void SetFieldsForCareAdvice(JourneyViewModel model)
        {
            SetUserZoomFields(model.QuestionNo, GetQuestionUrl(model), model);
        }

        public void SetFieldsForInitialQuestion(JourneyViewModel model)
        {
            SetUserZoomFields("Initial Question", "InitialQuestion", model);
        }

        public void SetFieldsForDemographics(JourneyViewModel model)
        {
            SetUserZoomFields("Demographics", "Demographics", model);
        }

        public void SetFieldsForHome(JourneyViewModel model)
        {
            SetUserZoomFields("Home", "Home", model);
        }

        private static string GetQuestionUrl(JourneyViewModel model)
        {
            UrlHelper urlHelper = new UrlHelper();

            return string.Format("{0}/{1}/{2}/", urlHelper.Encode(model.PathwayId), urlHelper.Encode(model.PathwayTitle), urlHelper.Encode(model.QuestionNo));
        }

        private static void SetUserZoomFields(string title, string url, JourneyViewModel model)
        {
            model.UserZoomTitle = title;
            model.UserZoomUrl = url;
        }
    }

    public interface IUserZoomDataBuilder
    {
        void SetFieldsForQuestion(JourneyViewModel model);
        void SetFieldsForOutcome(JourneyViewModel model);
        void SetFieldsForSearch(SearchJourneyViewModel model);
        void SetFieldsForSearchResults(SearchJourneyViewModel model);
        void SetFieldsForCareAdvice(JourneyViewModel model);
        void SetFieldsForInitialQuestion(JourneyViewModel model);
        void SetFieldsForDemographics(JourneyViewModel model);
        void SetFieldsForHome(JourneyViewModel model);
    }
}
