using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Text.RegularExpressions;
using System.Web.Mvc;
using FluentValidation.Validators;
using NHS111.Features;

namespace NHS111.Models.Models.Web.Validators
{
    public static class PostCodeFormatValidator
    {
        public static string PostcodeRegex = @"^([Gg][Ii][Rr] 0[Aa]{2})|((([A-Za-z][0-9]{1,2})|(([A-Za-z][A-Ha-hJ-Yj-y][0-9]{1,2})|(([A-Za-z][0-9][A-Za-z])|([A-Za-z][A-Ha-hJ-Yj-y][0-9]?[A-Za-z]))))[0-9][A-Za-z]{2})$";

    }
    public class PostCodeFormatValidator<TModel, TProperty> : PropertyValidator, IClientValidatable
    {
        private readonly string _dependencyElement;

        public PostCodeFormatValidator(Expression<Func<TModel, TProperty>> expression) : base("Incorrect postcode format")
        {
            _dependencyElement = (expression.Body as MemberExpression).Member.Name;
        }


        protected override bool IsValid(PropertyValidatorContext context)
        {
            var personalInfoAddressViewModel = context.Instance as FindServicesAddressViewModel;

            return IsAValidPostcode(personalInfoAddressViewModel.Postcode);
        }

        public static bool IsAValidPostcode(string postcode)
        {
            return !string.IsNullOrEmpty(postcode) && Regex.IsMatch(postcode.Replace(" ", string.Empty).ToLower(), PostCodeFormatValidator.PostcodeRegex);
        }

        public IEnumerable<ModelClientValidationRule> GetClientValidationRules(ModelMetadata metadata, ControllerContext context)
        {
            var rule = new ModelClientValidationRule
            {
                ErrorMessage = this.ErrorMessageSource.GetString(), // default error message
                ValidationType = "validpostcode" // name of the validation which will be used inside unobtrusive library
            };

            rule.ValidationParameters["prefixelement"] = _dependencyElement; // html element which includes prefix information

            yield return rule;
        }
    }
}
