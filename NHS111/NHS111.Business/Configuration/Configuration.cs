using System;
using System.Collections.Generic;
using System.Configuration;
using System.IO;
using Nest;
using Newtonsoft.Json;

namespace NHS111.Business.Configuration
{
    public class Configuration : IConfiguration
    {
        public string GetDomainApiMonitorHealthUrl()
        {
            return GetDomainApiUrl("DomainApiMonitorHealthUrl");
        }

        public string GetDomainApiQuestionUrl(string questionId)
        {
            return GetDomainApiUrl("DomainApiQuestionUrl").
                Replace("{questionId}", questionId);
        }

        public string GetDomainApiAnswersForQuestionUrl(string questionId)
        {
            return GetDomainApiUrl("DomainApiAnswersForQuestionUrl").
                Replace("{questionId}", questionId);
        }

        public string GetDomainApiNextQuestionUrl(string questionId, string nodeLabel)
        {
            return GetDomainApiUrl("DomainApiNextQuestionUrl").
                Replace("{questionId}", questionId).Replace("{nodeLabel}", nodeLabel);
        }

        public string GetDomainApiFirstQuestionUrl(string pathwayId)
        {
            return GetDomainApiUrl("DomainApiFirstQuestionUrl").
                Replace("{pathwayId}", pathwayId);
        }

        public string GetDomainApiJustToBeSafeQuestionsFirstUrl(string pathwayId)
        {
            return GetDomainApiUrl("DomainApiJustToBeSafeQuestionsFirstUrl").
                Replace("{pathwayId}", pathwayId);
        }

        public string GetDomainApiJustToBeSafeQuestionsNextUrl(string pathwayId, IEnumerable<string> answeredQuestionIds, bool multipleChoice, string selectedQuestionId)
        {
            return GetDomainApiUrl("DomainApiJustToBeSafeQuestionsNextUrl").
                Replace("{pathwayId}", pathwayId).
                Replace("{answeredQuestionIds}", string.Join(",", answeredQuestionIds)).
                Replace("{multipleChoice}", multipleChoice.ToString()).
                Replace("{selectedQuestionId}", selectedQuestionId);
        }

        public string GetDomainApiPathwaysUrl(bool grouped, bool startingOnly)
        {
            return GetDomainApiUrl("DomainApiPathwaysUrl")
                .Replace("{grouped}", grouped.ToString())
                .Replace("{startingOnly}", startingOnly.ToString());
        }
        
        public string GetDomainApiGroupedPathwaysUrl(bool grouped, bool startingOnly)
        {
            return GetDomainApiUrl("DomainApiGroupedPathwaysUrl")
                .Replace("{grouped}", grouped.ToString())
                .Replace("{startingOnly}", startingOnly.ToString());
        }

        public string GetDomainApiPathwaysUrl(bool grouped, bool startingOnly, string gender, int age)
        {
            return GetDomainApiUrl("DomainApiPathwaysAgeGenderUrl")
                .Replace("{grouped}", grouped.ToString())
                .Replace("{startingOnly}", startingOnly.ToString())
                .Replace("{gender}", gender)
                .Replace("{age}", age.ToString());
        }

        public string GetDomainApiPathwayUrl(string pathwayId)
        {
            return GetDomainApiUrl("DomainApiPathwayUrl").
                Replace("{pathwayId}", pathwayId);
        }

        public string GetDomainApiPathwayMetadataUrl(string pathwayId)
        {
            return GetDomainApiUrl("DomainApiPathwayMetadataUrl").
                Replace("{pathwayId}", pathwayId);
        }

        public string GetDomainApiIdentifiedPathwayUrl(string pathwayNumbers, string gender, int age)
        {
            return GetDomainApiUrl("DomainApiIdentifiedPathwayUrl").
                Replace("{pathwayNumbers}",pathwayNumbers).
                Replace("{gender}", gender).
                Replace("{age}", age.ToString());
        }

        public string GetDomainApiIdentifiedPathwayFromTitleUrl(string pathwayTitle, string gender, int age)
        {
            return GetDomainApiUrl("DomainApiIdentifiedPathwayFromTitleUrl").
                Replace("{pathwayTitle}", pathwayTitle).
                Replace("{gender}", gender).
                Replace("{age}", age.ToString());
        }

        public string GetDomainApiPathwaySymptomGroup(string pathwayNumbers)
        {
            return GetDomainApiUrl("DomainApiPathwaySymptomGroup").
                Replace("{pathwayNumbers}", pathwayNumbers);
        }

        public string GetDomainApiPathwayNumbersUrl(string pathwayTitle)
        {
            return GetDomainApiUrl("DomainApiPathwayNumbersUrl").Replace("{pathwayTitle}", pathwayTitle);
        }

        public string GetDomainApiCareAdviceUrl(int age, string gender, IEnumerable<string> markers)
        {
            return GetDomainApiUrl("DomainApiCareAdviceUrl").
                Replace("{age}", age.ToString()).
                Replace("{gender}", gender).
                Replace("{markers}", string.Join(",", markers));
        }

        public string GetDomainApiCareAdviceUrl(string dxCode, string ageCategory, string gender) {
            return GetDomainApiUrl("DomainApiInterimCareAdviceUrl").
                Replace("{dxCode}", dxCode).
                Replace("{ageCategory}", ageCategory).
                Replace("{gender}", gender);
        }

        public string GetDomainApiListOutcomesUrl() {
            return GetDomainApiUrl("DomainApiListOutcomesUrl");
        }

        private static string GetDomainApiUrl(string key)
        {
            return ConfigurationManager.AppSettings[key].Replace("&amp;", "&");
        }

        public string GetRedisUrl()
        {
            return ConfigurationManager.AppSettings["RedisUrl"];
        }


