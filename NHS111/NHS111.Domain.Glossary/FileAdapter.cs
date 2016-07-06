using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NHS111.Domain.Glossary
{
    public class FileAdapter : IFileAdapter
    {
        private string _filepath;
        public FileAdapter(string filepath)
        {
            _filepath = filepath;
        }
        public string Filename
        {
            get
            {
              return _filepath;
            }
            set { _filepath = value; }
        }

        public StreamReader OpenText()
        {
           return File.OpenText(_filepath);
        }
    }
}
