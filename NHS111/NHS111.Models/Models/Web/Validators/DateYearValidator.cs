using FluentValidation.Validators;
using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Web.Mvc;
using FluentValidation.Validators;
using NHS111.Models.Models.Web.PersonalDetails;

namespace NHS111.Models.Models.Web.Validators
{
    public class DateYearValidator<TModel, TProperty> : PropertyValidator, IClientValidatable
    {
        private string dependencyElement;
        public DateYearValidator(Expression<Func<TModel, TProperty>> expression)
            : base("Incorrect Year")
        {
            dependencyElement = (expression.Body as MemberExpression).Member.Name;
        }


        protected override bool IsValid(PropertyValidatorContext context)
        {
            var userInfo = context.Instance as DateOfBirthViewModel;
            if (userInfo != null) return IsAValidYear(userInfo.Year);

            var dateTimeViewModel = context.Instance as DateTimeViewModel;
            return IsAValidYear(dateTimeViewModel.Year);
        }

        private bool IsAValidYear(int? year)
        {
            return (year > 1900 && year < (DateTime.Now.Year + 1));
        }

        public IEnumerable<ModelClientValidationRule> GetClientValidationRules(ModelMetadata metadata, ControllerContext context)
        {
            var ruleYear = new ModelClientValidationRule
            {
                ErrorMessage = this.ErrorMessageSource.GetString(), // default error message
                ValidationType = "year" // name of the validation which will be used inside unobtrusive library
            };

            ruleYear.ValidationParameters["prefixelement"] = dependencyElement; // html element which includes prefix information

            yield return ruleYear;
        }
    }
}
