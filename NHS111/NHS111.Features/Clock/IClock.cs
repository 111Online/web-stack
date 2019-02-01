using System;

namespace NHS111.Features.Clock
{
    public interface IClock
    {
        DateTime Now { get; }
    }
}
