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
        InvalidSyntax,
        InPathwaysAreaWithPharmacyServices,
        InPathwaysAreaWithoutPharmacyServices
    }

    public static class PostcodeValidatioResponseExtension
    {
        public static bool IsInAreaForOutcome(this PostcodeValidatorResponse response)
        {
            if (response == PostcodeValidatorResponse.InPathwaysAreaWithoutPharmacyServices || response == PostcodeValidatorResponse.InPathwaysAreaWithPharmacyServices)
                return true;

            return response == PostcodeValidatorResponse.InPathwaysAreaWithPharmacyServices;
        }
    }
}
