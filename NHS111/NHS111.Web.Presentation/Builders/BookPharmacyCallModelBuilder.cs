using System;
using NHS111.Models.Models.Web;
using NHS111.Models.Models.Web.Enums;
using NHS111.Models.Models.Web.FromExternalServices;
using System.Collections.Generic;
using System.Linq;

namespace NHS111.Web.Presentation.Builders
{
    public class BookPharmacyCallModelBuilder
    {
        public static readonly int EmergencyNationalResponse_ServiceTypeId = 138;

        public static readonly string gp_phcas_click = "gp-phcas-click";
        public static readonly string gp_phcas_no_click = "gp-phcas-no-click";
        public static readonly string gp_nophcas = "gp-nophcas";
        public static readonly string ph_phcas_click = "ph-phcas-click";
        public static readonly string ph_phcas_no_click = "ph-phcas-no-click";
        public static readonly string ph_nophcas = "ph-nophcas";
        public static readonly string Dx06 = "Dx06";
        public static readonly string Dx07 = "Dx07";
        public static readonly string Dx08 = "Dx08"; 
        public static readonly string Dx09 = "Dx09";
        public static readonly string Dx10 = "Dx10";
        public static readonly string Dx14 = "Dx14";
        public static readonly string Dx15 = "Dx15";
        public static readonly string Dx16 = "Dx16";
        public static readonly string Dx28 = "Dx28";
        public static readonly string Dx75 = "Dx75";

        private static readonly IEnumerable<string> PharmacyServiceTypeOutcomes = new List<string>()
        {
            Dx06,
            Dx07,
            Dx08,
            Dx09,
            Dx10,
            Dx14,
            Dx15,
            Dx16,
            Dx28,
            Dx75
        };

        private static readonly Dictionary<string, string> PharmacyServiceTypeOutcomeWithNerFlagMap = new Dictionary<string, string>()
        {
            {
                Dx06, string.Empty
            },
            {
                Dx07, string.Empty
            },
            {
                Dx08, string.Empty
            },
            {
                Dx09, gp_phcas_no_click
            },
            {
                Dx10, gp_phcas_no_click
            },
            {
                Dx14, string.Empty
            },
            {
                Dx15, string.Empty
            },
            {
                Dx16, gp_phcas_no_click
            },
            {
                Dx28, ph_phcas_no_click
            },
            {
                Dx75, gp_phcas_no_click
            }
        };

        private static readonly Dictionary<string, string> PharmacyServiceTypeSelectedFlagMap = new Dictionary<string, string>()
        {
            {
                Dx06, gp_phcas_click
            },
            {
                Dx07, gp_phcas_click
            },
            {
                Dx08, gp_phcas_click
            },
            {
                Dx09, gp_phcas_click
            },
            {
                Dx10, gp_phcas_click
            },
            {
                Dx14, gp_phcas_click
            },
            {
                Dx15, gp_phcas_click
            },
            {
                Dx16, gp_phcas_click
            },
            {
                Dx28, ph_phcas_click
            },
            {
                Dx75, gp_phcas_click
            }
        };

        private static readonly Dictionary<string, string> PharmacyServiceTypeNotSelectedFlagMap = new Dictionary<string, string>()
        {
            {
                Dx06, gp_phcas_no_click
            },
            {
                Dx07, gp_phcas_no_click
            },
            {
                Dx08, gp_phcas_no_click
            },
            {
                Dx09, gp_phcas_no_click
            },
            {
                Dx10, gp_phcas_no_click
            },
            {
                Dx14, gp_phcas_no_click
            },
            {
                Dx15, gp_phcas_no_click
            },
            {
                Dx16, gp_phcas_no_click
            },
            {
                Dx28, ph_phcas_no_click
            },
            {
                Dx75, gp_phcas_no_click
            }
        };

        private static readonly Dictionary<string, string> PharmacyServiceTypeOutcomeWithoutNerFlagMap = new Dictionary<string, string>()
        {
            {
                Dx06, string.Empty
            },
            {
                Dx07, string.Empty
            },
            {
                Dx08, string.Empty
            },
            {
                Dx09, gp_nophcas
            },
            {
                Dx10, gp_nophcas
            },
            {
                Dx14, string.Empty
            },
            {
                Dx15, string.Empty
            },
            {
                Dx16, gp_nophcas
            },
            {
                Dx28, ph_nophcas
            },
            {
                Dx75, gp_nophcas
            }
        };

        public static string BookPharmacyCallValue(string id, long selectedServiceTypeId, List<ServiceViewModel> services, bool prePopulatesDosResults)
        {
            return IsPharmacyCallbackDispo(id)
                ? ChooseCallbackValue(id, selectedServiceTypeId, services, prePopulatesDosResults)
                : string.Empty;
        }

        private static bool IsPharmacyCallbackDispo(string id)
        {
            return PharmacyServiceTypeOutcomes.Contains(id);
        }

        private static string ChooseCallbackValue(string id, long selectedServiceTypeId, List<ServiceViewModel> services, bool prePopulatesDosResults)
        {
            if(!HasService(services) && !prePopulatesDosResults) return string.Empty;

            return HasEnrCasService(services)
                ? PharmacyServiceTypeOutcomeOfferingCASService(id, selectedServiceTypeId)
                : PharmacyServiceTypeOutcomeWithoutNerFlagMap[id];
        }

        private static bool HasService(List<ServiceViewModel> services)
        {
            var result = services
                .FindAll(s => s.ServiceType != null);
            return result.Count > 0;
        }

        private static bool HasEnrCasService(List<ServiceViewModel> services)
        {
            var result = services
                .FindAll(s => s.ServiceType != null)
                .FindAll(s => s.ServiceType.Id == EmergencyNationalResponse_ServiceTypeId)
                .FilterByOnlineDOSServiceType(OnlineDOSServiceType.Callback);
            return result.Count > 0;
        }

        private static string PharmacyServiceTypeOutcomeOfferingCASService(string id, long selectedServiceTypeId)
        {
            if (selectedServiceTypeId == -1) return PharmacyServiceTypeOutcomeWithNerFlagMap[id];

            return HasSelectedPharmacyCASServiceType(selectedServiceTypeId)
                ? PharmacyServiceTypeSelectedFlagMap[id]
                : PharmacyServiceTypeNotSelectedFlagMap[id];
        }

        private static bool HasSelectedPharmacyCASServiceType(long selectedServiceTypeId)
        {
            return selectedServiceTypeId.Equals(EmergencyNationalResponse_ServiceTypeId);
        }
    }
}