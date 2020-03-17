using System.Text.RegularExpressions;
using NHS111.Models.Models.Web.Validators;

namespace NHS111.Web.Presentation.Validators
{
    public class EmailAddressValidator : IEmailAddressValidator
    {
        public bool Validate(string emailAddress)
        {
            if (string.IsNullOrWhiteSpace(emailAddress))
            {
                return false;
            }

            if (!_emailRegex.IsMatch(emailAddress))
            {
                return false;
            }

            return true;
        }

        private readonly Regex _emailRegex = new Regex(@"[^@]+@[^\.]+\..+");
    }
}
