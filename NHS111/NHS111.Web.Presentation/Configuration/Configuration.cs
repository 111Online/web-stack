using System;
using System.Configuration;
using NHS111.Models.Models.Web.Enums;

namespace NHS111.Web.Presentation.Configuration
{
    public class Configuration : IConfiguration
    {
        public string ItkDispatcherApiBaseUrl { get { return ConfigurationManager.AppSettings["ItkDispatcherApiBaseUrl"]; } }
        public string ItkDispatcherApiSendItkMessageUrl { get { return ConfigurationManager.AppSettings["ItkDispatcherApiSendItkMessageUrl"]; } }
        public string GPSearchUrl { get { return ConfigurationManager.AppSettings["GPSearchUrl"]; } }
        public string GPSearchApiUrl { get { return ConfigurationManager.AppSettings["GPSearchApiUrl"]; } }
        public string GPSearchByIdUrl { get { return ConfigurationManager.AppSettings["GPSearchByIdUrl"]; } }
        public string BusinessApiProtocolandDomain { get { return ConfigurationManager.AppSettings["BusinessApiProtocolandDomain"]; } }

        public string CCGBusinessApiBaseProtocolandDomain { get { return ConfigurationManager.AppSettings["CCGApiBaseUrl"]; } }

        public string BusinessDosApiBaseUrl { get { return ConfigurationManager.AppSettings["BusinessDosApiBaseUrl"]; } }
        public string BusinessDosApiCheckCapacitySummaryUrl { get { return ConfigurationManager.AppSettings["BusinessDosApiCheckCapacitySummaryUrl"]; } }
        public string BusinessDosApiCheckServicesUrl { get { return ConfigurationManager.AppSettings["BusinessDosApiCheckServicesUrl"]; } }
        public string BusinessSlotApiGetUrl { get { return ConfigurationManager.AppSettings["BusinessSlotApiGetUrl"]; } }
        public string BusinessSlotApiBookUrl { get { return ConfigurationManager.AppSettings["BusinessSlotApiBookUrl"]; } }


        public string FeedbackApiBaseUrl { get { return ConfigurationManager.AppSettings["FeedbackApiBaseUrl"]; } }
        public string FeedbackAddFeedbackUrl { get { return ConfigurationManager.AppSettings["FeedbackAddFeedbackUrl"]; } }
        public string FeedbackDeleteFeedbackUrl { get { return ConfigurationManager.AppSettings["FeedbackDeleteFeedbackUrl"]; } }
        public string FeedbackAuthorization { get { return ConfigurationManager.AppSettings["FeedbackAuthorization"]; } }
        public string PostcodeApiBaseUrl { get { return ConfigurationManager.AppSettings["PostcodeApiBaseUrl"]; } }
        public string PostcodeSearchByIdUrl { get { return ConfigurationManager.AppSettings["PostcodeSearchByIdUrl"]; } }
        public string PostcodeSubscriptionKey { get { return ConfigurationManager.AppSettings["PostcodeSubscriptionKey"]; } }

        public string IntegrationApiItkDispatcher { get { return ConfigurationManager.AppSettings["IntegrationApiItkDispatcher"]; } }
        public string RedisConnectionString { get { return ConfigurationManager.AppSettings["RedisConnectionString"]; } }
        public string DOSWhitelist { get { return ConfigurationManager.AppSettings["DOSWhitelist"]; } }
        
        public string BusinessApiListOutcomesUrl { get { return ConfigurationManager.AppSettings["BusinessApiListOutcomesUrl"]; } }

        public string GoogleAnalyticsContainerId { get {return ConfigurationManager.AppSettings["GoogleAnalyticsContainerId "]; } }
        public string MapsApiUrl { get { return ConfigurationManager.AppSettings["MapsApiUrl"]; } }
        public string MapsApiKey { get { return ConfigurationManager.AppSettings["MapsApiKey"]; } }
        public string DosMobileBaseUrl { get { return ConfigurationManager.AppSettings["DOSMobileBaseUrl"]; } }
        public string DosMobileUsername { get { return ConfigurationManager.AppSettings["dos_mobile_credential_user"]; } }
        public string DosMobilePassword { get { return ConfigurationManager.AppSettings["dos_mobile_credential_password"]; } }

        public string QueryStringEncryptionKey { get { return ConfigurationManager.AppSettings["QueryStringEncryptionKey"]; } }
        public string QueryStringEncryptionBytes { get { return ConfigurationManager.AppSettings["QueryStringEncryptionBytes"]; } }
        public string Expert24Url { get { return ConfigurationManager.AppSettings["Expert24Url"]; } }

