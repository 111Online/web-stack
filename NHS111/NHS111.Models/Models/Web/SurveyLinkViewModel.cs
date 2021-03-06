﻿using System;
using System.Collections.Generic;
using System.Net;
using NHS111.Models.Models.Business.MicroSurvey;

namespace NHS111.Models.Models.Web
{
    public class SurveyLinkViewModel
    {
        public string SurveyId { get; set; }

        public string JourneyId { get; set; }

        public Guid SessionId { get; set; }

        public string PathwayNo { get; set; }

        public string DigitalTitle { get; set; }

        public string EndPathwayNo { get; set; }

        public string EndPathwayTitle { get; set; }

        public string DispositionCode { get; set; }

        public DateTime DispositionDateTime { get; set; }

        public string Campaign { get; set; }

        public string CampaignSource { get; set; }

        public int ServiceCount { get; set; }

        public string ServiceOptions { get; set; }

        public bool ValidationCallbackOffered { get; set; }

        public string RecommendedServiceType { get; set; }
        public int RecommendedServiceId { get; set; }

        private string _recommendedServiceName;
        public string RecommendedServiceName {
            //Decode Html encoded chars from DOS an let View renderer handle encoding itslef 
            get { return WebUtility.HtmlDecode(_recommendedServiceName); } 
            set { _recommendedServiceName = value; } 
        }

        public string DispositionChoiceReasoning { get; set; }

        public string SurveyUrl { get; set; }

        public string GuidedSelection { get; set; }
        
        public string BookPharmacyCall { get; set; }

        public List<ServiceViewModel> Services { get; set; }
        public string RecommendedServiceTypeAlias { get; set; }
        public string StartUrl { get; set; }
        public EmbeddedData EmbeddedData { get; set; }
    }
}
