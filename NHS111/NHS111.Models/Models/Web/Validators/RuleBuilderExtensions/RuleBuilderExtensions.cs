using FluentValidation;
using System;
using System.Linq.Expressions;

namespace NHS111.Models.Models.Web.Validators.RuleBuilderExtensions
{
    public static class RuleBuilderExtensions
    {
        public static IRuleBuilderOptions<TItem, TProperty> IsPostCode<TItem, TProperty>(this IRuleBuilder<TItem, TProperty> ruleBuilder, Expression<Func<TItem, TProperty>> expression)
        {
            return ruleBuilder.SetValidator(new PostCodeFormatValidator<TItem, TProperty>(expression));
        }
    }
}
