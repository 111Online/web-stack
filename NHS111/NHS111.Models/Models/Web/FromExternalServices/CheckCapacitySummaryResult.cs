namespace NHS111.Models.Models.Web.FromExternalServices
{
    public class CheckCapacitySummaryResult
    {
        public int IdField { get; set; }
        public DosCapacity CapacityField { get; set; }
        public string NameField { get; set; }
        public string PublicNameField { get; set; }
        public string ContactDetailsField { get; set; }
        public string AddressField { get; set; }
        public string PostcodeField { get; set; }
        public int NorthingsField { get; set; }
        public bool NorthingsSpecifiedField { get; set; }
        public int EastingsField { get; set; }
        public bool EastingsSpecifiedField { get; set; }
        public string UrlField { get; set; }
        public string NotesField { get; set; }
        public bool ObsoleteField { get; set; }
        public System.DateTime UpdateTimeField { get; set; }
        public bool OpenAllHoursField { get; set; }
        public ServiceCareItemRotaSession[] RotaSessionsField { get; set; }
        public string[] OpenTimeSpecifiedSessionsField { get; set; }
        public ServiceType ServiceTypeField { get; set; }
        public string OdsCodeField { get; set; }
        public ServiceDetails RootParentField { get; set; }
    }
}