﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FluentValidation;

namespace NHS111.Models.Models.Web.Validators
{
    public class AgeGenderViewModelValidator : AbstractValidator<AgeGenderViewModel>
    {
        public AgeGenderViewModelValidator()
        {
            RuleFor(p => p.Age).SetValidator(new AgeValidator<AgeGenderViewModel, int>(u => u.Age));
        }
    }
}