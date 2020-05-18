using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NHS111.Models.Models.Web
{
    public class PersonalDetailViewModel : OutcomeViewModel
    {
        public LocationInfoViewModel AddressInformation { get; set; }
     
        public EmailAddressViewModel EmailAddress { get; set; }
        public PersonalDetailViewModel() 
        {
            AddressInformation = new LocationInfoViewModel();
         
            EmailAddress = new EmailAddressViewModel();
        }
    }
}
