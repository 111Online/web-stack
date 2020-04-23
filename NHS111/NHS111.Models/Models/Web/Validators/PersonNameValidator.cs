using FluentValidation.Validators;
using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Web.Mvc;

namespace NHS111.Models.Models.Web.Validators
{
    public class PersonNameValidator<TModel, TProperty> : PropertyValidator, IClientValidatable
    {
        private string _dependencyElement;

        public PersonNameValidator(Expression<Func<TModel, TProperty>> expression)
            : base("Specify Name")
        {
            _dependencyElement = (expression.Body as MemberExpression).Member.Name;
        }

        protected override bool IsValid(PropertyValidatorContext context)
        {
            return true;
        }

        public IEnumerable<ModelClientValidationRule> GetClientValidationRules(ModelMetadata metadata, ControllerContext context)
        {
            var ruleYear = new ModelClientValidationRule
            {
                ErrorMessage = this.ErrorMessageSource.GetString(), // default error message
                ValidationType = "personname" // name of the validatoin which will be used inside unobtrusive library
            };

            ruleYear.ValidationParameters["prefixelement"] = _dependencyElement; // html element which includes prefix information

            yield return ruleYear;
        }


    }
}
