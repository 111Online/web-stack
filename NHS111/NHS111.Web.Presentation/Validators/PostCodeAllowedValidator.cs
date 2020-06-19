using NHS111.Features;
using NHS111.Models.Models.Web.CCG;
using NHS111.Models.Models.Web.Validators;
using NHS111.Web.Presentation.Builders;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace NHS111.Web.Presentation.Validators
{
    using System;

    public class PostCodeAllowedValidator : IPostCodeAllowedValidator
    {
        private readonly ICCGModelBuilder _ccgModelBuilder;
        private readonly IAllowedPostcodeFeature _allowedPostcodeFeature;

        public PostCodeAllowedValidator(IAllowedPostcodeFeature allowedPostcodeFeature, ICCGModelBuilder ccgModelBuilder)
        {
            _allowedPostcodeFeature = allowedPostcodeFeature;
            _ccgModelBuilder = ccgModelBuilder;
        }

        public PostcodeValidatorResponse IsAllowedPostcode(string postcode)
        {
            var postcodeValidatorResponse = Task.Run(async () => await IsAllowedPostcodeAsync(postcode));
            return postcodeValidatorResponse.Result;
        }

        public async Task<PostcodeValidatorResponse> IsAllowedPostcodeAsync(string postcode)
        {
            if (string.IsNullOrWhiteSpace(postcode))
                return PostcodeValidatorResponse.InvalidSyntax;
            if (!_postcodeRegex.IsMatch(postcode))
                return PostcodeValidatorResponse.InvalidSyntax;
            if (!_allowedPostcodeFeature.IsEnabled)
                return PostcodeValidatorResponse.InPathwaysAreaWithPharmacyServices;

            try
            {
                CcgModel = await _ccgModelBuilder.FillCCGDetailsModelAsync(postcode);
            }
            catch (ArgumentException)
            {
                return PostcodeValidatorResponse.InvalidSyntax;
            }
            if (CcgModel.Postcode == null)
                return PostcodeValidatorResponse.PostcodeNotFound;
            if (!CcgModel.PharmacyServicesAvailable && !string.IsNullOrEmpty(CcgModel.Postcode))
                return PostcodeValidatorResponse.InPathwaysAreaWithoutPharmacyServices;
            return PostcodeValidatorResponse.InPathwaysAreaWithPharmacyServices;
        }

        public CCGDetailsModel CcgModel { get; private set; }

        private readonly Regex _postcodeRegex = new Regex(PostCodeFormatValidator.PostcodeRegex, RegexOptions.IgnoreCase);
    }
}
