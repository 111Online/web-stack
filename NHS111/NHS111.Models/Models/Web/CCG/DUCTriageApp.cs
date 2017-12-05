using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NHS111.Models.Models.Web.CCG
{
    public class DUCTriageApp
    {
        public static DUCTriageApp Pathways = new DUCTriageApp { Name = "Pathways" };
        public static DUCTriageApp Babylon = new DUCTriageApp { Name = "Babylon" };
        public static DUCTriageApp Sensely = new DUCTriageApp { Name = "Ask NHS" };
        public static DUCTriageApp Expert24 = new DUCTriageApp { Name = "Expert24" };

        public string Name { get; private set; }

        public static bool IsSupported(string ccgApp)
        {
            return !string.IsNullOrEmpty(ccgApp) && _supportedApps.Any(a => a.Name.ToLower() == ccgApp.ToLower());
        }

        public static bool IsPathways(string ccgApp)
        {
            return !string.IsNullOrEmpty(ccgApp) && (ccgApp.ToLower().Equals(Pathways.Name.ToLower()));
        }

        private DUCTriageApp() { }
        private static readonly DUCTriageApp[] _supportedApps = { Pathways, Babylon, Sensely, Expert24 };
    }
}

