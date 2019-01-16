using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NHS111.Models.Models.Web.Validators
{
    public interface IPostCodeAllowedValidator
    {
        PostcodeValidatorResponse IsAllowedPostcode(string postcode);
    }


    public enum PostcodeValidatorResponse
    {
        ValidPostcodePathwaysAreaUndefined,
        InPathwaysArea,
        PostcodeNotFound,
        OutsidePathwaysArea,
        InvalidSyntax
    }
}
