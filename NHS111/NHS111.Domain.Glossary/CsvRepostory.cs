using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CsvHelper;

namespace NHS111.Domain.Glossary
{
    public class CsvRepostory : ICsvRepository
    {

        private IFileAdapter _fileAdapter;

        public CsvRepostory(IFileAdapter fileAdapter)
        {
            _fileAdapter = fileAdapter;
        }

        public IEnumerable<T> List<T>(string filename)
        {
            var csv = new CsvReader(_fileAdapter.OpenText());
            return csv.GetRecords<T>();
        }
    }
}
