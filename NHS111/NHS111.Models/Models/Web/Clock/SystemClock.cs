using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NHS111.Models.Models.Web.Clock
{
    public class SystemClock : IClock
    {
        public DateTime Now { get { return DateTime.Now; } }
    }
}
