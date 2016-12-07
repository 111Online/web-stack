using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NHS111.Utils.Parser
{
    public class CsvParser : ICsvParser
    {
        public CsvParser(string filePath) : this(filePath, false)
        {
        
        }

        public CsvParser(string filePath, bool hasHeader)
        {
            if (!File.Exists(filePath)) throw new FileNotFoundException();

            Lines = hasHeader ? File.ReadLines(filePath).Skip(1) : File.ReadLines(filePath);
        }

        public IEnumerable<string> Lines { get; set; }

        public IEnumerable<string> GetColumnValues(int columnIndex)
        {
            if(columnIndex < 0) throw  new IndexOutOfRangeException();

            var csv = Lines.Select(line => line.Split(',')).ToList();

            if(csv.Any() && Lines.First().Length < columnIndex) throw new IndexOutOfRangeException();

            return csv.Select(c => c[columnIndex]);
        }
    }

    public interface ICsvParser
    {
        IEnumerable<string> Lines { get; }

        IEnumerable<string> GetColumnValues(int columnIndex);
    } 
}
