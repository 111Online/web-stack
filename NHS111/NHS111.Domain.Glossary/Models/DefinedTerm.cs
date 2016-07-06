using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace NHS111.Business.Glossary.Api.Models
{
    public class DefinedTerm
    {
        public string Term { get; set; }
        public string Definition { get; set; }
        public string Synonyms { get; set; }
    }
}