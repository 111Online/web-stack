using System.Threading.Tasks;
using NHS111.Models.Models.Web.CCG;

namespace NHS111.Business.DOS.WhiteListPopulator
{
    public interface IWhiteListPopulator
    {
        Task<ServiceListModel> PopulateCCGWhitelist(string postCode);
    }
}