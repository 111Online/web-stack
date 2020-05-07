using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Mvc;
using System.Web.Mvc.Html;

namespace NHS111.Utils.Extensions
{
    public static class HtmlHelperExtenstions
    {
        public static void RenderPartialWithPrefix(this HtmlHelper helper, string partialViewName, object model, string prefix)
        {
            helper.RenderPartial(partialViewName,
                model,
                new ViewDataDictionary { TemplateInfo = new System.Web.Mvc.TemplateInfo { HtmlFieldPrefix = prefix } });
        }
    }
}
