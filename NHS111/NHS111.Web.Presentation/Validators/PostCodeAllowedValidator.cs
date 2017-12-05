using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NHS111.Models.Models.Web.Validators;
using NHS111.Models.Models.Web.CCG;
using NHS111.Web.Presentation.Builders;
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
        public bool IsAllowedPostcode(string postcode)
        {
            if (!_allowedPostcodeFeature.IsEnabled) return true;
            Task<CCGModel> ccgModelBuildertask = Task.Run<CCGModel>(async () => await _ccgModelBuilder.FillCCGModel(postcode));
            var ccg = ccgModelBuildertask.Result;
            return DUCTriageApp.IsPathways(ccg.App);
        }
    }

}
