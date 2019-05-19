using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NHS111.Models.Models.Domain;
using NHS111.Models.Models.Web.CCG;

namespace NHS111.Models.Models.Web.Validators
{
    public interface IPostCodeAllowedValidator
    {
        PostcodeValidatorResponse IsAllowedPostcode(string postcode);
        CCGDetailsModel CcgModel { get; }
    }


    public enum PostcodeValidatorResponse
    {
        ValidPostcodePathwaysAreaUndefined,
        PostcodeNotFound,
        OutsidePathwaysArea,
        InvalidSyntax,
        InPathwaysAreaWithPharmacyServices,
        InPathwaysAreaWithoutPharmacyServices,
        OutsidePathwaysAreaWithPharmacyServices
    }

    public static class PostcodeValidatioResponseExtension
    {
        public static bool IsInPilotAreaForOutcome(this PostcodeValidatorResponse response, OutcomeGroup outcome)
        {
            if (!outcome.IsPharmacyGroup && response == PostcodeValidatorResponse.InPathwaysAreaWithoutPharmacyServices)
                return true;

            return outcome.IsPharmacyGroup && response == PostcodeValidatorResponse.InPathwaysAreaWithPharmacyServices;
        }
    }
}
