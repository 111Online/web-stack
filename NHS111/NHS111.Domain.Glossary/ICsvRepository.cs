using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CsvHelper.Configuration;

namespace NHS111.Domain.Glossary
{
    public interface ICsvRepository<M> where M : CsvClassMap
    {
        IEnumerable<T> List<T>();
    }
}
