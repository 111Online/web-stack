using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using System.Web.Mvc;
using FluentValidation.Validators;

namespace NHS111.Models.Models.Web.Validators
{
    public class HomeAddressValidator<TModel, TProperty> : PropertyValidator, IClientValidatable
    {
        private string _dependencyElement;

        public HomeAddressValidator(Expression<Func<TModel, TProperty>> expression)
            : base("Specify Name")
        {
            _dependencyElement = (expression.Body as MemberExpression).Member.Name;
        }

        protected override bool IsValid(PropertyValidatorContext context)
        {
            return true;
        }

        public IEnumerable<ModelClientValidationRule> GetClientValidationRules(ModelMetadata metadata, ControllerContext context)
        {
            var ruleYear = new ModelClientValidationRule
            {
                ErrorMessage = this.ErrorMessageSource.GetString(), // default error message
                ValidationType = "homeaddress" // name of the validation which will be used inside unobtrusive library
            };

            ruleYear.ValidationParameters["prefixelement"] = _dependencyElement; // html element which includes prefix information

            yield return ruleYear;
        }


    }
}

