using FluentValidation;

namespace NHS111.Models.Models.Web.Validators
{
    public class UserInfoValidator : AbstractValidator<UserInfo>
    {
        public UserInfoValidator()
        {
            RuleFor(p => p.FirstName).NotEmpty();
            RuleFor(p => p.LastName).NotEmpty();
            RuleFor(p => p.TelephoneNumber).NotEmpty();
            RuleFor(p => p.Day).SetValidator(new DateOfBirthValidator<UserInfo, int?>(m => m.Day));
            RuleFor(p => p.Month).SetValidator(new DateOfBirthValidator<UserInfo, int?>(m => m.Month));
            RuleFor(p => p.Year).SetValidator(new DateOfBirthValidator<UserInfo, int?>(m => m.Year));
        }
    }
}
