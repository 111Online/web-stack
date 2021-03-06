﻿using FluentValidation.Validators;
using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Web.Mvc;
using FluentValidation.Validators;
using NHS111.Models.Models.Web.PersonalDetails;

namespace NHS111.Models.Models.Web.Validators
{
    public class DateMonthValidator<TModel, TProperty> : PropertyValidator, IClientValidatable
    {
        private string dependencyElement;
        public DateMonthValidator(Expression<Func<TModel, TProperty>> expression)
            : base("Incorrect Month")
        {
            dependencyElement = (expression.Body as MemberExpression).Member.Name;
        }


        protected override bool IsValid(PropertyValidatorContext context)
        {
            var userInfo = context.Instance as DateOfBirthViewModel;
            if (userInfo != null) return IsAValidMonth(userInfo.Month);

            var dateTimeViewModel = context.Instance as DateTimeViewModel;
            return IsAValidMonth(dateTimeViewModel.Month);
        }

        private bool IsAValidMonth(int? month)
        {
            return (month > 0 && month < 13);
        }

        public IEnumerable<ModelClientValidationRule> GetClientValidationRules(ModelMetadata metadata, ControllerContext context)
        {
            var ruleMonth = new ModelClientValidationRule
            {
                ErrorMessage = this.Options.ErrorMessageSource.GetString(null), // default error message
                ValidationType = "month" // name of the validation which will be used inside unobtrusive library
            };

            ruleMonth.ValidationParameters["prefixelement"] = dependencyElement; // html element which includes prefix information

            yield return ruleMonth;
        }
    }
}
