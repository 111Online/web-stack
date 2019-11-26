
namespace NHS111.Models.Models.Web.ITK {
    using System;

    public class DirectLinkPersonalDetails {
        public bool? IsFirstParty { get; set; }
        public string Firstname { get; set; }
        public string Lastname { get; set; }
        public string ThirdPartyFirstname { get; set; }
        public string ThirdPartyLastname { get; set; }
        public DateTime? DateOfBirth { get; set; }
        public string TelephoneNumber { get; set; }
        public bool? IsAtHome { get; set; }
        public string HomePostcode { get; set; }
        public bool? HomePostcodeIsKnown { get; set; }

        public bool HasAnyValue {
            get {
                return !string.IsNullOrEmpty(Firstname) ||
                       !string.IsNullOrEmpty(Lastname) ||
                       !string.IsNullOrEmpty(ThirdPartyFirstname) ||
                       !string.IsNullOrEmpty(ThirdPartyLastname) ||
                       DateOfBirth.HasValue ||
                       !string.IsNullOrEmpty(TelephoneNumber);
            }
        }

        public Web.InformantType GetInformantType() {
            if (!IsFirstParty.HasValue)
                return Web.InformantType.NotSpecified;

            return IsFirstParty.Value ? Web.InformantType.Self : Web.InformantType.ThirdParty;
        }

        public DirectLinkPersonalDetails() { }

        public bool IsValid() {
            if (!ValidateRequiredFields())
                return false;

            if (!ValidateParty())
                return false;

            if (!ValidateAtHome())
                return false;

            return true;
        }

        private bool ValidateAtHome() {
            if (IsAtHome.Value)
                return true;

            return !HomePostcodeIsKnown.Value || !string.IsNullOrEmpty(HomePostcode);
        }

        private bool ValidateRequiredFields() {
            AssignDefaults();

            if (!HasAnyValue)
                return true; //valid because no values have been supplied

            return !string.IsNullOrEmpty(Firstname) &&
                   !string.IsNullOrEmpty(Lastname) &&
                   (IsFirstParty.Value || 
                   !string.IsNullOrEmpty(ThirdPartyFirstname) && !string.IsNullOrEmpty(ThirdPartyLastname)) &&
                   DateOfBirth.HasValue &&
                   !string.IsNullOrEmpty(TelephoneNumber);
        }

        private void AssignDefaults() {
            if (!IsFirstParty.HasValue)
                IsFirstParty = true;

            if (!IsAtHome.HasValue)
                IsAtHome = true;

            if (!HomePostcodeIsKnown.HasValue)
                HomePostcodeIsKnown = false;
        }

        private bool ValidateParty() {

            if (string.IsNullOrEmpty(Firstname) || string.IsNullOrEmpty(Lastname))
                return false;

            if (IsFirstParty.Value)
                return true;

            return !string.IsNullOrEmpty(ThirdPartyFirstname) && !string.IsNullOrEmpty(ThirdPartyLastname);
        }

        public void ApplyTo(PersonalDetailViewModel personalDetailsViewModel) {
            personalDetailsViewModel.PatientInformantDetails = new PatientInformantViewModel {
                Informant = GetInformantType()
            };
            if (GetInformantType() == Web.InformantType.Self) {
                personalDetailsViewModel.PatientInformantDetails.SelfName = new PersonViewModel {
                    Forename = Firstname,
                    Surname = Lastname
                };
            } else if (GetInformantType() == Web.InformantType.ThirdParty) {
                personalDetailsViewModel.PatientInformantDetails.PatientName = new PersonViewModel {
                    Forename = ThirdPartyFirstname,
                    Surname = ThirdPartyLastname
                };
                personalDetailsViewModel.PatientInformantDetails.InformantName = new PersonViewModel {
                    Forename = Firstname,
                    Surname = Lastname
                };
            }

            if (personalDetailsViewModel.UserInfo == null)
                personalDetailsViewModel.UserInfo = new UserInfo();

            if (DateOfBirth.HasValue) {
                personalDetailsViewModel.UserInfo.Day = DateOfBirth.Value.Day;
                personalDetailsViewModel.UserInfo.Month = DateOfBirth.Value.Month;
                personalDetailsViewModel.UserInfo.Year = DateOfBirth.Value.Year;
            }

            personalDetailsViewModel.UserInfo.TelephoneNumber = TelephoneNumber;

            personalDetailsViewModel.AddressInformation.HomeAddressSameAsCurrentWrapper = new HomeAddressSameAsCurrentWrapper();
            if (IsAtHome.HasValue && IsAtHome.Value) { //is at home
                personalDetailsViewModel.AddressInformation.HomeAddressSameAsCurrentWrapper.HomeAddressSameAsCurrent
                    = HomeAddressSameAsCurrent.Yes;
                personalDetailsViewModel.AddressInformation.PatientHomeAddress = personalDetailsViewModel.AddressInformation.PatientCurrentAddress;
            } else { //isn't at home
                if (!HomePostcodeIsKnown.HasValue || !HomePostcodeIsKnown.Value) { //doesn't know home postcode
                    personalDetailsViewModel.AddressInformation.PatientHomeAddress = null;
                    personalDetailsViewModel.AddressInformation.HomeAddressSameAsCurrentWrapper
                            .HomeAddressSameAsCurrent =
                        HomeAddressSameAsCurrent.DontKnow;
                } else { //isn't at home and knows home postcode
                    personalDetailsViewModel.AddressInformation.HomeAddressSameAsCurrentWrapper.HomeAddressSameAsCurrent
                        = HomeAddressSameAsCurrent.No;

                    personalDetailsViewModel.AddressInformation.PatientHomeAddress.Postcode = HomePostcode;
                }
            }
        }
    }
}