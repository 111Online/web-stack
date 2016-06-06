using NHS111.Models.Models.Domain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NHS111.Web.Presentation.Models
{
    public class DosCase
    {
        public DosCase()
        {
            AgeFormat = AgeFormatType.Years;
            Surgery = "UKN";
            NumberPerType = 2;
            SearchDistance = 60;
        }

        public string CaseRef { get; set; }
        public string CaseId { get; set; }
        public string PostCode { get; set; }
        public string Surgery  { get; set; }
        public string Age { get; set; }
        public AgeFormatType AgeFormat { get; set; }
        public int Disposition { get; set; }
        public int SymptomGroup { get; set; }
        public int[] SymptomDiscriminatorList { get; set; }
        public int SymptomDiscriminator { get; set; }
        public int SearchDistance { get; set; }
        public bool SearchDistanceSpecified { get { return SearchDistance > 0; } }
        public string Gender { get; set; }
        public int NumberPerType { get; set; }
    }

    public enum AgeFormatType
    {
        Years,
        AgeGroup,
    }
}
