using FluentValidation.Validators;
using NHS111.Features;
using NHS111.Models.Models.Domain;
using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Web.Mvc;

namespace NHS111.Models.Models.Web.Validators
{
    public class AgeMaximumValidator<TModel, TProperty> : PropertyValidator, IClientValidatable
    {

        private readonly string _dependencyElement;

        public AgeMaximumValidator(Expression<Func<TModel, TProperty>> expression) : this(expression, new FilterPathwaysByAgeFeature())
        {

        }

        public AgeMaximumValidator(Expression<Func<TModel, TProperty>> expression, IFilterPathwaysByAgeFeature filterPathwaysByAgeFeature) : base("Age restriction violated")
        {
            _dependencyElement = (expression.Body as MemberExpression).Member.Name;
        }

        protected override bool IsValid(PropertyValidatorContext context)
        {
            var ageGenderViewModel = context.Instance as AgeGenderViewModel;
            return IsAValidAge(ageGenderViewModel.Age);
        }

        public bool IsAValidAge(int age)
        {
            return (age <= AgeCategory.Adult.MaximumAge);
        }

        public IEnumerable<ModelClientValidationRule> GetClientValidationRules(ModelMetadata metadata, ControllerContext context)
        {
            var rule = new ModelClientValidationRule
            {
                ErrorMessage = this.Options.ErrorMessageSource.GetString(null), // default error message
                ValidationType = "agemaximum" // name of the validation which will be used inside unobtrusive library
            };

            rule.ValidationParameters["prefixelement"] = _dependencyElement;
            // html element which includes prefix information

            yield return rule;
        }
    }
}
