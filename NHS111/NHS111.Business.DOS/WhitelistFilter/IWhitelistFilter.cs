using System.Collections.Generic;
using System.Threading.Tasks;
using BusinessModels = NHS111.Models.Models.Business;

namespace NHS111.Business.DOS.WhitelistFilter
{
    public interface IWhitelistFilter
    {
        Task<List<BusinessModels.DosService>> Filter(List<BusinessModels.DosService> resultsToFilter, string postCode);
    }
}
