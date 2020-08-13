using FluentValidation.Attributes;
using NHS111.Models.Models.Web.Validators;

namespace NHS111.Models.Models.Web
{

    [Validator(typeof(LocationViewModelValidator))]
    public class LocationViewModel : JourneyViewModel
    {

    }

    public class ProviderViewModel : LocationViewModel
    {
    }

    public class OutOfAreaViewModel : LocationViewModel
    {
    }
}
