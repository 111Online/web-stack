﻿using log4net;
using NHS111.Business.DOS.Configuration;
using NHS111.Business.DOS.EndpointFilter;
using NHS111.Business.DOS.Service;

using NHS111.Features;
using NHS111.Models.Models.Business;
using NHS111.Models.Models.Web.Clock;
using NHS111.Utils.IoC;
using NHS111.Utils.Logging;
using NHS111.Utils.RestTools;
using RestSharp;
using StructureMap;
using StructureMap.Graph;

namespace NHS111.Business.DOS.IoC
{
    public class BusinessDosRegistry : Registry
    {
        public BusinessDosRegistry(IConfiguration configuration, ILog logger)
        {
            IncludeRegistry<UtilsRegistry>();
            For<IServiceAvailabilityManager>().Use<ServiceAvailablityManager>();
            For<IRestClient>().Use(new RestClient(configuration.CCGApiBaseUrl));
            For<IPublicHolidayService>().Use(new PublicHolidayService(
                PublicHolidaysDataService.GetPublicHolidays(configuration),
                new SystemClock()));
            For<IFilterServicesFeature>().Use<FilterServicesFeature>();
            Scan(scan =>
            {
                scan.TheCallingAssembly();
                scan.WithDefaultConventions();
            });
        }
    }
}
