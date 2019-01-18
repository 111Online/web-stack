﻿
namespace NHS111.Web.Presentation.Builders
{
    using System;
    using NHS111.Models.Models.Domain;
    using NHS111.Models.Models.Web;
    using NHS111.Models.Models.Web.FromExternalServices;
    using NHS111.Models.Models.Web.Validators;

    public interface IReferralResultBuilder {
        ReferralResultViewModel Build(OutcomeViewModel outcomeModel);
        ReferralResultViewModel BuildFailureResult(OutcomeViewModel outcomeModel);
        ReferralResultViewModel BuildDuplicateResult(OutcomeViewModel outcomeModel);
        ReferralResultViewModel BuildConfirmationResult(OutcomeViewModel outcomeModel);
        ServiceUnavailableReferralResultViewModel BuildServiceUnavailableResult(OutcomeViewModel outcomeModel, DosCheckCapacitySummaryResult dosResult);
    }

    public class ReferralResultBuilder
        : IReferralResultBuilder {

        public ReferralResultBuilder(IPostCodeAllowedValidator postCodeAllowedValidator) {
            if (postCodeAllowedValidator == null)
                throw new ArgumentNullException("postCodeAllowedValidator");
            _postCodeAllowedValidator = postCodeAllowedValidator;
        }

        public ReferralResultViewModel Build(OutcomeViewModel outcomeModel) {
            if (outcomeModel == null)
                throw new ArgumentNullException("outcomeModel");

            if (outcomeModel.HasAcceptedCallbackOffer.HasValue && outcomeModel.HasAcceptedCallbackOffer.Value)
                outcomeModel.WaitTimeText = _dx334WaitTimeText; //todo data drive the 334 outcome

            if (outcomeModel.ItkSendSuccess.HasValue && outcomeModel.ItkSendSuccess.Value) {
                return BuildConfirmationResult(outcomeModel);
            }

            if (outcomeModel.ItkDuplicate.HasValue && outcomeModel.ItkDuplicate.Value) {
                return BuildDuplicateResult(outcomeModel);
            }

            return BuildFailureResult(outcomeModel);
        }

        public ReferralResultViewModel BuildFailureResult(OutcomeViewModel outcomeModel) {
            if (outcomeModel == null)
                throw new ArgumentNullException("outcomeModel");

            if (outcomeModel.OutcomeGroup != null) {
                if (outcomeModel.OutcomeGroup.Is999NonUrgent)
                    return new Call999ReferralFailureResultViewModel(outcomeModel);

                if (outcomeModel.OutcomeGroup.IsEDCallback)
                    return new AccidentAndEmergencyReferralFailureResultViewModel(outcomeModel);
            }

            return new ReferralFailureResultViewModel(outcomeModel);
        }

        public ReferralResultViewModel BuildDuplicateResult(OutcomeViewModel outcomeModel) {
            if (outcomeModel == null)
                throw new ArgumentNullException("outcomeModel");

            if (outcomeModel.OutcomeGroup != null) {
                if (outcomeModel.OutcomeGroup.Is999NonUrgent)
                    return new Call999DuplicateReferralResultViewModel(outcomeModel);

                if (outcomeModel.OutcomeGroup.IsEDCallback)
                    return new AccidentAndEmergencyDuplicateReferralResultViewModel(outcomeModel);
            }

            return new DuplicateReferralResultViewModel(outcomeModel);
        }

        public ReferralResultViewModel BuildConfirmationResult(OutcomeViewModel outcomeModel) {
            if (outcomeModel == null)
                throw new ArgumentNullException("outcomeModel");

            if (outcomeModel.OutcomeGroup != null) {
                if (outcomeModel.OutcomeGroup.Is999NonUrgent)
                    return new Call999ReferralConfirmationResultViewModel(outcomeModel);

                if (outcomeModel.OutcomeGroup.IsEDCallback)
                    return new AccidentAndEmergencyReferralConfirmationResultViewModel(outcomeModel);
            }

            return new ReferralConfirmationResultViewModel(outcomeModel);
        }

        public ServiceUnavailableReferralResultViewModel BuildServiceUnavailableResult(OutcomeViewModel outcomeModel, DosCheckCapacitySummaryResult dosResult) {
            if (outcomeModel == null)
                throw new ArgumentNullException("outcomeModel");

            var result = new ServiceUnavailableReferralResultViewModel(outcomeModel);
            if (outcomeModel.OutcomeGroup != null) {
                if (outcomeModel.OutcomeGroup.Is999NonUrgent)
                    result = new Call999ServiceUnavailableReferralResultViewModel(outcomeModel);
                if (outcomeModel.OutcomeGroup.IsEDCallback)
                    result = new AccidentAndEmergencyServiceUnavailableReferralResultViewModel(outcomeModel);
            }

            result.OutcomeModel = outcomeModel;
            outcomeModel.UnavailableSelectedService = outcomeModel.SelectedService;
            outcomeModel.DosCheckCapacitySummaryResult = dosResult;
            outcomeModel.DosCheckCapacitySummaryResult.ServicesUnavailable = dosResult.ResultListEmpty;
            outcomeModel.UserInfo.CurrentAddress.IsInPilotArea = _postCodeAllowedValidator.IsAllowedPostcode(outcomeModel.CurrentPostcode) == PostcodeValidatorResponse.InPathwaysArea;
            return result;
        }

        private readonly IPostCodeAllowedValidator _postCodeAllowedValidator;
        private readonly string _dx334WaitTimeText = "30 minutes";
    }
}