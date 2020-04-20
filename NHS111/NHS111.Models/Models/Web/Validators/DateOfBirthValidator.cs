using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq.Expressions;
using System.Web.Mvc;
using NHS111.Features;
using FluentValidation.Validators;
using System.Linq;
using Nest;
using NHS111.Models.Models.Domain;

namespace NHS111.Models.Models.Web.Validators
{
    public class DateOfBirthValidator<TModel, TProperty> : PropertyValidator, IClientValidatable
    {
        private readonly IFilterPathwaysByAgeFeature _filterPathwaysByAgeFeature;
        private readonly IEnumerable<AgeCategory> _excludedAgeCategories;
        private PropertyValidatorContext _context;

        private static class _ErrorMessage
        {
            public const string InvalidDate = "Enter a valid date";
            public const string InvalidDoB = "Enter a valid date of birth";
            public const string MinimumAge = "Sorry, this service is not available for children under 5 years of age, for medical advice please call 111.";
        }

        // By default one error message is used per validator. We override this by 
        // using AppendArgument (inside SetValidationMessage) which sets the message as appropriate.
        private static readonly string dynamicErrorMessage = "{ValidationMessage}"; 

        public DateOfBirthValidator(Expression<Func<TModel, TProperty>> expression) : base(dynamicErrorMessage)
        {
            _filterPathwaysByAgeFeature = new FilterPathwaysByAgeFeature();
            _excludedAgeCategories = _filterPathwaysByAgeFeature.FilteredAgeCategories.Select(a => new AgeCategory(a));
        }


        protected override bool IsValid(PropertyValidatorContext context)
        {
            _context = context;

            var userInfo = context.Instance as UserInfo;
            if (userInfo != null) return IsValidAge(userInfo.DoB);

            var dateTimeViewModel = context.Instance as DateTimeViewModel;
            return IsValidAge(dateTimeViewModel.Date);
        }


        private bool IsValidAge(DateTime? DoB)
        {

            if (!DoB.HasValue)
            {
                SetValidationMessage(_ErrorMessage.InvalidDate);
                return false;
            }


            var today = DateTime.Today;
            var dob = DoB.Value;

            int age = today.Year - dob.Year;
            if (today.Month < dob.Month || (today.Month == dob.Month && today.Day < dob.Day))
                age--;

            // Ensure date is not in the future (ie. not a DoB)
            if (age < 0)
            {
                SetValidationMessage(_ErrorMessage.InvalidDoB);
                return false;
            }

            if (_filterPathwaysByAgeFeature.IsEnabled)
            {
                // Check minimum age
                if (_excludedAgeCategories.Any(a => age >= a.MinimumAge && age <= a.MaximumAge))
                {
                    SetValidationMessage(_ErrorMessage.MinimumAge);
                    return false;
                }

                // Check maximum age
                if (age > AgeCategory.Adult.MaximumAge)
                {
                    SetValidationMessage(_ErrorMessage.InvalidDoB);
                    return false;
                }
            }

            return true;
        }

        public IEnumerable<ModelClientValidationRule> GetClientValidationRules(ModelMetadata metadata, ControllerContext context)
        {
            // Check it is a valid date.
            // Ideally we would overwrite the default date error message instead but that is not allowed.
            var ruleDate = new ModelClientValidationRule
            {
                ErrorMessage = _ErrorMessage.InvalidDate,
                ValidationType = "datevalid"
            };

            yield return ruleDate;


            // Check it is a valid date of birth (ie. in the past)
            var ruleDoB = new ModelClientValidationRule
            {
                ErrorMessage = _ErrorMessage.InvalidDoB,
                ValidationType = "dateofbirth"
            };

            yield return ruleDoB;


            if (_filterPathwaysByAgeFeature.IsEnabled)
            {
                var ruleMinAge = new ModelClientValidationRule
                {
                    ErrorMessage = _ErrorMessage.MinimumAge,
                    ValidationType = "dateagemin"
                };
                ruleMinAge.ValidationParameters.Add("minimumage", _excludedAgeCategories.Any() ? _excludedAgeCategories.Max(a => a.MaximumAge) : 0);

                yield return ruleMinAge;


                var ruleMaxAge = new ModelClientValidationRule
                {
                    ErrorMessage = _ErrorMessage.InvalidDoB,
                    ValidationType = "dateagemax"
                };
                ruleMaxAge.ValidationParameters.Add("maximumage", AgeCategory.Adult.MaximumAge);

                yield return ruleMaxAge;
            }
        }

        private void SetValidationMessage(string message)
        {
            _context.MessageFormatter.AppendArgument("ValidationMessage", message);
        }
    }
}
