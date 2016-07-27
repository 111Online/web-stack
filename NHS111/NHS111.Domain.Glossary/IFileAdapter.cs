using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NHS111.Domain.Glossary
{
    public interface IFileAdapter
    {
        String Filename { get; set; }
        StreamReader OpenText();
    }
   
}
