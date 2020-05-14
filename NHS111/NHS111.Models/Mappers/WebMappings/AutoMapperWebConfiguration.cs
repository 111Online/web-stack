using AutoMapper;
using NHS111.Models.Models.Web;
using NHS111.Models.Models.Web.Logging;

namespace NHS111.Models.Mappers.WebMappings
{
    public static class AutoMapperWebConfiguration
    {
        public static void Configure()
        {
            Mapper.Initialize(cfg =>
            {
                cfg.AddProfile(new JourneyViewModelMapper());
                cfg.AddProfile(new FromOutcomeViewModelToDosViewModel());
                cfg.AddProfile(new AddressInfoViewModelMapper());
                cfg.AddProfile(new FromOutcomeViewModelToSubmitEncounterToServiceRequest());
                cfg.AddProfile(new FromOutcomeVIewModelToITKDispatchRequest());
                cfg.AddProfile(new FromOutcomeViewModelToPersonalDetailViewModel());
                cfg.AddProfile(new FromDosCaseToDosServicesByClinicalTermRequest());
                cfg.AddProfile(new AuditedModelMappers());
                cfg.AddProfile(new FromSystemDayOfWeekToDosDayOfWeek());
                cfg.AddProfile(new FromServiceViewModelToRecommendedServiceViewModelMapper());
                cfg.AddProfile(new FromOutcomeViewModelToOtherServicesViewModel());
                cfg.AddProfile(new DataCaptureApiRequestMappings());
                cfg.AddProfile(new FromTelephoneNumberViewModelToPersonalDetailsViewModel());
                cfg.AddProfile(new FromDateOfBirthViewModelToPersonalDetailsViewModel());
            });
        }
    }
}