using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NHS111.Models.Models.Web
{
    public class PageCustomContent
    {
        private string _placeHolder;
        public string PlaceHolder
        {
            get { return string.Format("#{{{0}}}#", _placeHolder); }
            set { _placeHolder = value; }
        }
        public string Content { get; set; }

        public static PageCustomContent CovidPlaceHolder = new PageCustomContent { PlaceHolder = "CovidContent", Content = "<p>Some questions will check for coronavirus as standard</p>" };
        
        public static string ReplaceCovidPlaceHolderInPageContent(QuestionViewModel model, IEnumerable<string> covidPathways)
        {
            var pageContent = model.Content;
            if (!pageContent.Contains(CovidPlaceHolder.PlaceHolder)) return pageContent;

            //if via guided selection then user knows to expect corona
            if ((model.ViaGuidedSelection.HasValue && model.ViaGuidedSelection.Value) || !covidPathways.Contains(model.PathwayNo))
                    return pageContent.Replace(CovidPlaceHolder.PlaceHolder, string.Empty);
            
            //if not via guide selection and pathway is covid add content
            return pageContent.Replace(CovidPlaceHolder.PlaceHolder, CovidPlaceHolder.Content);
        }
    }
}
