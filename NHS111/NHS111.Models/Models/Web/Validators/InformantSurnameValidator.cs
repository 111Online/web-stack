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
    public class InformantSurnameValidator<TModel, TProperty> : PropertyValidator, IClientValidatable
    {
        private string dependencyElement;
        public InformantSurnameValidator(Expression<Func<TModel, TProperty>> expression)
            : base("Empty surname")
        {
            dependencyElement = (expression.Body as MemberExpression).Member.Name;
        }


        protected override bool IsValid(PropertyValidatorContext context)
        {
            var informantViewModel = context.Instance as InformantViewModel;

            return informantViewModel.IsInformantForPatient == false || !string.IsNullOrEmpty(informantViewModel.Surname);
        }

        public IEnumerable<ModelClientValidationRule> GetClientValidationRules(ModelMetadata metadata, ControllerContext context)
        {
            var ruleForename = new ModelClientValidationRule
            {
                ErrorMessage = this.ErrorMessageSource.GetString(), // default error message
                ValidationType = "surname" // name of the validation which will be used inside unobtrusive library
            };

            ruleForename.ValidationParameters["prefixelement"] = dependencyElement; // html element which includes prefix information

            yield return ruleForename;
        }
    }
}
