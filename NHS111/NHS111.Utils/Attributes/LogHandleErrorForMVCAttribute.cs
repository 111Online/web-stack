using NHS111.Utils.Logging;
using System.Web.Mvc;

namespace NHS111.Utils.Attributes
{
    using System;
    using System.Collections;

    public class LogHandleErrorForMVCAttribute : HandleErrorAttribute
    {
        public override void OnException(ExceptionContext filterContext)
        {
            var controllerAction = string.Empty;
            if (filterContext.RouteData != null && filterContext.RouteData != null & filterContext.RouteData.Values != null)
            {
                controllerAction = string.Join("/", filterContext.RouteData.Values.Values);
            }

            string data = "";
            foreach (DictionaryEntry pair in filterContext.Exception.Data)
            {
                data += string.Format("{0}: {1}{2}", pair.Key, pair.Value, Environment.NewLine);
            }

            Log4Net.Error(string.Format("{0} occured executing '{1}'{5}{2}{5}{3}{5}Exception.Data:{5}{4}", filterContext.Exception.GetType().FullName, controllerAction, filterContext.Exception.Message, filterContext.Exception.StackTrace, data, Environment.NewLine));
        }
    }
}
