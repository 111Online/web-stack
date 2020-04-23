using FluentValidation.Validators;
using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Web.Mvc;

namespace NHS111.Models.Models.Web.Validators
{
    public class DateOfBirthMonthValidator<TModel, TProperty> : PropertyValidator, IClientValidatable
    {
        private string dependencyElement;
        public DateOfBirthMonthValidator(Expression<Func<TModel, TProperty>> expression)
            : base("Incorrect Month")
        {
            dependencyElement = (expression.Body as MemberExpression).Member.Name;
        }


        protected override bool IsValid(PropertyValidatorContext context)
        {
            var userInfo = context.Instance as UserInfo;
            if (userInfo != null) return IsAValidMonth(userInfo.Month);

            var dateTimeViewModel = context.Instance as DateTimeViewModel;
            return IsAValidMonth(dateTimeViewModel.Month);
        }

        private bool IsAValidMonth(int? month)
        {
            return (month > 0 && month < 13);
        }

        public IEnumerable<ModelClientValidationRule> GetClientValidationRules(ModelMetadata metadata, ControllerContext context)
        {
            var ruleMonth = new ModelClientValidationRule
            {
                ErrorMessage = this.ErrorMessageSource.GetString(), // default error message
                ValidationType = "month" // name of the validatoin which will be used inside unobtrusive library
            };

            ruleMonth.ValidationParameters["prefixelement"] = dependencyElement; // html element which includes prefix information

            yield return ruleMonth;
        }
    }
}