        public string BusinessApiLocationSearchGetAddressByGeoUrl { get{ return ConfigurationManager.AppSettings["BusinessApiLocationSearchGetAddressByGeoUrl"];} }
        public string BusinessApiLocationSearchGetAddressByPostcodeUrl { get { return ConfigurationManager.AppSettings["BusinessApiLocationSearchGetAddressByPostcodeUrl"]; } }
        public string BusinessApiLocationSearchGetAddressByUDPRNUrl { get { return ConfigurationManager.AppSettings["BusinessApiLocationSearchGetAddressByUDPRNUrl"]; } }
        public string BusinessApiLocationSearchGetAddressValidatedByPostcodeUrl { get { return ConfigurationManager.AppSettings["BusinessApiLocationSearchGetAddressValidatedByPostcodeUrl"]; } }
        public string GetBusinessApiGetAddressByGeoUrl(string latlong)
        {
            return string.Format(BusinessApiLocationSearchGetAddressByGeoUrl, latlong);
        }

        public string GetBusinessApiGetAddressByPostcodeUrl(string postcode)
        {
            return string.Format(BusinessApiLocationSearchGetAddressByPostcodeUrl, postcode);
        }

        public string GetBusinessApiGetAddressByUDPRNUrl(string udprn)
        {
            return string.Format(BusinessApiLocationSearchGetAddressByUDPRNUrl, udprn);
        }


        public string GetBusinessApiGetValidatedAddressByPostcodeUrl(string postcode)
        {
            return string.Format(BusinessApiLocationSearchGetAddressValidatedByPostcodeUrl, postcode);
        }
        public bool IsPublic {
            get {
                if (ConfigurationManager.AppSettings["IsPublic"] == null)
                    return true; //default to public if the setting isn't defined
                return ConfigurationManager.AppSettings["IsPublic"].ToLower() == "true";
            }
        }

        public bool SuggestStartingPathwaysOnly
        {
            get
            {
                if (ConfigurationManager.AppSettings["SuggestStartingPathwaysOnly"] == null)
                    return true; //default to only starting pathways if the setting isn't defined
                return ConfigurationManager.AppSettings["SuggestStartingPathwaysOnly"].ToLower() == "true";
            }
        }

        public string LoggingServiceApiBaseUrl { get { return ConfigurationManager.AppSettings["LoggingServiceApiBaseUrl"]; } }
        public string LoggingServiceApiAuditUrl { get { return ConfigurationManager.AppSettings["LoggingServiceApiAuditUrl"]; } }

        public string BusinessApiGetFullPathwayJourneyUrl
        {
            get { return ConfigurationManager.AppSettings["BusinessApiGetFullPathwayJourneyUrl"]; }
        }

        public string CCGBusinessApiGetCCGUrl(string postcode)
        {
            return String.Format(ConfigurationManager.AppSettings["CCGApiGetCCGByPostcodeUrl"], postcode);
        }

        public string CCGApiGetCCGDetailsByPostcode(string postcode)
        {
            return string.Format(ConfigurationManager.AppSettings["CCGApiGetCCGDetailsByPostcodeUrl"], postcode);
        }

        public string GetBusinessApiGetCategoriesWithPathways() { return GetBusinessApiUrlWithDomain("BusinessApiGetCategoriesWithPathways"); }
        public string GetBusinessApiPathwayMetadataUrl(string pathwayNo)
        {
            return string.Format(GetBusinessApiUrlWithDomain("BusinessApiPathwayMetadataUrl"), pathwayNo);
        }
        public string GetBusinessApiPathwaySearchUrl(string gender, string age, bool pathOnly = false)
        {
            return string.Format(GetBusinessApiUrlWithDomain("BusinessApiPathwaySearchUrl", pathOnly), gender, age);
        }

        public string GetBusinessApiGetCategoriesWithPathwaysGenderAge(string gender, int age, bool pathOnly = false)
        {
            return string.Format(GetBusinessApiUrlWithDomain("BusinessApiGetCategoriesWithPathwaysGenderAge", pathOnly), gender, age);
        }

        public string GetBusinessApiGetPathwaysGenderAge(string gender, int age) {
            return string.Format(GetBusinessApiUrlWithDomain("BusinessApiGetPathwaysGenderAge"), gender, age);
        }

        public string GetBusinessApiGroupedPathwaysUrl(string searchString)
        {
            return string.Format(GetBusinessApiUrlWithDomain("BusinessApiGroupedPathwaysUrl"), searchString, SuggestStartingPathwaysOnly);
        }

