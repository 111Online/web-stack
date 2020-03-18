using System;

namespace NHS111.Models.Models.Web.ITK
{
    public class PatientDetails
    {
        public string Forename { get; set; }
        public string Surname { get; set; }
        public DateTime DateOfBirth { get; set; }
        public string AgeGroup { get; set; }
        public string Gender { get; set; }
        public string ServiceAddressPostcode { get; set; }
        public Address HomeAddress { get; set; }
        public Address CurrentAddress { get; set; }
        public GpPractice GpPractice { get; set; }
        public string TelephoneNumber { get; set; }
        public InformantDetails Informant { get; set; }
        public string EmailAddress { get; set; }
    }
}
