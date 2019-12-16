
namespace NHS111.Models.Mappers.WebMappings {
    using AutoMapper;
    using Models.Web.FromExternalServices;

    public class ServiceTypeDescriptionMapper
        : Profile {

        protected override void Configure() {

            Mapper.CreateMap<ServiceType, ServiceType>()
                .ForMember(src => src.Id, opt => opt.MapFrom(dest => dest.Id))
                .ForMember(src => src.Name, opt => opt.MapFrom(dest => dest.Name))
                .ForMember(src => src.Description, opt => opt.MapFrom(dest => ResolveDescription(dest.Id)));
        }

        private string ResolveDescription(long serviceTypeId) {
            switch (serviceTypeId) {
                case (long)ServiceTypes.AcuteAssessmentUnit:
                    return "This is where patients have symptoms urgently checked by senior nurses or consultants. You get a diagnosis and you’ll either be sent home that day or admitted to a ward in the hospital. It is not the same as A&E, but usually a part of it.";
                case (long)ServiceTypes.CommunityHospital:
                    return "Community hospitals offer things like blood tests, minor injuries, sexual health services, dental services, therapy and rehabilitation and child health.";
                case (long)ServiceTypes.DomiciliaryDentist:
                    return "This dentist will come to your home or a care home.";
                case (long)ServiceTypes.DentalEmergency:
                    return "Emergency dentists treat serious bleeding and swelling in the mouth.";
                //case ServiceTypes.DentalServices:
                case (long)ServiceTypes.EmergencyDepartment:
                    return "A&E is for critical or life-threatening injuries or illnesses. Also known as the emergency department or casualty.";
                case (long)ServiceTypes.EyeCasualty:
                    return "This hospital offers specialist emergency services for urgent eye problems.";
                case (long)ServiceTypes.SpecialistService:
                    return "A&E is for critical or life-threatening injuries or illnesses. Also known as the emergency department or casualty.";
                case (long)ServiceTypes.GPAccessHub:
                    return "Local GP centres offer extra daytime, evening and weekend GP appointments in the local area, even if you're registered with another GP.";
                case (long)ServiceTypes.GPChoice:
                    return "By registering with GP Choice you can see a GP outside your home catchment area, for example, near work.";
                case (long)ServiceTypes.GPOutofHoursProvider_OOH:
                    return "This is a GP service available outside normal working hours (out of hours) and over weekends and public holidays.";
                //case ServiceTypes.GPinHours:
                case (long)ServiceTypes.GPExtendedHours:
                    return "This GP surgery offers longer opening times, such as phone consultations later in the day.";
                //case ServiceTypes.IntegratedUrgentCareClinicalAssessmentService_IUCCAS:
                //case ServiceTypes.NHS111Provider:
                case (long)ServiceTypes.MentalHealth:
                    return "This is a service with trained mental health specialists. CAMHS are children and adolescent mental health services.";
                //case ServiceTypes.Midwifery:
                case (long)ServiceTypes.MinorInjuryUnit_MIU:
                    return "Minor injury units (also called urgent treatment centres) are open for at least 12 hours every day. They can diagnose and deal with many of the most common problems people go to A&E for.";
                //case ServiceTypes.Optician:
                case (long)ServiceTypes.DomiciliaryOptician:
                    return "This is an optician that can see you in your home or care home.";
                case (long)ServiceTypes.Optician:
                    return "This is a local optician who works with the NHS and can provide a higher level of investigation.";
                case (long)ServiceTypes.Pharmacist:
                    return "A pharmacist can help with a wide range of minor health problems. You can have a consultation in a private room. Pharmacists can provide emergency prescriptions for some medicines that you're prescribed regularly.";
                //case ServiceTypes.Pharmac Clinical Assessment Service(CAS)
                //case ServiceTypes.Pharmacy Distance Selling
                case (long)ServiceTypes.PharmacistEnhancedService:
                    return "A pharmacist can help with a wide range of minor health problems. You can have a consultation in a private room. Pharmacists can provide emergency prescriptions for some medicines that you're prescribed regularly.";
                case (long)ServiceTypes.PharmacistExtendedHours:
                    return "A pharmacist can help with a wide range of minor health problems. You can have a consultation in a private room. Pharmacists can provide emergency prescriptions for some medicines that you're prescribed regularly. This pharmacy is open for longer than usual.";
                case (long)ServiceTypes.PharmacistUrgentPrescription:
                    return "A pharmacist can help with a wide range of minor health problems. You can have a consultation in a private room. Pharmacists can provide emergency prescriptions for some medicines that you're prescribed regularly.";
                case (long)ServiceTypes.SexualHealth:
                    return "Sexual health services provide contraception, testing and treatment for sexually transmitted infections (STIs), pregnancy advice and testing and help with sexual problems. Sexual assault referral centres offer confidential help and medical care.";
                //case ServiceTypes.SpecialistService:
                case (long)ServiceTypes.UrgentCareCentre_UCC:
                    return "Urgent care centres (sometimes called walk-in centres) are staffed by nurses who can deal with a range of problems like rashes, minor injuries, emergency contraception, infections, sprains, cuts and bruises, and wound dressing.";
                case (long)ServiceTypes.UrgentTreatmentCentre_UTC:
                    return "Urgent treatment centres (also called minor injury units) are staffed by GPs. They’re open for at least 12 hours every day. They can diagnose and deal with many of the most common problems people go to A&E for.";
                case (long)ServiceTypes.WalkInCentre_WIC:
                    return "Walk-in centres (sometimes called urgent care centres)  can deal with a range of problems like rashes, minor injuries, emergency contraception, infections, sprains, cuts and bruises, and wound dressing. You don't need an appointment.";
                default:
                    return null;
            }
        }

        public enum ServiceTypes {
            AcuteAssessmentUnit = 41,
            AcuteHospital_Capacity = 114,
            AmbulanceService = 5,
            Burns_B = 109,
            CareHome = 24,
            Clinics	= 17,
            CommissioningOrganisation = 22,
            CommunityBasedServices = 20,
            CommunityHospital = 28,
            Council	= 3,
            CriticalCare_CC = 107,
            DentalEmergency = 47,
            DentalServices = 12,
            DistrictCommunityNurse = 38,
            DomiciliaryDentist = 124,
            DomiciliaryOptician = 125,
            DoSRegion = -1,
            EmergencyCarePractitioner_ECP = 31,
            EmergencyDepartment = 40,
            EyeCasualty = 120,
            GPAccessHub	= 136,
            GPChoice = 117,
            GPExtendedHours	= 123,
            GPinHours = 100,
            GPOutofHoursProvider_OOH = 25,
            GPLedUCCwithED = 106,
            HCPCallback = 164,
            HealthInformation = 113,
            HealthVisitor = 18,
            HyperAcuteStrokeUnit_HASU = 122,
            InpatientGeneralWard = 42,
            IntegratedUrgentCareClinicalAssessmentService_IUCCAS = 133,
            IntermediateCare = 27,
            LocalTemplates = -2,
            MaternityAndNeonate_MN = 110,
            MentalHealth = 7,
            Midwifery = 19,
            MinorEyeConditionService_MECS = 112,
            MinorInjuryUnit_MIU	= 35,
            MultiDisciplinaryServices = 49,
            NHSTrust = 15,
            NHS111Provider = 130,
            Optician = 14,
            OrganisationalCluster = 6,
            Paediatrics_PDR = 111,
            PaediatricsIntensiveCareUnit_PICU = 108,
            PalliativeCare = 50,
            Pharmacist = 13,
            PharmacistExtendedHours = 116,
            PharmacistDistanceSelling = 134,
            PharmacistEnhancedService = 132,
            PharmacistUrgentPrescription = 131,
            PrimaryCarePractitioner_PCP = 32,
            PrimaryPercutaneousCoronaryIntervention_PPCI = 121,
            ProviderEscalationRAG_Capacity = 115,
            RetiredServices = 21,
            Safeguarding = 129,
            SexualHealth = 29,
            SinglePointofAccess_SPoA = 30,
            SocialCare = 8,
            Spare1 = 102,
            Spare2 = 9,
            SpecialistService = 48,
            SpecialityEmergencyDepartment_SpecED = 105,
            Therapist = 4,
            UrgentCareCentre_UCC	= 46,
            UrgentTreatmentCentre_UTC = 135,
            VoluntaryAgency = 11,
            WalkInCentre_WIC = 45,
            NewServiceType = 137
        }

}
}
