using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NHS111.Features.Defaults;

namespace NHS111.Features
{
    public class AllowedPostcodeFeature : BaseFeature, IAllowedPostcodeFeature
    {

        public AllowedPostcodeFeature()
        {
            DefaultIsEnabledSettingStrategy = new EnabledByDefaultSettingStrategy();
        }

        public string PostcodeFilePath
        {
            get { return FeatureValue(new PostcodeFilePathDefaultSettingStrategy(), "PostcodeFilePath").Value; }
        }
    }

    public interface IAllowedPostcodeFeature : IFeature
    {
        string PostcodeFilePath { get; }
    }
}