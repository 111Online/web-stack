namespace NHS111.Web.Presentation.ModelBinders {
    using System;
    using System.ComponentModel;
    using System.Web.Mvc;

    public class DirectLinkPersonalDetailsModelBinder : DefaultModelBinder
    {
        protected override void BindProperty(ControllerContext controllerContext, ModelBindingContext bindingContext, PropertyDescriptor propertyDescriptor)
        {
            if (propertyDescriptor.Name != "DateOfBirth") {
                base.BindProperty(controllerContext, bindingContext, propertyDescriptor);
                return;
            }
            var model = bindingContext.Model;
            var property = model.GetType().GetProperty(propertyDescriptor.Name);
            if (property == null)
                return;

            var value = bindingContext.ValueProvider.GetValue(propertyDescriptor.Name);

            if (value == null)
                return;

            var cultureInfo = new System.Globalization.CultureInfo("en-GB");
            var date = DateTime.Parse(value.AttemptedValue, cultureInfo);
            property.SetValue(model, date, null);
        }
    }
}