using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using StructureMap.Diagnostics.TreeView;

namespace NHS111.Models.Models.Business
{
    public class PublicHoliday
    {
        public string Title { get; set; }
        public DateTime Date { get; set; }
        public string Notes { get; set; }
    }
}
