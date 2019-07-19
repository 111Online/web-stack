using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using System.Web.Script.Serialization;
using AutoMapper;
using Microsoft.Ajax.Utilities;
using NHS111.Models.Models.Web;
using NHS111.Models.Models.Web.Validators;
using NHS111.Utils.Attributes;
using NHS111.Web.Presentation.Builders;
using NHS111.Web.Presentation.Logging;

namespace NHS111.Web.Controllers
{
    public class PersonalDetailsController : Controller
    {
        private readonly IAuditLogger _auditLogger;
        private readonly ILocationResultBuilder _locationResultBuilder;

        public PersonalDetailsController(IAuditLogger auditLogger, ILocationResultBuilder locationResultBuilder)
        {
            _auditLogger = auditLogger;
            _locationResultBuilder = locationResultBuilder;
        }

        [HttpPost]
        public async Task<JsonResult> PostcodeLookup(string postCode)
        {
            var locationResults = await GetPostcodeResults(postCode);
            return Json((locationResults));
        }

        private async Task<AddressInfoCollectionViewModel> GetPostcodeResults(string postCode)
        {
            if (string.IsNullOrWhiteSpace(postCode)) return AddressInfoCollectionViewModel.InvalidSyntaxResponse;
            Regex regex = new Regex(@"^[a-zA-Z0-9]+$");
            if (!regex.IsMatch(postCode.Replace(" ", ""))) return AddressInfoCollectionViewModel.InvalidSyntaxResponse;

            var results = await _locationResultBuilder.LocationResultValidatedByPostCodeBuilder(postCode);
            return Mapper.Map<AddressInfoCollectionViewModel>(results);
        }

        [HttpPost]
        public async Task<ActionResult> PersonalDetails(PersonalDetailViewModel model)
        {
            ModelState.Clear();
            _auditLogger.LogSelectedService(model);

            return View("~\\Views\\PersonalDetails\\PersonalDetails.cshtml", model);
        }


        private async Task<PersonalDetailViewModel> PopulateAddressPickerFields(PersonalDetailViewModel model)
        {
            //map postcode to field to submit to ITK (preventing multiple entries of same data)
            model.AddressInformation.PatientCurrentAddress.PreviouslyEnteredPostcode = model.CurrentPostcode;

            //pre-populate picker fields from postcode lookup service
            var postcodes = await GetPostcodeResults(model.AddressInformation.PatientCurrentAddress.PreviouslyEnteredPostcode);
            if (postcodes.ValidatedPostcodeResponse == PostcodeValidatorResponse.PostcodeNotFound) return model;

            var items = new List<SelectListItem>();
            items.AddRange(postcodes.Addresses.Select(postcode =>
                new SelectListItem { Text = postcode.FormattedAddress, Value = postcode.UPRN }).ToList());
            model.AddressInformation.PatientCurrentAddress.AddressPicker = items;

            return model;
        }

        [HttpPost]
        public async Task<ActionResult> ChangePostcode(OutcomeViewModel model)
        {
            ModelState.Clear();
            _auditLogger.LogEventData(model, "User elected to change postcode.");
            return View(model); ;
        }

        [HttpPost]
        public async Task<ActionResult> CurrentAddress(PersonalDetailViewModel model)
        {
            model = await PopulateAddressPickerFields(model);

            return View(model);
        }

        [HttpPost]
        public async Task<ActionResult> EnterDifferentCurrentAddress(PersonalDetailViewModel model)
        {
            return View("~\\Views\\PersonalDetails\\CurrentAddress_Change.cshtml", model);
        }

        [HttpPost]
        public async Task<ActionResult> SubmitCurrentAddress(PersonalDetailViewModel model, string currentAddress)
        {
            if (currentAddress == "AddressNotListed")
                return await EnterDifferentCurrentAddress(model);

            //populate current address fields from data


            return View("~\\Views\\PersonalDetails\\HomeAddressSameAsCurrentAddress.cshtml", model);
        }
    }
}