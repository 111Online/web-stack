using System;

namespace NHS111.Models.Models.Web.Clock
{
    public class SystemClock : IClock
    {
        public DateTime Now { get { return DateTime.Now; } }
    }
}
