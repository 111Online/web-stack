using System.Web.Mvc;
using NHS111.Models.Models.Web.Validators;

namespace NHS111.Web.Controllers
{
    public class BaseController : Controller
    {
        public readonly IPostCodeAllowedValidator PostCodeAllowedValidator;

        public BaseController(IPostCodeAllowedValidator postCodeAllowedValidator)
        {
            PostCodeAllowedValidator = postCodeAllowedValidator;
        }

        public bool IsPostcodeInArea(string postcode)
        {
            return PostCodeAllowedValidator.IsAllowedPostcode(postcode).IsInAreaForOutcome();
        }
    }
}