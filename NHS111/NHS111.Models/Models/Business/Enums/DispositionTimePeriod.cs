using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NHS111.Models.Models.Business.Enums
{
    public enum DispositionTimePeriod
    {
        DispositionAndTimeFrameInHours,
        DispositionInHoursTimeFrameOutOfHours,
        DispositionOutOfHoursTimeFrameInHours,
        DispositionAndTimeFrameOutOfHours,
        DispositionAndTimeFrameOutOfHoursTraversesInHours,
        DispositionOutOfHoursTimeFrameInShoulder,
        DispositionInShoulderTimeFrameOutOfHours,
        DispositionInShoulderTimeFrameInHours,
        DispositionInShoulderTimeFrameInShoulder
    }
}
