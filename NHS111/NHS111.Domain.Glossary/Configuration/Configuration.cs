using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CsvHelper.Configuration;
using CsvHelper.TypeConversion;

namespace NHS111.Domain.Glossary.Configuration
{
    public class Configuration : IConfiguration
    { 
        public string TermsCsvFilePath()
        {
            return ConfigurationManager.AppSettings["termsCsvFilePath"];
        }

    }

}
