using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using NHS111.Utils.Parser;
using NUnit.Framework;

namespace NHS111.Utils.Test.Parser
{
    [TestFixture]
    public class CsvParserTests
    {
        private const string FilePath = @"Parser\Leeds postcodes.csv";

        [Test]
        [ExpectedException(typeof(FileNotFoundException))]
        public void Missing_file_throws_exception()
        {
            var sut = new CsvParser("file/does/not/exist");
        }

        [Test]
        public void Valid_file_reads_all_lines()
        {
            var sut = new CsvParser(FilePath);
            Assert.AreEqual(21364, sut.Lines.Count());
        }

        [Test]
        public void Valid_file_with_header_removes_header_line()
        {
            var sut = new CsvParser(FilePath, true);
            Assert.AreEqual(21363, sut.Lines.Count());
        }

        [Test]
        [ExpectedException(typeof(IndexOutOfRangeException))]
        public void Invalid_column_index_throws_error()
        {
            var sut = new CsvParser(FilePath);
            sut.GetColumnValues(1000);
        }

        [Test]
        public void Valid_column_index_returns_all_values_in_column()
        {
            var sut = new CsvParser(FilePath, true);
            var colVals = sut.GetColumnValues(0);

            Assert.AreEqual(21363, colVals.Count());
        }
    }
}
