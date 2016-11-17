﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NHS111.Models.Models.Business
{
    public enum ServiceAvailability
    {
        DispositionAndTimeFrameInHours,
        DispositionInHoursTimeFrameOutOfHours,
        DispositionOutOfHoursTimeFrameInHours,
        DispositionAndTimeFrameOutOfHours
    }
}
