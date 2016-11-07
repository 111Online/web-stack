using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NHS111.Models.Models.Domain
{
    public class CategoryWithPathways
    {
        public Category Category { get; set; }

        public List<Pathway> Pathways { get; set; }
    }
}
