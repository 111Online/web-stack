namespace NHS111.Models.Models.Web.FromExternalServices
{
    public class CheckCapacitySummaryResult
    {
        public int IdField { get; set; }
        public Capacity CapacityField { get; set; }
        public string NameField { get; set; }
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
        public ServiceDetails ServiceTypeField { get; set; }
        public string OdsCodeField { get; set; }
        public ServiceDetails RootParentField { get; set; }
    }

    public enum Capacity
    {

        /// <remarks/>
        High,

        /// <remarks/>
        Low,

        /// <remarks/>
        None,
    }

    public  class ServiceCareItemRotaSession 
    {

        public DayOfWeek StartDayOfWeekField;

        public TimeOfDay StartTimeField;

        public DayOfWeek EndDayOfWeekField;

        public TimeOfDay EndTimeField;

        public string StatusField;
    }

    public enum DayOfWeek
    {

        /// <remarks/>
        Sunday,

        /// <remarks/>
        Monday,

        /// <remarks/>
        Tuesday,

        /// <remarks/>
        Wednesday,

        /// <remarks/>
        Thursday,

        /// <remarks/>
        Friday,

        /// <remarks/>
        Saturday,
    }

    public  class TimeOfDay
    {

        public short HoursField;

        public short MinutesField;
    }

    public class ServiceDetails
    {

        public long IdField;

        public string NameField;
    }
}