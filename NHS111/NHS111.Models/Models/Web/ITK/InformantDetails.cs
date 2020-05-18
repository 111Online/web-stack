namespace NHS111.Models.Models.Web.ITK
{
    public enum InformantType
    {
        Self,
        NotSpecified
    }

    public class InformantDetails
    {
        public string Forename { get; set; }
        public string Surname { get; set; }
        public string TelephoneNumber { get; set; }
        public InformantType Type { get; set; }
    }
}