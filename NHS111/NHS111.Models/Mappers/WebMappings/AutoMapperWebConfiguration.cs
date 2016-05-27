using AutoMapper;

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
                cfg.AddProfile(new FromPafToAddressInfo());
                cfg.AddProfile(new FromOutcomeViewModelToSubmitEncounterToServiceRequest());
                cfg.AddProfile(new FromOutcomeVIewModelToITKDispatchRequest());
                cfg.AddProfile(new FromDosCaseToDosServicesByClinicalTermRequest());
            });
        }
    }
}