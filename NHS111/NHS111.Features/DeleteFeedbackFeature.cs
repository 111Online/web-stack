using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NHS111.Features.Defaults;

namespace NHS111.Features
{
    public class DeleteFeedbackFeature : BaseFeature, IDeleteFeedbackFeature
    {
        public DeleteFeedbackFeature()
        {
            DefaultIsEnabledSettingStrategy = new DisabledByDefaultSettingStrategy();
        }
    }

    public interface IDeleteFeedbackFeature : IFeature { }
}
