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
    class EmailAddressValidator<TModel, TProperty> : PropertyValidator, IClientValidatable
    {
        public EmailAddressValidator(string errorMessageResourceName, Type errorMessageResourceType) : base(errorMessageResourceName, errorMessageResourceType)
        {
        }

        public EmailAddressValidator(string errorMessage) : base(errorMessage)
        {
        }

        public EmailAddressValidator(Expression<Func<string>> errorMessageResourceSelector) : base(errorMessageResourceSelector)
        {
        }

        protected override bool IsValid(PropertyValidatorContext context)
        {
            throw new NotImplementedException();
        }

        public IEnumerable<ModelClientValidationRule> GetClientValidationRules(ModelMetadata metadata, ControllerContext context)
        {
            throw new NotImplementedException();
        }
    }
}
