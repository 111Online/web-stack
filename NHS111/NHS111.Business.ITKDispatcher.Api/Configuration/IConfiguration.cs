using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NHS111.Web.Presentation.Models;

namespace NHS111.Business.ITKDispatcher.Api.Configuration
{
    public interface IConfiguration
    {
        string EsbEndpointUrl { get; }
    }
}
