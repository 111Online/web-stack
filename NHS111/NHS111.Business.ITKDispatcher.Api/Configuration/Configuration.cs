using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Web;

namespace NHS111.Business.ITKDispatcher.Api.Configuration
{
    public class Configuration : IConfiguration
    {
        public string EsbEndpointUrl
        {
            get { return ConfigurationManager.AppSettings["EsbEndpointUrl"];}
        }
    }
}