
namespace NHS111.Web.Presentation.ModelBinders {
    using System.ComponentModel;
    using System.Web.Mvc;
    using Newtonsoft.Json;
    using NHS111.Models.Models.Web;
    using NHS111.Models.Models.Web.FromExternalServices;

    public class JourneyViewModelBinder : DefaultModelBinder {
        protected override void BindProperty(ControllerContext controllerContext, ModelBindingContext bindingContext, PropertyDescriptor propertyDescriptor) {

            base.BindProperty(controllerContext, bindingContext, propertyDescriptor);

            if (propertyDescriptor.Name != "JourneyJson")
                return;

            var model = bindingContext.Model as JourneyViewModel;
            if (model == null)
                return;

            model.Journey = JsonConvert.DeserializeObject<Journey>(model.JourneyJson);
        }
    }
}
