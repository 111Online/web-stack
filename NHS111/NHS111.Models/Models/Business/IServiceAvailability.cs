using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NHS111.Models.Models.Business
{
    public interface IServiceAvailability
    {
        bool IsOutOfHours { get; }
    }
}
