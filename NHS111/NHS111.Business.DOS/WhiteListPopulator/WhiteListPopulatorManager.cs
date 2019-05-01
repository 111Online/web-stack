using System;

namespace NHS111.Business.DOS.WhiteListPopulator
{
    public class WhiteListPopulatorManager: IWhiteListPopulatorManager
    {
        public IWhiteListPopulator GetWhiteListPopulator(int disposition)
        {
            throw new NotImplementedException();
        }
    }

    public interface IWhiteListPopulatorManager
    {
        IWhiteListPopulator GetWhiteListPopulator(int disposition);
    }
}
