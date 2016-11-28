using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Web.Mvc;
using FluentValidation.Validators;
using NHS111.Features;

namespace NHS111.Models.Models.Web.Validators
{
    public class AgeValidator<TModel, TProperty> : PropertyValidator, IClientValidatable
    {

        private readonly string _dependencyElement;

        public AgeValidator(Expression<Func<TModel, TProperty>> expression) : base("Age restriction violated")
        {
            _dependencyElement = (expression.Body as MemberExpression).Member.Name;
        }


        protected override bool IsValid(PropertyValidatorContext context)
        {
            var userInfo = context.Instance as UserInfo;
            return IsAValidAge(userInfo.Age);
        }

        private static bool IsAValidAge(int age)
        {
            var ageFeature = new FilterPathwaysByAgeFeature();
            return ageFeature.IsEnabled;
        }

        public IEnumerable<ModelClientValidationRule> GetClientValidationRules(ModelMetadata metadata, ControllerContext context)
        {
            var rule = new ModelClientValidationRule
            {
                ErrorMessage = this.ErrorMessageSource.GetString(), // default error message
                ValidationType = "age" // name of the validatoin which will be used inside unobtrusive library
            };

            rule.ValidationParameters["prefixelement"] = _dependencyElement;
            // html element which includes prefix information

            yield return rule;
        }
    }
}
