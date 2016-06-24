
namespace NHS111.Web.Presentation.Builders {
    using NHS111.Models.Models.Web;

    public interface IAddressViewModelBuilder {
        OutcomeViewModel Build(OutcomeViewModel model);
    }
}