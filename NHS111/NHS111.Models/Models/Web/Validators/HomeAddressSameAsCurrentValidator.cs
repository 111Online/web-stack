using FluentValidation;

namespace NHS111.Models.Models.Web.Validators
{
    public class HomeAddressSameAsCurrentValidator : AbstractValidator<HomeAddressSameAsCurrentWrapper>
    {
        public HomeAddressSameAsCurrentValidator()
        {
            RuleFor(m => m.HomeAddressSameAsCurrent.HasValue).Equals(true);
        }
    }
}