using FluentValidation.Validators;
using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Text.RegularExpressions;
using System.Web.Mvc;
using System.Web.UI.WebControls;

namespace NHS111.Models.Models.Web.Validators
{
    public class FirstNameValidator<TModel, TProperty> : PropertyValidator, IClientValidatable
    {
        private const string validCharactersPattern = @"^[a-zA-Z]+(([',. -][a-zA-Z ])?[a-zA-Z]*)*$";

        public FirstNameValidator(Expression<Func<TModel, TProperty>> expression)
            : base("Enter first name")
        {
            
        }

        protected override bool IsValid(PropertyValidatorContext context)
        {
            var value = context.PropertyValue as string;

            return !string.IsNullOrEmpty(value) && Regex.Match(value, validCharactersPattern).Success;
        }

        public IEnumerable<ModelClientValidationRule> GetClientValidationRules(ModelMetadata metadata, ControllerContext context)
        {
            var ruleForFirstName = new ModelClientValidationRule
            {
                ErrorMessage = "Do not use special characters", 
                ValidationType = "firstname" // name of the validation which will be used inside unobtrusive library
            };

            yield return ruleForFirstName;
        }


    }
}
