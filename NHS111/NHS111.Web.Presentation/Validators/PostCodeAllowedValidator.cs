using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NHS111.Models.Models.Web.Validators;
using NHS111.Web.Presentation.Builders;

namespace NHS111.Web.Presentation.Validators
{
    public class PostCodeAllowedValidator : IPostCodeAllowedValidator
    {
        private ICCGModelBuilder _ccgModelBuilder;
        public PostCodeAllowedValidator(ICCGModelBuilder ccgModelBuilder)
        {
            _ccgModelBuilder= ccgModelBuilder;
        }
        public bool IsAllowedPostcode(string postcode)
        {
            var ccg = _ccgModelBuilder.FillCCGModel(postcode).GetAwaiter().GetResult();
            return (ccg != null && ccg.App == "Pathways");
        }
    }
}
