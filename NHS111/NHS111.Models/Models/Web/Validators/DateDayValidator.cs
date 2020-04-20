using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Web.Mvc;
using FluentValidation.Validators;

namespace NHS111.Models.Models.Web.Validators
{
    public class DateDayValidator<TModel, TProperty> : PropertyValidator, IClientValidatable
    {

        public DateDayValidator(Expression<Func<TModel, TProperty>> expression)
            : base("Incorrect Day")
        {
        }


        protected override bool IsValid(PropertyValidatorContext context)
        {
            var userInfo = context.Instance as UserInfo;
            if (userInfo != null) return IsAValidDay(userInfo.Day);

            var dateTimeViewModel = context.Instance as DateTimeViewModel;
            return IsAValidDay(dateTimeViewModel.Day);
        }
  

        private bool IsAValidDay(int? day)
        {
            return (day > 0 && day < 32);
        }

        public IEnumerable<ModelClientValidationRule> GetClientValidationRules(ModelMetadata metadata, ControllerContext context)
        {
            var ruleDay = new ModelClientValidationRule
            {
                ErrorMessage = this.ErrorMessageSource.GetString(), // default error message
                ValidationType = "day" // name of the validation which will be used inside unobtrusive library
            };

            yield return ruleDay;
        }
    }
}
