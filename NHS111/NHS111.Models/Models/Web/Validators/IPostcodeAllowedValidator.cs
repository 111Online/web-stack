using NHS111.Models.Models.Domain;
using NHS111.Models.Models.Web.CCG;
using System.Threading.Tasks;


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
            if (!outcome.IsPharmacyGroup && (response == PostcodeValidatorResponse.InPathwaysAreaWithoutPharmacyServices || response == PostcodeValidatorResponse.InPathwaysAreaWithPharmacyServices))
                return true;

            return outcome.IsPharmacyGroup && response == PostcodeValidatorResponse.InPathwaysAreaWithPharmacyServices;
        }
    }
}
