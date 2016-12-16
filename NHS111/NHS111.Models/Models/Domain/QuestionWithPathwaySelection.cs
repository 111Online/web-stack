using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NHS111.Models.Models.Domain
{
    public class QuestionWithPathwaySelection
    {
        public Question Question { get; set; }
        public IEnumerable<string> Labels { get; set; }
        public IDictionary<string, string> State { get; set; }
    }
}