        public string GetBusinessApiGroupedPathwaysUrl(string searchString, string gender, int age, bool pathOnly = false)
        {
            return string.Format(GetBusinessApiUrlWithDomain("BusinessApiGroupedPathwaysGenderAgeUrl", pathOnly), searchString, SuggestStartingPathwaysOnly, gender, age);
        }

        public string GetBusinessApiPathwayUrl(string pathwayId, bool pathOnly = false)
        {
            return string.Format(GetBusinessApiUrlWithDomain("BusinessApiPathwayUrl", pathOnly), pathwayId);
        }

        public string GetBusinessApiPathwayIdUrl(string pathwayNumber, string gender, int age)
        {
            return string.Format(GetBusinessApiUrlWithDomain("BusinessApiPathwayIdUrl"), pathwayNumber, gender, age);
        }

        public string GetBusinessApiPathwaySymptomGroupUrl(string symptonGroups)
        {
            return string.Format(GetBusinessApiUrlWithDomain("BusinessApiPathwaySymptomGroupUrl"), symptonGroups);
        }

        public string GetBusinessApiNextNodeUrl(string pathwayId, NodeType currentNodeType, string journeyId, bool pathOnly = false)
        {
            return string.Format(GetBusinessApiUrlWithDomain("BusinessApiNextNodeUrl", pathOnly), pathwayId, currentNodeType, journeyId);
        }

        public string GetBusinessApiQuestionByIdUrl(string pathwayId, string questionId, bool pathOnly = false)
        {
            return string.Format(GetBusinessApiUrlWithDomain("BusinessApiQuestionByIdUrl", pathOnly), pathwayId, questionId);
        }

        public string GetBusinessApiCareAdviceUrl(int age, string gender, string careAdviceMarkers)
        {
            return string.Format(GetBusinessApiUrlWithDomain("BusinessApiCareAdviceUrl"), age, gender, careAdviceMarkers);
        }

        public string GetBusinessApiFirstQuestionUrl(string pathwayId, string state)
        {
            return string.Format(GetBusinessApiUrlWithDomain("BusinessApiFirstQuestionUrl"), pathwayId, state);
        }

        public string GetBusinessApiPathwayNumbersUrl(string pathwayTitle, bool pathOnly = false)
        {
            return string.Format(GetBusinessApiUrlWithDomain("BusinessApiPathwayNumbersUrl", pathOnly), pathwayTitle);
        }

        public string GetBusinessApiPathwayIdFromTitleUrl(string pathwayTitle, string gender, int age)
        {
            return string.Format(GetBusinessApiUrlWithDomain("BusinessApiPathwayIdFromTitleUrl"), pathwayTitle, gender, age);
        }

        public string GetBusinessApiJustToBeSafePartOneUrl(string pathwayId)
        {
            return string.Format(GetBusinessApiUrlWithDomain("BusinessApiJustToBeSafePartOneUrl"), pathwayId);
        }

       
        public string GetBusinessApiJustToBeSafePartTwoUrl(string pathwayId, string questionId, string jtbsQuestionIds, bool hasAnswwers)
        {
            return string.Format(GetBusinessApiUrlWithDomain("BusinessApiJustToBeSafePartTwoUrl"), pathwayId, questionId, jtbsQuestionIds,hasAnswwers);
        }

        public string GetBusinessApiInterimCareAdviceUrl(string dxCode, string ageGroup, string gender)
        {
            return string.Format(GetBusinessApiUrlWithDomain("BusinessApiInterimCareAdviceUrl"), dxCode, ageGroup, gender);
        }

        public string GetBusinessApiSymptomDiscriminatorUrl(string symptomDiscriminatorCode)
        {
            return string.Format(GetBusinessApiUrlWithDomain("BusinessApiSymptomDiscriminatorUrl"), symptomDiscriminatorCode);
        }

        public string GetBusinessApiListOutcomesUrl()
        {
            return GetBusinessApiUrlWithDomain("BusinessApiListOutcomesUrl");
        }

        public string GetBusinessApiVersionUrl(bool pathOnly = false)
        {
            return GetBusinessApiUrlWithDomain("BusinessApiVersionUrl", pathOnly);
        }

        private string GetBusinessApiUrlWithDomain(string endpointUrlkey, bool pathOnly=false)
        {
            var buinessEndpointconfigValue = ConfigurationManager.AppSettings[endpointUrlkey];
            if (pathOnly) return "/" + buinessEndpointconfigValue;
            return buinessEndpointconfigValue;
        }


