using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CsvHelper.Configuration;
using CsvHelper.TypeConversion;
using NHS111.Business.Glossary.Api.Models;


namespace NHS111.Domain.Glossary.Configuration
{

    public class DefinitionsMap : CsvClassMap<DefinedTerm>
    {
        public override void CreateMap()
        {
            Map(m => m.Term).Name("GLOSSARYTERM");
            Map(m => m.Definition).Name("DESCRIPTION");
            Map(m => m.Synonyms).Name("SYNONYMS");
        }
    }
}
