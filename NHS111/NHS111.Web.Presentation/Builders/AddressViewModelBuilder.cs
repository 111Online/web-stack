namespace NHS111.Web.Presentation.Builders
{
    using Configuration;
    using NHS111.Models.Models.Web;

    public class AddressViewModelBuilder
        : IAddressViewModelBuilder
    {

        private readonly IConfiguration _configuration;

        public AddressViewModelBuilder(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        public AddressSearchViewModel Build(OutcomeViewModel model)
        {
            return new AddressSearchViewModel
            {
                PostcodeApiAddress = _configuration.PostcodeSearchByIdUrl,
                PostcodeApiSubscriptionKey = _configuration.PostcodeSubscriptionKey
            };
        }

    }
}