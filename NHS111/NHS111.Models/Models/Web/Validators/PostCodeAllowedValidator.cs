using System;
using System.CodeDom;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Linq.Expressions;
using System.Web.Mvc;
using CsvHelper;
using FluentValidation.Validators;
using NHS111.Features;
using NHS111.Models.Models.Domain;
using NHS111.Models.Models.Web.FromExternalServices;

namespace NHS111.Models.Models.Web.Validators
{
    public class PostCodeAllowedValidator<TModel, TProperty> : PropertyValidator, IClientValidatable
    {
        private readonly string _dependencyElement;
        private readonly IAllowedPostcodeFeature _allowedPostcodeFeature;

        public PostCodeAllowedValidator(Expression<Func<TModel, TProperty>> expression) : this(expression, new AllowedPostcodeFeature())
        {

        }

        public PostCodeAllowedValidator(Expression<Func<TModel, TProperty>> expression, IAllowedPostcodeFeature allowedPostcodeFeature) : base("Allowed postcode violated")
        {
            _dependencyElement = (expression.Body as MemberExpression).Member.Name;
            _allowedPostcodeFeature = allowedPostcodeFeature;
        }

        protected override bool IsValid(PropertyValidatorContext context)
        {
            var postcodeViewModel = context.Instance as PostcodeViewModel;
            return IsAllowedPostcode(postcodeViewModel.Postcode);
        }

        public bool IsAllowedPostcode(string postcode)
        {
            if (!_allowedPostcodeFeature.IsEnabled) return true;

            if(_allowedPostcodeFeature.PostcodeFile == TextReader.Null) return false;

            var csv = new CsvReader(_allowedPostcodeFeature.PostcodeFile);
            var postcodes = csv.GetRecords<ValidPostCode>().ToList();
            return postcodes.Any(p => p.ParsedPostcode.Contains(ValidPostCode.ParsePostcode(postcode)));
        }

        public IEnumerable<ModelClientValidationRule> GetClientValidationRules(ModelMetadata metadata, ControllerContext context)
        {
            var rule = new ModelClientValidationRule
            {
                ErrorMessage = this.ErrorMessageSource.GetString(), // default error message
                ValidationType = "allowedpostcode" // name of the validatoin which will be used inside unobtrusive library
            };

            rule.ValidationParameters["prefixelement"] = _dependencyElement;

            yield return rule;
        }
    }
}
