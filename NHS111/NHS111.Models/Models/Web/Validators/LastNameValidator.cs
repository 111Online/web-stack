using FluentValidation.Validators;
using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Text.RegularExpressions;
using System.Web.Mvc;

namespace NHS111.Models.Models.Web.Validators
{
    public class LastNameValidator<TModel, TProperty> : PropertyValidator, IClientValidatable
    {
        private const string validCharactersPattern = @"^[a-zA-Z0-9]+(([',. -][a-zA-Z0-9 ])?[a-zA-Z]*)*$";

        public LastNameValidator(Expression<Func<TModel, TProperty>> expression)
            : base("Enter last name")
        {
        }

        protected override bool IsValid(PropertyValidatorContext context)
        {
            var value = context.PropertyValue as string;
            return !string.IsNullOrWhiteSpace(value) && Regex.Match(value, validCharactersPattern).Success;
        }

        public IEnumerable<ModelClientValidationRule> GetClientValidationRules(ModelMetadata metadata, ControllerContext context)
        {
            var ruleForLastName = new ModelClientValidationRule
            {
                ErrorMessage = "Do not use special characters",
                ValidationType = "lastname" // name of the validation which will be used inside unobtrusive library
            };

            yield return ruleForLastName;
        }


    }
}
