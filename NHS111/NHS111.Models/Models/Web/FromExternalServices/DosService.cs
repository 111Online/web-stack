using System;
using System.Runtime.CompilerServices;
using Newtonsoft.Json;

namespace NHS111.Models.Models.Web.FromExternalServices
{
    public class DosService
    {
        [JsonProperty(PropertyName = "idField")]
        public int Id { get; set; }
        [JsonProperty(PropertyName = "nameField")]
        public string Name { get; set; }
        [JsonProperty(PropertyName = "postcodeField")]
        public string PostCode { get; set; }
        [JsonProperty(PropertyName = "odsCodeField")]
        public string OdsCode { get; set; }
        [JsonProperty(PropertyName = "capacityField")]
        public DosCapacity Capacity { get; set; }
        [JsonProperty(PropertyName = "contactDetailsField ")]
        public string ContactDetails { get; set; }
        [JsonProperty(PropertyName = "addressField")]
        public string Address { get; set; }
        [JsonProperty(PropertyName = "northingsField")]
        public int Northings { get; set; }
        [JsonProperty(PropertyName = "eastingsField")]
        public int Eastings { get; set; }
        [JsonProperty(PropertyName = "urlField")]
        public string Url { get; set; }
        [JsonProperty(PropertyName = "serviceTypeField")]
        public ServiceDetails ServiceType { get; set; }
        [JsonProperty(PropertyName = "nonPublicTelephoneNo")]
        public string NonPublicTelephoneNo { get; set; }
        [JsonProperty(PropertyName = "fax")]
        public string Fax { get; set; }
        [JsonProperty(PropertyName = "referralText")]
        public string ReferralText { get; set; }
        [JsonProperty(PropertyName = "distance")]
        public string Distance { get; set; }
        [JsonProperty(PropertyName = "notesField")]
        public string Notes { get; set; }
        [JsonProperty(PropertyName = "openAllHoursField")]
        public bool OpenAllHours { get; set; }
        [JsonProperty(PropertyName = "rotaSessionsField")]
        public ServiceCareItemRotaSession[] RotaSessions { get; set; }
        [JsonProperty(PropertyName = "openTimeSpecifiedSessions")]
        public OpenTimeSpecifiedSession[] OpenTimeSpecifiedSessions { get; set; }
    }

    public enum DosCapacity
    {
        /// <remarks/>
        High,
        /// <remarks/>
        Low,
        /// <remarks/>
        None,
    }

    public class ServiceCareItemRotaSession
    {
        [JsonProperty(PropertyName = "startDayOfWeek")]
        public DayOfWeek StartDayOfWeek;
        [JsonProperty(PropertyName = "startTime")]
        public TimeOfDay StartTime;
        [JsonProperty(PropertyName = "endDayOfWeek")]
        public DayOfWeek EndDayOfWeek;
        [JsonProperty(PropertyName = "endTime")]
        public TimeOfDay EndTime;
        [JsonProperty(PropertyName = "status")]
        public string Status;
    }

    public class OpenTimeSpecifiedSession
    {
        [JsonProperty(PropertyName = "openTimeSpecified")]
        public string OpenTimeSpecified;
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

    public class TimeOfDay
    {
        [JsonProperty(PropertyName = "hours")]
        public short Hours;
        [JsonProperty(PropertyName = "minutes")]
        public short Minutes;
    }
}
