﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Web.Mvc;
using AutoMapper;
using NHS111.Features;
using NHS111.Models.Models.Web;
using NHS111.Models.Models.Web.FromExternalServices;
using NHS111.Models.Models.Web.PersonalDetails;
using NHS111.Models.Models.Web.Validators;
using NHS111.Web.Presentation.Builders;
using NHS111.Web.Presentation.Logging;

namespace NHS111.Web.Controllers
{
    public class PersonalDetailsController : Controller
    {
        private readonly IAuditLogger _auditLogger;
        private readonly ILocationResultBuilder _locationResultBuilder;
        private readonly IEmailCollectionFeature _emailCollectionFeature;

        public PersonalDetailsController(IAuditLogger auditLogger, ILocationResultBuilder locationResultBuilder, 
            IEmailCollectionFeature emailCollectionFeature)
        {
            _auditLogger = auditLogger;
            _locationResultBuilder = locationResultBuilder;
            _emailCollectionFeature = emailCollectionFeature;
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
            model.AddressInformation.PatientCurrentAddress.PreviouslyEnteredPostcode = model.CurrentPostcode;

            var postcodeToUseForSearch = model.CurrentPostcode;

            if (model.AddressInformation.ChangePostcode != null && !string.IsNullOrEmpty(model.AddressInformation.ChangePostcode.Postcode))
                postcodeToUseForSearch = model.AddressInformation.ChangePostcode.Postcode;

            //pre-populate picker fields from postcode lookup service
            var postcodes = await GetPostcodeResults(postcodeToUseForSearch);
            if (postcodes.ValidatedPostcodeResponse == PostcodeValidatorResponse.PostcodeNotFound) return model;

            var items = new List<SelectListItem>();
            items.AddRange(postcodes.Addresses.Select(postcode =>
                new SelectListItem { Text = postcode.FormattedAddress, Value = postcode.UDPRN }).ToList());
            model.AddressInformation.PatientCurrentAddress.AddressPicker = items;

            return model;
        }

        private async Task<PersonalDetailViewModel> PopulateChosenCurrentAddress(string udprn, PersonalDetailViewModel model)
        {
            var result = await _locationResultBuilder.LocationResultByUDPRNBuilder(udprn);
            var mappedResult = Mapper.Map<CurrentAddressViewModel>(result);

            model.AddressInformation.PatientCurrentAddress = mappedResult;

            return model;
        }

        [HttpPost]
        public async Task<ActionResult> ChangeCurrentAddressPostcode(PersonalDetailViewModel model)
        {          
            if (!ModelState.IsValid)
                return View("~\\Views\\PersonalDetails\\CurrentAddress_ChangePostcode.cshtml", model);

            return await DirectToPopulatedCurrentAddressPicker(model);
        }

        public async Task<ActionResult> DirectToPopulatedCurrentAddressPicker(PersonalDetailViewModel model)
        {
            model = await PopulateAddressPickerFields(model);

            if (model.AddressInformation.PatientCurrentAddress.AddressPicker.Count > 0)
            {
                ModelState.Clear();
                return View("~\\Views\\PersonalDetails\\CurrentAddress.cshtml", model);
            }
            else
            {
                //a location api error, or empty list
                return View("~\\Views\\PersonalDetails\\CurrentAddress_NotFound.cshtml", model);
            }

        }

        [HttpPost]
        public async Task<ActionResult> EnterHomePostcode(PersonalDetailViewModel model, string changeHomeAddressPostcode)
        {
            if (changeHomeAddressPostcode == "unknownHomeAddress")
            {

                model.AddressInformation.PatientHomeAddress = null;
                model.AddressInformation.HomeAddressSameAsCurrentWrapper.HomeAddressSameAsCurrent = HomeAddressSameAsCurrent.DontKnow;
                return View("~\\Views\\PersonalDetails\\ConfirmDetails.cshtml", model);
            }

            if (!ModelState.IsValid)
                return View("~\\Views\\PersonalDetails\\HomeAddress_Postcode.cshtml", model);
            else
            {
                var postcodes = await GetPostcodeResults(model.AddressInformation.ChangePostcode.Postcode);
                if (postcodes.ValidatedPostcodeResponse == PostcodeValidatorResponse.PostcodeNotFound)
                {
                    ModelState.AddModelError("AddressInformation.ChangePostcode.Postcode", new Exception());
                    return View("~\\Views\\PersonalDetails\\HomeAddress_Postcode.cshtml", model);
                }
                
                model.AddressInformation.PatientHomeAddress.Postcode = model.AddressInformation.ChangePostcode.Postcode;
                return View("~\\Views\\PersonalDetails\\ConfirmDetails.cshtml", model);
            }
        }

