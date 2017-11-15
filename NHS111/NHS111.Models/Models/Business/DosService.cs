using System;
using NHS111.Models.Models.Web.FromExternalServices;

namespace NHS111.Models.Models.Business
{
    public class DosService : Web.FromExternalServices.DosService
    {
        public bool CallbackEnabled { get; set; }
    }
}
