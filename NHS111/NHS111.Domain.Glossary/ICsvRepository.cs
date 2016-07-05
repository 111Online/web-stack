using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NHS111.Domain.Glossary
{
    public interface ICsvRepository
    {
        IEnumerable<T> List<T>(string filename);
    }
}