        [HttpPost]
        public ActionResult TelephoneNumber(PersonalDetailViewModel model)
        {

            if (!ModelState.IsValid)
            {
                return View("~\\Views\\PersonalDetails\\PersonalDetails.cshtml", model);
            }

            return View("~\\Views\\PersonalDetails\\TelephoneNumber.cshtml", new TelephoneNumberViewModel(model));
        }

        [HttpPost]
        public ActionResult DateOfBirth(PersonalDetailViewModel model)
        {

            if (!ModelState.IsValid)
            {
                return View("~\\Views\\PersonalDetails\\PersonalDetails.cshtml", model);
            }

            return View("~\\Views\\PersonalDetails\\DateOfBirth.cshtml", model);
        }

        [HttpPost]
       
        public async Task<ActionResult> CurrentAddress(TelephoneNumberViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return View("~\\Views\\PersonalDetails\\PersonalDetails.cshtml", model.PersonalDetailsViewModel);
            }
            
            if (_emailCollectionFeature.IsEnabled && (model.PersonalDetailsViewModel.OutcomeGroup.IsCoronaVirus || model.PersonalDetailsViewModel.OutcomeGroup.RequiresEmail))
            {
                return View("~\\Views\\PersonalDetails\\CollectEmailAddress.cshtml", Mapper.Map<TelephoneNumberViewModel, PersonalDetailViewModel>(model));
            }

            return await DirectToPopulatedCurrentAddressPicker(Mapper.Map<TelephoneNumberViewModel, PersonalDetailViewModel>(model));
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

            model = await PopulateChosenCurrentAddress(currentAddress, model);

            return View("~\\Views\\PersonalDetails\\CheckAtHome.cshtml", model);
        }

        [HttpPost]
        public async Task<ActionResult> SubmitChangeCurrentAddress(PersonalDetailViewModel model, string changeCurrentAddress)
        {
            switch (changeCurrentAddress)
            {
                case "changeCurrentPostcode" :
                    return View("~\\Views\\PersonalDetails\\CurrentAddress_ChangePostcode.cshtml", model);
                case "enterCurrentAddressManually":
                    return View("~\\Views\\PersonalDetails\\ManualAddress.cshtml", model);
                case "dontKnowCurrentAddress": 
                    return View("~\\Views\\PersonalDetails\\UnknownAddress.cshtml", model);
                default: return View("~\\Views\\PersonalDetails\\CurrentAddress_Change.cshtml", model);
            }
        }


        [HttpPost]
        public async Task<ActionResult> SubmitManualAddress(PersonalDetailViewModel model)
        {
            var postcodes = await GetPostcodeResults(model.AddressInformation.PatientCurrentAddress.Postcode);
            if (postcodes.ValidatedPostcodeResponse == PostcodeValidatorResponse.PostcodeNotFound)
            {
                ModelState.AddModelError("AddressInformation.PatientCurrentAddress.Postcode", new Exception());
            }

            if (!ModelState.IsValid)
            {
                return View("~\\Views\\PersonalDetails\\ManualAddress.cshtml", model);
            }

            return View("~\\Views\\PersonalDetails\\CheckAtHome.cshtml", model);

        }

        [HttpPost]
        public async Task<ActionResult> SubmitAtHome(PersonalDetailViewModel model)
        {
            if (!ModelState.IsValid || model.AddressInformation.HomeAddressSameAsCurrentWrapper == null)
            {
                ModelState.AddModelError("AddressInformation.HomeAddressSameAsCurrentWrapper.HomeAddressSameAsCurrent", new Exception());
                return View("~\\Views\\PersonalDetails\\CheckAtHome.cshtml", model);
            }

            if (model.AddressInformation.HomeAddressSameAsCurrentWrapper.HomeAddressSameAsCurrent == HomeAddressSameAsCurrent.Yes)
            {
                model.AddressInformation.PatientHomeAddress = model.AddressInformation.PatientCurrentAddress;

                return View("~\\Views\\PersonalDetails\\ConfirmDetails.cshtml", model);
            }

            if (model.AddressInformation.HomeAddressSameAsCurrentWrapper.HomeAddressSameAsCurrent == HomeAddressSameAsCurrent.No)
            {
                return View("~\\Views\\PersonalDetails\\HomeAddress_Postcode.cshtml", model);
            }

            return View("~\\Views\\PersonalDetails\\CheckAtHome.cshtml", model);
        }

        [HttpPost]
        public async Task<ActionResult> CollectEmailAddress(PersonalDetailViewModel model)
        {

            if (ModelState.IsValid)
            {
                return await DirectToPopulatedCurrentAddressPicker(model);
            }

            return View("~\\Views\\PersonalDetails\\CollectEmailAddress.cshtml", model);
        }
        
    }
}
