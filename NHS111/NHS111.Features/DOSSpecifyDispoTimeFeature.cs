using NHS111.Features.Clock;
using NHS111.Features.Defaults;
using System;
using System.Globalization;
using System.Web;

namespace NHS111.Features
{
    public class DOSSpecifyDispoTimeFeature : BaseFeature, IDOSSpecifyDispoTimeFeature
    {
        private readonly IClock _clock;

        public DOSSpecifyDispoTimeFeature() : this(new SystemClock())
        {
        }

        public DOSSpecifyDispoTimeFeature(IClock clock)
        {
            DefaultIsEnabledSettingStrategy = new DisabledByDefaultSettingStrategy();
            _clock = clock;
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
            var zeroSecondsDateTime = new DateTime(_clock.Now.Year, _clock.Now.Month, _clock.Now.Day, _clock.Now.Hour, _clock.Now.Minute, 0);
            if (parsedDateTime < zeroSecondsDateTime.AddMinutes(-1) || parsedDateTime > zeroSecondsDateTime.AddYears(1)) return _clock.Now;
            return parsedDateTime;
        }

        private readonly string _dosSearchDateTimeKeyname = "dossearchdatetime";
    }


    public interface IDOSSpecifyDispoTimeFeature : IFeature
    {
        bool HasDate(HttpRequestBase request);

        DateTime GetDosSearchDateTime(HttpRequestBase request);
    }

}
