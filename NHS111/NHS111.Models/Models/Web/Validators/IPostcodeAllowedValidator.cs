using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NHS111.Models.Models.Web.CCG;

namespace NHS111.Models.Models.Web.Validators
{
    public interface IPostCodeAllowedValidator
    {
        PostcodeValidatorResponse IsAllowedPostcode(string postcode);
        Task<PostcodeValidatorResponse> IsAllowedPostcodeAsync(string postcode);
        CCGDetailsModel CcgModel { get; }
    }


    public enum PostcodeValidatorResponse
    {
        ValidPostcodePathwaysAreaUndefined,
        InPathwaysArea,
        PostcodeNotFound,
        OutsidePathwaysArea,
        InvalidSyntax,
        InAreaWithPharmacyServices
    }
}
