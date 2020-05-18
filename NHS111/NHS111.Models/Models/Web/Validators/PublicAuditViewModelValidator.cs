using FluentValidation;
using NHS111.Models.Models.Web.Logging;

namespace NHS111.Models.Models.Web.Validators
{
    public class PublicAuditViewModelValidator : AbstractValidator<PublicAuditViewModel>
    {
        public PublicAuditViewModelValidator()
        {
            RuleFor(p => p.SessionId).NotEmpty();
            RuleFor(p => p.EventKey).NotEmpty();
            RuleFor(p => p.EventValue).NotEmpty().Length(1, 250);
            RuleFor(p => p.Page).Length(1, 250);
        }
    }
}