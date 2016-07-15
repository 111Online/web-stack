using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CsvHelper;
using CsvHelper.Configuration;
using NHS111.Domain.Glossary.Configuration;

namespace NHS111.Domain.Glossary
{
    public class CsvRepostory<M> : ICsvRepository<M> where M : CsvClassMap
    {

        private IFileAdapter _fileAdapter;


        public CsvRepostory(IFileAdapter fileAdapter)
        {
            _fileAdapter = fileAdapter;
        }

        public IEnumerable<T> List<T>()
        {
            using (var csv = new CsvReader(_fileAdapter.OpenText()))
            {
                csv.Configuration.RegisterClassMap<M>();
                return csv.GetRecords<T>().ToList();
            }
            
        }
    }
}
