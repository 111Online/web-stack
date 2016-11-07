using System.Collections.Generic;

namespace NHS111.Models.Models.Domain
{
    public class PathwayWithDescriptions
    {
        public Pathway Pathway { get; set; }

        public IEnumerable<PathwayMetaData> PathwayDescriptions { get; set; }
    }
}
