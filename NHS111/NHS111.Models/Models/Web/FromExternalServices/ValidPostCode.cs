using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NHS111.Models.Models.Web.FromExternalServices
{
    public class ValidPostCode
    {
        public string Postcode { get; set; }

        public string ParsedPostcode { get { return ParsePostcode(Postcode); } }

        public static string ParsePostcode(string postcode)
        {
            return postcode.Replace(" ", "").ToLower();
        }
    }
}
