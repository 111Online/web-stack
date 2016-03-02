using System.Threading.Tasks;
using System.Web.Mvc;
using NHS111.Models.Models.Web;
using NHS111.Models.Models.Web.FromExternalServices;
using NHS111.Utils.Attributes;
using NHS111.Web.Presentation.Builders;
using NHS111.Web.Presentation.Configuration;

namespace NHS111.Web.Controllers
{
    [LogHandleErrorForMVC]
    public class DOSController : Controller
    {
        private readonly IDOSBuilder _dosBuilder;

        public DOSController(IDOSBuilder dosBuilder)
        {
            _dosBuilder = dosBuilder;
        }

        [HttpPost]
        public async Task<ActionResult> FillServiceDetails(DosViewModel model)
        {
            return View("../DOS/Confirmation", await _dosBuilder.FillServiceDetailsBuilder(model));
        }
    }
}