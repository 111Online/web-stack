using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FluentValidation.Attributes;
using NHS111.Models.Models.Web.Validators;

namespace NHS111.Models.Models.Web
{
    [Validator(typeof(LocationInfoViewModelValidator))]
    public class LocationInfoViewModel
    {

        public CurrentAddressViewModel PatientCurrentAddress { get; set; }
        public PersonalDetailsAddressViewModel PatientHomeAddress { get; set; }
        public HomeAddressSameAsCurrent? HomeAddressSameAsCurrent { get; set; }


        public LocationInfoViewModel()
        {
            PatientCurrentAddress = new CurrentAddressViewModel();
            PatientHomeAddress = new PersonalDetailsAddressViewModel();
        }
    }

    public enum HomeAddressSameAsCurrent
    {
        Yes,
        No,
        DontKnow
    }

}
