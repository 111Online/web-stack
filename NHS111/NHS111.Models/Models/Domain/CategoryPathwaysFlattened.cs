using System.Collections.Generic;

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
