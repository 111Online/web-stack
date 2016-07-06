using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using CsvHelper.Configuration;

namespace NHS111.Domain.Glossary.Configuration
{
    public interface IConfiguration
    {
        string TermsCsvFilePath(); 
    }
}
