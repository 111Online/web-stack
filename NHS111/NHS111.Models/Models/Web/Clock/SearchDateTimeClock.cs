using System;

namespace NHS111.Models.Models.Web.Clock
{
    public class SearchDateTimeClock : IClock
    {
        private readonly DateTime _searchDatetime;

        public SearchDateTimeClock(DateTime searchDatetime)
        {
            _searchDatetime = searchDatetime;
        }

        public DateTime Now
        {
            get { return _searchDatetime; }
        }
    }
}
