using FluentValidation.Validators;
using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Web.Mvc;

namespace NHS111.Models.Models.Web.Validators
{
    public class InformantForenameValidator<TModel, TProperty> : PropertyValidator, IClientValidatable
    {
        private string dependencyElement;
        public InformantForenameValidator(Expression<Func<TModel, TProperty>> expression)
            : base("Empty forename")
        {
            dependencyElement = (expression.Body as MemberExpression).Member.Name;
        }


        protected override bool IsValid(PropertyValidatorContext context)
        {
            var informantViewModel = context.Instance as InformantViewModel;

            return informantViewModel.IsInformantForPatient == false || !string.IsNullOrEmpty(informantViewModel.Forename);
        }

        public IEnumerable<ModelClientValidationRule> GetClientValidationRules(ModelMetadata metadata, ControllerContext context)
        {
            var ruleForename = new ModelClientValidationRule
            {
                ErrorMessage = this.ErrorMessageSource.GetString(), // default error message
                ValidationType = "forename" // name of the validatoin which will be used inside unobtrusive library
            };

            ruleForename.ValidationParameters["prefixelement"] = dependencyElement; // html element which includes prefix information

            yield return ruleForename;
        }
    }
}
