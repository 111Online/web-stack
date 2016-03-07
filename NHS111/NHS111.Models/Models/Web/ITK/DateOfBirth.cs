using System;
using System.Xml.Serialization;

namespace NHS111.Models.Models.Web.ITK
{
    public class DateOfBirth
    {
        [XmlElement("dateOfBirth")]
        public DateTime DateOfBirthItem { get; set; }
    }
}
