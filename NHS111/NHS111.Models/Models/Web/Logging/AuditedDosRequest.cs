using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NHS111.Models.Models.Web.Logging
{
    public class AuditedDosRequest : DosViewModel
    {
        private static readonly char[] Digits = "0123456789".ToCharArray();
        private string _postcode;

        public override string PostCode
        {
            get { return GetPartialPostcode(); }
            set { _postcode = value; }
        }

        private string GetPartialPostcode()
        {
            if (string.IsNullOrEmpty(_postcode)) return _postcode;

            var postcode = _postcode.Replace(" ", "");
            var lastDigit = postcode.LastIndexOfAny(Digits);
            if (lastDigit == -1)
            {
                throw new ArgumentException("No digits!");
            }
            return (lastDigit < postcode.Length - 1) ? postcode.Substring(0, lastDigit) : postcode;
        }
    }
}
