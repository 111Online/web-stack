using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Web.Mvc;
using FluentValidation.Validators;

namespace NHS111.Models.Models.Web.Validators
{
    class SMSVerificationCodeValidator<TModel, TProperty> : PropertyValidator, IClientValidatable
    {
        private string _dependencyElement;
        public SMSVerificationCodeValidator(Expression<Func<TModel, TProperty>> expression)
            : base("Enter valid verification code")
        {
            _dependencyElement = (expression.Body as MemberExpression).Member.Name;
        }

        protected override bool IsValid(PropertyValidatorContext context)
        {
            var model = ((VerificationCodeInputViewModel)context.Instance);

            if(string.IsNullOrWhiteSpace(model.InputValue))
            {
                return false;
            }

            return GetMatch(model.InputValue).Success;
        }

        private Match GetMatch(string verificationCodeInput)
        {
            return Regex.Match(verificationCodeInput.Trim(), @"^[0-9]{6,6}$");
        }

        public IEnumerable<ModelClientValidationRule> GetClientValidationRules(ModelMetadata metadata, ControllerContext context)
        {
            var ruleEmail = new ModelClientValidationRule()
            {
                ErrorMessage = this.ErrorMessageSource.GetString(),
                ValidationType = "smsVerificationCodeInput"
            };

            ruleEmail.ValidationParameters["prefixelement"] = _dependencyElement;

            yield return ruleEmail;
        }
    }
}
