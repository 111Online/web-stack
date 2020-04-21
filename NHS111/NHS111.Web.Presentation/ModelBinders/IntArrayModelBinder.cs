
namespace NHS111.Web.Presentation.ModelBinders
{
    using System.Linq;
    using System.Web.Mvc;

    public class IntArrayModelBinder
        : DefaultModelBinder
    {

        public override object BindModel(ControllerContext controllerContext, ModelBindingContext bindingContext)
        {
            var value = bindingContext.ValueProvider.GetValue(bindingContext.ModelName);
            if (value == null || string.IsNullOrEmpty(value.AttemptedValue))
            {
                return null;
            }

            return value
                .AttemptedValue
                .Split(',')
                .Select(int.Parse)
                .ToArray();
        }
    }
}
