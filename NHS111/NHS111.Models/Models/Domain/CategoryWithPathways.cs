using System.Collections.Generic;

namespace NHS111.Models.Models.Domain
{
    public class CategoryWithPathways
    {
        public Category Category { get; set; }

        public IEnumerable<CategoryWithPathways> SubCategories { get; set; }

        public IEnumerable<PathwayWithDescription> Pathways { get; set; }
    }
}
