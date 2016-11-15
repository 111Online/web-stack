using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq.Expressions;
using System.Web.Mvc;
using FluentValidation;
using FluentValidation.Attributes;
using FluentValidation.Internal;
using FluentValidation.Mvc;
using FluentValidation.Validators;


namespace NHS111.Models.Models.Web
{
    [Validator(typeof(UserInfoValidator))]
    public class UserInfo
    {
        public string Gender { get; set; }
        public int Age { get; set; }


        public string FirstName { get; set; }
        public string LastName { get; set; }
        public int? Day { get; set; }
        public int? Month { get; set; }
        public int? Year { get; set; }

        private DateTime? _dob;
        public DateTime? DoB
        {
            get
            {
                if (Year != null && Month != null && Day != null)
                    return _dob = new DateTime(Year.Value, Month.Value, Day.Value);
                return _dob;
            }
        }
        public string TelephoneNumber { get; set; }
        public string Email { get; set; }


        public AddressInfoViewModel HomeAddress { get; set; }
        public AddressInfoViewModel CurrentAddress { get; set; }

        public UserInfo()
        {
            HomeAddress = new AddressInfoViewModel();
            CurrentAddress = new AddressInfoViewModel();
        }
    }

    public class UserInfoValidator : AbstractValidator<UserInfo>
    {
        public UserInfoValidator()
        {
            RuleFor(p => p.FirstName).NotEmpty();
            RuleFor(p => p.LastName).NotEmpty();
            RuleFor(p => p.TelephoneNumber).NotEmpty();
            RuleFor(p => p.Day).SetValidator(new DateOfBirthValidator<UserInfo, int?>(m => m.Day));
            RuleFor(p => p.Month).SetValidator(new DateOfBirthValidator<UserInfo, int?>(m => m.Month));
            RuleFor(p => p.Year).SetValidator(new DateOfBirthValidator<UserInfo, int?>(m => m.Year));
        }

       
    }

    public class DateOfBirthValidator<TModel, TProperty> : PropertyValidator, IClientValidatable
    {
        private string dependencyElement;
        public DateOfBirthValidator(Expression<Func<TModel, TProperty>> expression)
            : base("Incorrect Date")
        {
            dependencyElement = (expression.Body as MemberExpression).Member.Name;
        }


        protected override bool IsValid(PropertyValidatorContext context)
        {
           UserInfo userInfo = context.Instance as UserInfo;
            
            return IsAValidDate(userInfo.Day, userInfo.Month, userInfo.Year);
        }

        private bool IsAValidDate(int? day, int? month, int? year)
        {
            DateTime date;
            if (!day.HasValue || !month.HasValue || !year.HasValue) return false;
            return DateTime.TryParseExact(String.Format("{0}/{1}/{2}", day.Value, month.Value, year.Value).ToString(),
                "dd/MM/yyyy", CultureInfo.InvariantCulture, DateTimeStyles.None, out date);
        }

        public IEnumerable<ModelClientValidationRule> GetClientValidationRules(ModelMetadata metadata, ControllerContext context)
        {
            var rule = new ModelClientValidationRule
            {
                ErrorMessage = this.ErrorMessageSource.GetString(), // default error message
                ValidationType = "dateofbirth" // name of the validatoin which will be used inside unobtrusive library
            };

            rule.ValidationParameters["prefixelement"] = dependencyElement; // html element which includes prefix information

            yield return rule;
        }
    }
   
  
}