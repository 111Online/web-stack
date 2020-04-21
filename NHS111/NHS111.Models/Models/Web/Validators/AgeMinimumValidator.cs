using FluentValidation.Validators;
using NHS111.Features;
using NHS111.Models.Models.Domain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Web.Mvc;

namespace NHS111.Models.Models.Web.Validators
{
    public class AgeMinimumValidator<TModel, TProperty> : PropertyValidator, IClientValidatable
    {

        private readonly string _dependencyElement;
        private readonly IFilterPathwaysByAgeFeature _filterPathwaysByAgeFeature;

        public AgeMinimumValidator(Expression<Func<TModel, TProperty>> expression) : this(expression, new FilterPathwaysByAgeFeature())
        {

        }

        public AgeMinimumValidator(Expression<Func<TModel, TProperty>> expression, IFilterPathwaysByAgeFeature filterPathwaysByAgeFeature) : base("Age restriction violated")
        {
            _dependencyElement = (expression.Body as MemberExpression).Member.Name;
            _filterPathwaysByAgeFeature = filterPathwaysByAgeFeature;
        }

        protected override bool IsValid(PropertyValidatorContext context)
        {
            var ageGenderViewModel = context.Instance as AgeGenderViewModel;
            return IsAValidAge(ageGenderViewModel.Age);
        }

        public bool IsAValidAge(int age)
        {
            if (!_filterPathwaysByAgeFeature.IsEnabled) return true;

            var ageCategories = _filterPathwaysByAgeFeature.FilteredAgeCategories.Select(a => new AgeCategory(a));
            return !ageCategories.Any(a => age >= a.MinimumAge && age <= a.MaximumAge);
        }

        public IEnumerable<ModelClientValidationRule> GetClientValidationRules(ModelMetadata metadata, ControllerContext context)
        {
            var rule = new ModelClientValidationRule
            {
                ErrorMessage = this.ErrorMessageSource.GetString(), // default error message
                ValidationType = "ageminimum" // name of the validatoin which will be used inside unobtrusive library
            };

            rule.ValidationParameters["prefixelement"] = _dependencyElement;
            // html element which includes prefix information

            yield return rule;
        }
    }
}
