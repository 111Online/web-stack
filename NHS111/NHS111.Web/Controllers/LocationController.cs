using System;
using System.Collections.Generic;
using System.Net;
using System.Threading.Tasks;
using System.Web.Mvc;
using AutoMapper;
using Microsoft.Ajax.Utilities;
using NHS111.Models.Models.Web;
using NHS111.Models.Models.Web.CCG;
using NHS111.Models.Models.Web.Validators;
using NHS111.Utils.Filters;
using NHS111.Web.Presentation.Builders;
using IConfiguration = NHS111.Web.Presentation.Configuration.IConfiguration;

namespace NHS111.Web.Controllers
{
    public class LocationController : Controller
    {
        private readonly IPostCodeAllowedValidator _postCodeAllowedValidator;
        private readonly ILocationResultBuilder _locationResultBuilder;
        private readonly IConfiguration _configuration;

        public LocationController(IPostCodeAllowedValidator postCodeAllowedValidator, ILocationResultBuilder locationResultBuilder, IConfiguration configuration)
        {
            _postCodeAllowedValidator = postCodeAllowedValidator;
            _locationResultBuilder = locationResultBuilder;
            _configuration = configuration;
        }
        [HttpGet, SetSessionIdFilter]
        public ActionResult Home(LocationViewModel model)
        {
            return View(model);
        }

        [HttpPost]
        public ActionResult Find(LocationViewModel model)
        {
            var postcodeValidationRepsonse = _postCodeAllowedValidator.IsAllowedPostcode(model.Postcode);
            if (postcodeValidationRepsonse == PostcodeValidatorResponse.InvalidSyntax)
                return View("Home", model);

            return DeriveApplicationView(model, postcodeValidationRepsonse, _postCodeAllowedValidator.CcgModel);
        }

        [HttpPost]
        public ActionResult FindByAddress(ConfirmLocationViewModel model)
        {
            var postcodeValidationRepsonse = _postCodeAllowedValidator.IsAllowedPostcode(model.SelectedPostcode);

            return DeriveApplicationView(model, postcodeValidationRepsonse, _postCodeAllowedValidator.CcgModel);
        }

        [HttpPost]
        public async Task<ActionResult> ConfirmAddress(string longlat, string sessionId)
        {
            var results = await _locationResultBuilder.LocationResultByGeouilder(longlat);
            var locationResults = Mapper.Map<List<AddressInfoViewModel>>(results.DistinctBy(r => r.Thoroughfare));
            return View("ConfirmLocation", new ConfirmLocationViewModel { FoundLocations = locationResults, SessionId = Guid.Parse(sessionId) });
        }

        [HttpGet]
        [Route("home/provider/{providerName}", Name = "ProviderRoute")]
        public ActionResult Provider(ProviderViewModel model, string providerName)
        {
            if (providerName == DUCTriageApp.Expert24.Name)
                return Redirect(_configuration.Expert24Url);

            return View(providerName.Replace(" ", ""), model);
        }

        private ActionResult DeriveApplicationView(JourneyViewModel model, PostcodeValidatorResponse postcodeValidationRepsonse, CCGModel ccg)
        {
            var moduleZeroViewName = "../Question/InitialQuestion";
            switch (postcodeValidationRepsonse)
            {
                case PostcodeValidatorResponse.InPathwaysArea:
                    return View(moduleZeroViewName, new JourneyViewModel { SessionId = model.SessionId, CurrentPostcode = ccg.Postcode, Campaign = ccg.StpName, Source = ccg.CCG});
                case PostcodeValidatorResponse.OutsidePathwaysArea:
                    return RedirectToRoute("ProviderRoute",
                        new
                        {
                            providerName = ccg.App,
                            ccg.Postcode,
                            Campaign = WebUtility.UrlEncode(ccg.StpName),
                            Source = WebUtility.UrlEncode(ccg.CCG)
                        });
                case PostcodeValidatorResponse.PostcodeNotFound:
                    return View("OutOfArea", new OutOfAreaViewModel { SessionId = model.SessionId, Campaign = ccg.StpName, Source = ccg.CCG });
                default:
                    return View("Home");
            }
        }
    }
}