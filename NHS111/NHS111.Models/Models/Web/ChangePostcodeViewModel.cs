using FluentValidation.Attributes;
using NHS111.Models.Models.Web.Validators;

namespace NHS111.Models.Models.Web
{
    [Validator(typeof(ChangePostcodeViewModelValidator))]
    public class ChangePostcodeViewModel
    {
        public string Postcode { get; set; }
    }
}
