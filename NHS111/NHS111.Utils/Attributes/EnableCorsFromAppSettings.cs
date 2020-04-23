using System;
using System.Configuration;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web.Cors;
using System.Web.Http.Cors;

namespace NHS111.Utils.Attributes
{
    [AttributeUsage(AttributeTargets.Method | AttributeTargets.Class, AllowMultiple = false)]
    public class EnableCorsAppSettingsAttribute : Attribute, ICorsPolicyProvider
    {
        private CorsPolicy _policy;

        public EnableCorsAppSettingsAttribute(string appSettingOriginKey)
        {
            _policy = new CorsPolicy
            {
                AllowAnyMethod = true,
                AllowAnyHeader = true
            };

            // loads the origins from AppSettings
            string originsString = ConfigurationManager.AppSettings[appSettingOriginKey];
            if (!String.IsNullOrEmpty(originsString))
            {
                foreach (var origin in originsString.Split(','))
                {
                    _policy.Origins.Add(origin);
                }
            }
        }

        public Task<CorsPolicy> GetCorsPolicyAsync(HttpRequestMessage request, System.Threading.CancellationToken cancellationToken)
        {
            return Task.FromResult(_policy);
        }
    }
}
