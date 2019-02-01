using System;

namespace NHS111.Features.Clock
{
    public class SystemClock : IClock
    {
        public DateTime Now { get { return DateTime.Now; } }
    }
}
