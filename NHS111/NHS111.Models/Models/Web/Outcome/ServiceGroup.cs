using System.Collections.Generic;
using System.Linq;

namespace NHS111.Models.Models.Web.Outcome
{
    //mapping for dx to service group for different display options based on shared outcome group
    //but different service offerings.  When everything uses service first, outcome group can be
    //used for this or another relationship in neo would be the place for this info.
    public class ServiceGroup
    {
        public string Id { get; set; }

        public IEnumerable<string> Dispositions { get; set; }

        public static ServiceGroup EmergencyDepartment = new ServiceGroup { Dispositions = new[] { "Dx02", "Dx03", "Dx89" }, Id = "Emergency_Department" };

        public static ServiceGroup EmergencyDental = new ServiceGroup { Dispositions = new[] { "Dx118" }, Id = "Emergency_Department" };

        public static ServiceGroup EmergencyPrescription = new ServiceGroup { Dispositions = new[] { "Dx80", "Dx85", "Dx86", "Dx87" }, Id = "Emergency_Prescription" };

        public static ServiceGroup Default = new ServiceGroup { Id = "" };

        public static IEnumerable<ServiceGroup> ServiceGroups = new [] { EmergencyDepartment, EmergencyDental, EmergencyPrescription };

        public static ServiceGroup GetServiceGroupFromDisposition(string dispositionId)
        {
            var serviceGroup = ServiceGroups.FirstOrDefault(g => g.Dispositions.Contains(dispositionId));
            return serviceGroup == null ? Default : serviceGroup;
        }
    }
}
