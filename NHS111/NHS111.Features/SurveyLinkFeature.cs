using System;
using System.Collections.Generic;
using System.Linq;
using NHS111.Features.Defaults;

namespace NHS111.Features
{
    public class SurveyLinkFeature : BaseFeature, ISurveyLinkFeature
    {
        public SurveyLinkFeature()
        {
            DefaultIsEnabledSettingStrategy = new EnabledByDefaultSettingStrategy();
        }

        public string BaseUrl
        {
            get { return FeatureValue("BaseUrl").Value; }
        }

        public string SurveyId
        {
            get { return FeatureValue("SurveyId").Value; }
        }
    }

    public interface ISurveyLinkFeature : IFeature
    {
        string BaseUrl { get; }

        string SurveyId { get; }
    }
}
