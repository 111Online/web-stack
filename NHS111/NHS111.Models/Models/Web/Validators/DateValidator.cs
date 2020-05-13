using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq.Expressions;
using System.Web.Mvc;
using FluentValidation.Validators;
using NHS111.Models.Models.Web.PersonalDetails;

namespace NHS111.Models.Models.Web.Validators
{
    public class DateValidator<TModel, TProperty> : PropertyValidator, IClientValidatable
    {
        public DateValidator(Expression<Func<TModel, TProperty>> expression)
            : base("Enter a valid date")
        {
        }


        protected override bool IsValid(PropertyValidatorContext context)
        {
            var userInfo = context.Instance as DateOfBirthViewModel;
            if (userInfo != null) return IsAValidDate(userInfo.Day, userInfo.Month, userInfo.Year);

            var dateTimeViewModel = context.Instance as DateTimeViewModel;
            return IsAValidDate(dateTimeViewModel.Day, dateTimeViewModel.Month, dateTimeViewModel.Year);
        }

        private bool IsAValidDate(int? day, int? month, int? year)
        {
            DateTime date;
            if (!day.HasValue || !month.HasValue || !year.HasValue) return false;
            return DateTime.TryParseExact(String.Format("{0}/{1}/{2}", day.Value, month.Value, year.Value).ToString(),
                "d/M/yyyy", CultureInfo.InvariantCulture, DateTimeStyles.None, out date);
        }

        public IEnumerable<ModelClientValidationRule> GetClientValidationRules(ModelMetadata metadata, ControllerContext context)
        {
            var ruleDate = new ModelClientValidationRule
            {
                ErrorMessage = this.ErrorMessageSource.GetString(), // default error message
                ValidationType = "date" // name of the validation which will be used inside unobtrusive library
            };

            yield return ruleDate;
        }
    }
}
