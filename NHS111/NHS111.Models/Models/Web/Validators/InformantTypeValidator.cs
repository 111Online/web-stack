using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FluentValidation;
using NHS111.Models.Models.Web.PersonalDetails;

namespace NHS111.Models.Models.Web.Validators
{
    public class InformantTypeValidator : AbstractValidator<InformantTypeViewModel>
    {
        public InformantTypeValidator()
        {
            RuleFor(p => p.Informant).NotEqual(InformantType.NotSpecified);
        }
    }
}
