using FluentValidation;

namespace NHS111.Models.Models.Web.Validators
{
    public class ChangePostcodeViewModelValidator : AbstractValidator<ChangePostcodeViewModel>
    {
        public ChangePostcodeViewModelValidator()
        {
            RuleFor(a => a.Postcode).NotEmpty();
        }
    }
}
