using System;

namespace NHS111.Models.Models.Web.DosRequests
{
    public class DosCase
    {
        public DosCase()
        {
            AgeFormat = AgeFormatType.Years;
            NumberPerType = 2;
        }

        public string CaseRef { get; set; }
        public string CaseId { get; set; }
        public virtual string PostCode { get; set; }
        public string Surgery { get { return "UNK"; } }
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
        public string SearchDateTime { get;  set; }

        public void SpecifySpecificSearchDate(DateTime searchDateTime)
        {
            SearchDateTime = searchDateTime.ToUniversalTime().ToString("yyyy-MM-ddTHH:mm:ss+00:00");
        }
    }

    public enum AgeFormatType
    {
        Days,
        Months,
        Years,
        AgeGroup
    }
}
