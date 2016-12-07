using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Web.Mvc;
using FluentValidation.Validators;
using NHS111.Features;

namespace NHS111.Models.Models.Web.Validators
{
    public class PostCodeFormatValidator<TModel, TProperty> : PropertyValidator, IClientValidatable
    {
        private string dependencyElement;
        public PostCodeFormatValidator(Expression<Func<TModel, TProperty>> expression) : base("Incorrect postcode format")
        {
            dependencyElement = (expression.Body as MemberExpression).Member.Name;
        }


        protected override bool IsValid(PropertyValidatorContext context)
        {
            var postcodeViewModel = context.Instance as PostcodeViewModel;

            return IsAValidPostcode(postcodeViewModel.PostCode);
        }

        private static bool IsAValidPostcode(string postcode)
        {
            return true;
        }

        public IEnumerable<ModelClientValidationRule> GetClientValidationRules(ModelMetadata metadata, ControllerContext context)
        {
            var rule = new ModelClientValidationRule
            {
                ErrorMessage = this.ErrorMessageSource.GetString(), // default error message
                ValidationType = "valid-postcode" // name of the validatoin which will be used inside unobtrusive library
            };

            rule.ValidationParameters["prefixelement"] = dependencyElement; // html element which includes prefix information

            yield return rule;
        }
    }
}
