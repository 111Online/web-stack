namespace NHS111.Web.Presentation.Builders {
    using Configuration;
    using NHS111.Models.Models.Web;

    public class AddressViewModelBuilder
        : IAddressViewModelBuilder {

        private IConfiguration _configuration;

        public AddressViewModelBuilder(IConfiguration configuration) {
            _configuration = configuration;
        }

        public OutcomeViewModel Build(OutcomeViewModel model) {
            model.AddressSearchViewModel = new AddressSearchViewModel() {
                PostcodeApiAddress = _configuration.PostcodeSearchByIdApiUrl,
                PostcodeApiSubscriptionKey = _configuration.PostcodeSubscriptionKey
            };
            return model;
        }

    }
}