        public string GetLocationPostcodebyGeoUrl(double longitude, double latitude)
        {
            return 
                ConfigurationManager.AppSettings["LocationPostcodebyGeoUrl"]
                    .Replace("{apiKey}", GetLocationApiKey())
                    .Replace("{longitude}", longitude.ToString())
                    .Replace("{latitude}", latitude.ToString())
                    .Replace("&amp;", "&");
        }

        public string GetLocationByPostcodeUrl(string postcode)
        {
            return
                ConfigurationManager.AppSettings["LocationByPostcodeUrl"]
                    .Replace("{apiKey}", GetLocationApiKey())
                    .Replace("{postcode}", postcode);

        }

        public string GetCCGBaseUrl()
        {
            return
                ConfigurationManager.AppSettings["CCGApiBaseUrl"];
        }

        public string CCGBusinessApiGetCCGUrl(string postcode)
        {
            return
                ConfigurationManager.AppSettings["CCGApiGetCCGByPostcodeUrl"].Replace("{postcode}", postcode);
        }

        public string GetLocationBaseUrl()
        {
            return ConfigurationManager.AppSettings["LocationBaseUrl"];
        }

        public string GetDomainApiBaseUrl()
        {
            return ConfigurationManager.AppSettings["DomainApiBaseUrl"];
        }

        public string GetLocationApiKey()
        {
            return ConfigurationManager.AppSettings["LocationApiKey"];
        }


        public string GetDomainApiSymptomDisciminatorUrl(string symptomDiscriminatorCode)
        {
            return GetDomainApiUrl("DomainApiSymptomDiscriminatorCodeUrl").Replace("{SymptomDiscriminatorCodeId}", symptomDiscriminatorCode);
        }

        public string GetCategoriesWithPathwaysUrl() {
            return GetDomainApiUrl("DomainApiGetCategoriesWithPathwaysUrl");
        }


        public string GetCategoriesWithPathwaysUrl(string gender, int age)
        {
            return GetDomainApiUrl("DomainApiGetCategoriesWithPathwaysAgeGenderUrl")
                .Replace("{gender}", gender)
                .Replace("{age}", age.ToString());
        }

        public ConnectionSettings GetElasticClientSettings()
        {
            return new ConnectionSettings(new Uri(ConfigurationManager.AppSettings["PathwayElasticSearchUrl"]));
        }

        public IElasticClient GetElasticClient()
        {
            return new ElasticClient(GetElasticClientSettings().DisableDirectStreaming());
        }

        public string GetDomainApiVersionUrl()
        {
            return GetDomainApiUrl("DomainApiGetVersionUrl");
        }

        public string GetDomainApiPathwayJourneyUrl()
        {
            return GetDomainApiUrl("FullPathwayJourneyUrl");
        }


        public string GetDomainApiPathwayJourneyUrl(string startingPathwayUrl, string dispositionCode)
        {
            return GetDomainApiUrl("PathwayJourneyUrl") + "/" + startingPathwayUrl + "/" + dispositionCode;
        }
    }

    public interface IConfiguration
    {
        string GetDomainApiBaseUrl();
        string GetDomainApiMonitorHealthUrl();

        /* Questions */
        string GetDomainApiQuestionUrl(string questionId);
        string GetDomainApiAnswersForQuestionUrl(string questionId);
        string GetDomainApiNextQuestionUrl(string questionId, string nodeLabel);
        string GetDomainApiFirstQuestionUrl(string pathwayId);
        string GetDomainApiJustToBeSafeQuestionsFirstUrl(string pathwayId);
        string GetDomainApiJustToBeSafeQuestionsNextUrl(string pathwayId, IEnumerable<string> answeredQuestionIds, bool multipleChoice, string selectedQuestionId);

        string GetDomainApiPathwayJourneyUrl();
        string GetDomainApiPathwayJourneyUrl(string startingPathwayUrl, string dispositionCode);

        /* Pathways */
        string GetDomainApiPathwaysUrl(bool grouped, bool startingOnly);
        string GetDomainApiGroupedPathwaysUrl(bool grouped, bool startingOnly);
        string GetDomainApiPathwaysUrl(bool grouped, bool startingOnly, string gender, int age);
        string GetDomainApiPathwayUrl(string pathwayId);
        string GetDomainApiPathwayMetadataUrl(string pathwayId);
        string GetDomainApiIdentifiedPathwayUrl(string pathwayNumbers, string gender, int age);
        string GetDomainApiIdentifiedPathwayFromTitleUrl(string pathwayTitle, string gender, int age);
        string GetDomainApiPathwaySymptomGroup(string pathwayNumbers);
        string GetDomainApiPathwayNumbersUrl(string pathwayTitle);

        /* Care Advice */
        string GetDomainApiCareAdviceUrl(int age, string gender, IEnumerable<string> markers);
        string GetDomainApiCareAdviceUrl(string dxCode, string ageCategory, string gender);

        /* Outcomes */
        string GetDomainApiListOutcomesUrl();

        string GetRedisUrl();

        /* Symptom disciminator */
        string GetDomainApiSymptomDisciminatorUrl(string symptomDiscriminatorCode);

        /*Categories*/
        string GetCategoriesWithPathwaysUrl();
        string GetCategoriesWithPathwaysUrl(string gender, int age);

        /*Pathways Search */
        ConnectionSettings GetElasticClientSettings();
        IElasticClient GetElasticClient();

        /*Location*/
        string GetLocationBaseUrl();
        string GetLocationPostcodebyGeoUrl(double longitude, double latitude);
        string GetLocationByPostcodeUrl(string postcode);

        /* CCG */
        string GetCCGBaseUrl();
        string CCGBusinessApiGetCCGUrl(string postcode);

        /*Version*/
        string GetDomainApiVersionUrl();
    }
}