using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using NHS111.Features.Defaults;

namespace NHS111.Features
{
    public class DOSSpecifyDispoTimeFeature : BaseFeature, IDOSSpecifyDispoTimeFeature
    {
        public DOSSpecifyDispoTimeFeature()
        {
            DefaultIsEnabledSettingStrategy = new DisabledByDefaultSettingStrategy();
        }

        public bool HasDate(HttpRequestBase request)
        {
            if (string.IsNullOrEmpty(request.QueryString[_dosSearchDateTimeKeyname])) return false;

            var dateTimestring = request.QueryString[_dosSearchDateTimeKeyname];
            DateTime parsedDateTime;
            if (!DateTime.TryParseExact(dateTimestring, "yyyy-MM-dd HH:mm", null, DateTimeStyles.AssumeLocal, out parsedDateTime))
                throw new ArgumentException(_dosSearchDateTimeKeyname + " cannot be parsed. Date time must be in the format yyyy-MM-dd HH:mm");

            return true;
        }

        public DateTime GetDosSearchDateTime(HttpRequestBase request)
        {
            var dateTimestring = request.QueryString[_dosSearchDateTimeKeyname];
            DateTime parsedDateTime;
            DateTime.TryParseExact(dateTimestring, "yyyy-MM-dd HH:mm", null, DateTimeStyles.AssumeLocal, out parsedDateTime);
            return parsedDateTime;
        }

        private readonly string _dosSearchDateTimeKeyname = "dossearchdatetime";
    }


    public interface IDOSSpecifyDispoTimeFeature
        : IFeature
    {
        bool HasDate(HttpRequestBase request);

        DateTime GetDosSearchDateTime(HttpRequestBase request);
    }

}
