namespace NHS111.Models.Models.Web.ITK
{
    public class PatientDetails
    {
        public string Forename { get; set; }
        public string Surname { get; set; }
        public DateOfBirth DateOfBirth { get; set; }
        public string Gender { get; set; }
        public string NhsNumber { get; set; }
        public string InformationType { get; set; }
        public string InformationName { get; set; }
        public Address CurrentAddress { get; set; }
        public Address HomeAddress { get; set; }
        public GpPractice GpPractice { get; set; }
        public string EmailAdress { get; set; }
        public string TelephoneNumber { get; set; }
    }
}
