using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;

namespace NHS111.Web.Controllers
{
    public class MicroSurveyController : Controller
    {
        // GET: MicroSurvey
        public MicroSurveyController()
        {
            
        }

        [HttpGet]
        public async Task<JsonResult> GetQuestionJson(string questionId)
        {
            throw new NotImplementedException();
        }
    }
}