using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NHS111.Models.Models.Web.ITK;

namespace NHS111.Models.Models.Web.Book
{
    public class Patient
    {
        public string FamilyName { get; set; }
        public string GivenName { get; set; }
        public string TelephoneNumber { get; set; }
        public string Gender { get; set; }
        public DateTime DateOfBirth { get; set; }
        public Address Address { get; set; }
    }
}
