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
    public class PersonNameValidator<TModel, TProperty> : PropertyValidator, IClientValidatable
    {

        public PersonNameValidator(Expression<Func<TModel, TProperty>> expression)
            : base("Specify Name")
        {
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
                ValidationType = "personname" // name of the validation which will be used inside unobtrusive library
            };

            yield return ruleYear;
        }


    }
}
