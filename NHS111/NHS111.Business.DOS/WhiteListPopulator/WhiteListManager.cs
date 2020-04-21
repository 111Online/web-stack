using NHS111.Business.DOS.Configuration;
using NHS111.Business.DOS.DispositionMapper;
using NHS111.Utils.RestTools;

namespace NHS111.Business.DOS.WhiteListPopulator
{
    public class WhiteListManager : IWhiteListManager
    {
        private readonly IDispositionMapper _dispositionMapper;
        private readonly ILoggingRestClient _restCCGApi;
        private readonly IConfiguration _configuration;

        public WhiteListManager(IDispositionMapper dispositionMapper, ILoggingRestClient restClient, IConfiguration configuration)
        {
            _dispositionMapper = dispositionMapper;
            _restCCGApi = restClient;
            _configuration = configuration;
        }

        public IWhiteListPopulator GetWhiteListPopulator(int dxCode)
        {
            if (_dispositionMapper.IsRepeatPrescriptionDisposition(dxCode))
            {
                return new PharmacyReferralServicesWhiteListPopulator(_restCCGApi, _configuration);
            }
            else
            {
                return new ReferralServicesWhiteListPopulator(_restCCGApi, _configuration);
            }
        }
    }

    public interface IWhiteListManager
    {
        IWhiteListPopulator GetWhiteListPopulator(int dxCode);
    }
}
