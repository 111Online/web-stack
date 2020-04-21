using System;

namespace NHS111.Models.Models.Web.Clock
{
    public interface IClock
    {
        DateTime Now { get; }
    }
}