        bool IsAbsoluteUrl(string url)
        {
            Uri result;
            return Uri.TryCreate(url, UriKind.Absolute, out result);
        }

    }



    public interface IConfiguration
    {
        string BusinessApiProtocolandDomain { get; }
        string ItkDispatcherApiSendItkMessageUrl { get; }
        string ItkDispatcherApiBaseUrl { get; }
        string GPSearchUrl { get; }
        string GPSearchApiUrl { get; }
        string GPSearchByIdUrl { get; }

        string CCGBusinessApiBaseProtocolandDomain { get; }

        string CCGBusinessApiGetCCGUrl(string postcode);
        string CCGApiGetCCGDetailsByPostcode(string postcode);
        string GetBusinessApiPathwayUrl(string pathwayId, bool pathOnly = false);
        string GetBusinessApiGroupedPathwaysUrl(string searchString);
        string GetBusinessApiGroupedPathwaysUrl(string searchString, string gender, int age, bool pathOnly = false);
        string GetBusinessApiPathwayIdUrl(string pathwayNumber, string gender, int age);
        string GetBusinessApiPathwaySymptomGroupUrl(string symptonGroups);
        string GetBusinessApiNextNodeUrl(string pathwayId, NodeType currentNodeType, string journeyId, bool pathOnly = false);
        string GetBusinessApiQuestionByIdUrl(string pathwayId, string questionId, bool pathOnly = false);
        string GetBusinessApiCareAdviceUrl(int age, string gender, string careAdviceMarkers);
        string GetBusinessApiFirstQuestionUrl(string pathwayId, string state);
        string GetBusinessApiPathwayNumbersUrl(string pathwayTitle, bool pathOnly = false);
        string GetBusinessApiPathwayIdFromTitleUrl(string pathwayTitle, string gender, int age);
        string GetBusinessApiJustToBeSafePartOneUrl(string pathwayId);
        string GetBusinessApiJustToBeSafePartTwoUrl(string pathwayId, string questionId, string jtbsQuestionIds,bool hasAnswwers);
        string GetBusinessApiInterimCareAdviceUrl(string dxCode, string ageGroup, string gender);
        string GetBusinessApiListOutcomesUrl();
        string GetBusinessApiSymptomDiscriminatorUrl(string symptomDiscriminatorCode);
        string GetBusinessApiGetCategoriesWithPathways();
        string GetBusinessApiGetCategoriesWithPathwaysGenderAge(string gender, int age, bool pathOnly = false);
        string GetBusinessApiGetPathwaysGenderAge(string gender, int age);
        string GetBusinessApiPathwayMetadataUrl(string pathwayNo);
        string GetBusinessApiPathwaySearchUrl(string gender, string age, bool pathOnly=false);
        string GetBusinessApiVersionUrl(bool pathOnly = false);
        string GetBusinessApiGetAddressByGeoUrl(string latlong);
        string GetBusinessApiGetAddressByPostcodeUrl(string postcode);
        string GetBusinessApiGetAddressByUDPRNUrl(string udprn);

        string GetBusinessApiGetValidatedAddressByPostcodeUrl(string postcode);
        string BusinessDosApiBaseUrl { get; }
        string BusinessDosApiCheckCapacitySummaryUrl { get; }
        string BusinessDosApiCheckServicesUrl { get; }
        string BusinessSlotApiGetUrl { get; }
        string BusinessSlotApiBookUrl { get; }
        string FeedbackApiBaseUrl { get; }
        string FeedbackAddFeedbackUrl { get; }
        string FeedbackDeleteFeedbackUrl { get; }
        string FeedbackAuthorization { get; }
        string PostcodeApiBaseUrl { get; }
        string PostcodeSearchByIdUrl { get; }
        string PostcodeSubscriptionKey { get; }
        string IntegrationApiItkDispatcher { get; }
        string RedisConnectionString { get; }
       string DOSWhitelist { get; }

        string BusinessApiListOutcomesUrl { get; }
        string BusinessApiGetFullPathwayJourneyUrl { get; }
        string GoogleAnalyticsContainerId { get; }
        string MapsApiUrl { get; }
        string MapsApiKey { get; }
        bool IsPublic { get; }
        string LoggingServiceApiBaseUrl { get; }
        string LoggingServiceApiAuditUrl { get; }
        string DosMobileBaseUrl { get; }
        string DosMobileUsername { get; }
        string DosMobilePassword { get; }

        string QueryStringEncryptionKey { get; }
        string QueryStringEncryptionBytes { get; }
        string Expert24Url { get; }
    }
}