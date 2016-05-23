using NHS111.Models.Models.Web;
using NHS111.Utils.Attributes;
using NHS111.Web.Presentation.Builders;
using System.Threading.Tasks;
using System.Web.Mvc;

namespace NHS111.Web.Tools.Controllers
{
    [LogHandleErrorForMVC]
    public class DoSController : Controller
    {
        private readonly IDOSBuilder _dosBuilder;

        public DoSController(IDOSBuilder dosBuilder)
        {
            _dosBuilder = dosBuilder;
        }

        [HttpGet]
        public ActionResult Search()
        {
            return View();
        }

        [HttpPost]
        public async Task<ActionResult> FillServiceDetails(DosViewModel model)
        {
            return View(await _dosBuilder.FillServiceDetailsBuilder(model));
        }
    }
}