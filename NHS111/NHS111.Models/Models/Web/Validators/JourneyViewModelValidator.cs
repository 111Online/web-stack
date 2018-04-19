using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using System.Web.Mvc;
using FluentValidation;
using FluentValidation.Validators;

namespace NHS111.Models.Models.Web.Validators
{
    public class JourneyViewModelValidator : AbstractValidator<JourneyViewModel>
    {
        public JourneyViewModelValidator()
        {
            
        }
    }
}
