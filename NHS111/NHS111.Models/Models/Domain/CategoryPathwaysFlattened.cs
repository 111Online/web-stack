using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NHS111.Models.Models.Domain
{
    public class CategoryPathwaysFlattened
    {
        public Category Category { get; set; }

        public IEnumerable<Pathway> Pathways { get; set; }

        public IEnumerable<PathwayMetaData> PathwaysMetaData { get; set; }

        public Category SubCategory { get; set; }

        public IEnumerable<Pathway> SubCategoryPathways { get; set; }

        public IEnumerable<PathwayMetaData> SubCategoryPathwaysMetaData { get; set; }
    }
}
