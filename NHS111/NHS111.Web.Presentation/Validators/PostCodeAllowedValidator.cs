using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NHS111.Models.Models.Web.Validators;
using NHS111.Models.Models.Web.CCG;
using NHS111.Web.Presentation.Builders;
using System.Text.RegularExpressions;
using NHS111.Features;
namespace NHS111.Web.Presentation.Validators
{
    public class PostCodeAllowedValidator : IPostCodeAllowedValidator
    {
        private ICCGModelBuilder _ccgModelBuilder;
        private IAllowedPostcodeFeature _allowedPostcodeFeature;
        
        public PostCodeAllowedValidator(IAllowedPostcodeFeature allowedPostcodeFeature, ICCGModelBuilder ccgModelBuilder)
        {
            _allowedPostcodeFeature = allowedPostcodeFeature;
            _ccgModelBuilder= ccgModelBuilder;
        }
        public PostcodeValidatorResponse IsAllowedPostcode(string postcode)
        {
            if (string.IsNullOrWhiteSpace(postcode)) return PostcodeValidatorResponse.InvalidSyntax;
            Regex regex = new Regex(@"^[a-zA-Z0-9]+$");
            if(!regex.IsMatch(postcode.Replace(" ", ""))) return PostcodeValidatorResponse.InvalidSyntax;
            Task<CCGModel> ccgModelBuildertask = Task.Run<CCGModel>(async () => await _ccgModelBuilder.FillCCGModel(postcode));
            if (!_allowedPostcodeFeature.IsEnabled) return PostcodeValidatorResponse.InPathwaysArea;
            var ccg = ccgModelBuildertask.Result;
            if (ccg.Postcode == null) return PostcodeValidatorResponse.PostcodeNotFound;
            if (!DUCTriageApp.IsPathways(ccg.App)) return PostcodeValidatorResponse.OutsidePathwaysArea;
            else return PostcodeValidatorResponse.InPathwaysArea;
        }
    }


   

}
