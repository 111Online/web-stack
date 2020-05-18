using NHS111.Models.Models.Web.CCG;
using System.Threading.Tasks;

namespace NHS111.Business.DOS.WhiteListPopulator
{
    public interface IWhiteListPopulator
    {
        Task<ServiceListModel> PopulateCCGWhitelist(string postCode);
    }
}