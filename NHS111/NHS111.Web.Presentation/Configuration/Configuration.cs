using System;
using System.Configuration;

namespace NHS111.Web.Presentation.Configuration
{
    public class Configuration : IConfiguration
    {
        public string ItkDispatchApiUrl { get { return ConfigurationManager.AppSettings["ItkDispatchApiUrl"]; } }
        public string GPSearchUrl { get { return ConfigurationManager.AppSettings["GPSearchUrl"]; } }
        public string GPSearchApiUrl { get { return ConfigurationManager.AppSettings["GPSearchApiUrl"]; } }
        public string GPSearchByIdUrl { get { return ConfigurationManager.AppSettings["GPSearchByIdUrl"]; } }
        public string BusinessDosCheckCapacitySummaryUrl { get { return ConfigurationManager.AppSettings["BusinessDosCheckCapacitySummaryUrl"]; } }
        public string BusinessDosServicesByClinicalTermUrl { get { return ConfigurationManager.AppSettings["BusinessDosServicesByClinicalTermUrl"]; } }
        public string BusinessDosServiceDetailsByIdUrl { get { return ConfigurationManager.AppSettings["BusinessDosServiceDetailsByIdUrl"]; } }
        public string FeedbackAddFeedbackUrl { get { return ConfigurationManager.AppSettings["FeedbackAddFeedbackUrl"]; } }
        public string FeedbackAuthorization { get { return ConfigurationManager.AppSettings["FeedbackAuthorization"]; } }
        public string PostcodeSearchByIdApiUrl { get { return ConfigurationManager.AppSettings["PostcodeSearchByIdApiUrl"]; } }
        public string PostcodeSubscriptionKey { get { return ConfigurationManager.AppSettings["PostcodeSubscriptionKey"]; } }

        public string IntegrationApiItkDispatcher { get { return ConfigurationManager.AppSettings["IntegrationApiItkDispatcher"]; } }
        public string RedisConnectionString { get { return ConfigurationManager.AppSettings["RedisConnectionString"]; } }
        public string DosUsername { get { return ConfigurationManager.AppSettings["dos_credential_user"]; } }
        public string DosPassword { get { return ConfigurationManager.AppSettings["dos_credential_password"]; } }
       
        public string DOSMobileBaseUrl { get { return ConfigurationManager.AppSettings["DOSMobileBaseUrl"]; } }
        public string DOSMobileUsername { get { return ConfigurationManager.AppSettings["dos_mobile_credential_user"]; } }
        public string DOSMobilePassword { get { return ConfigurationManager.AppSettings["dos_mobile_credential_password"]; } }

        public string BusinessApiListOutcomesUrl { get { return ConfigurationManager.AppSettings["BusinessApiListOutcomesUrl"]; } }



        public string GetBusinessApiGroupedPathwaysUrl(string searchString)
        {
            return string.Format(GetBusinessApiUrlWithDomain("BusinessApiGroupedPathwaysUrl"), searchString);
        }

        public string GetBusinessApiPathwayUrl(string pathwayId)
        {
            return string.Format(GetBusinessApiUrlWithDomain("BusinessApiPathwayUrl"), pathwayId);
        }

        public string GetBusinessApiPathwayIdUrl(string pathwayNumber, string gender, int age)
        {
            return string.Format(GetBusinessApiUrlWithDomain("BusinessApiPathwayIdUrl"), pathwayNumber, gender, age);
        }

        public string GetBusinessApiPathwaySymptomGroupUrl(string symptonGroups)
        {
            return string.Format(GetBusinessApiUrlWithDomain("BusinessApiPathwaySymptomGroupUrl"), symptonGroups);
        }

        public string GetBusinessApiNextNodeUrl(string pathwayId, string journeyId, string state)
        {
            return string.Format(GetBusinessApiUrlWithDomain("BusinessApiNextNodeUrl"), pathwayId, journeyId, state);
        }

        public string GetBusinessApiQuestionByIdUrl(string pathwayId, string questionId)
        {
            return string.Format(GetBusinessApiUrlWithDomain("BusinessApiQuestionByIdUrl"), pathwayId, questionId);
        }

        public string GetBusinessApiCareAdviceUrl(int age, string gender, string careAdviceMarkers)
        {
            return string.Format(GetBusinessApiUrlWithDomain("BusinessApiCareAdviceUrl"), age, gender, careAdviceMarkers);
        }

        public string GetBusinessApiFirstQuestionUrl(string pathwayId, string state)
        {
            return string.Format(GetBusinessApiUrlWithDomain("BusinessApiFirstQuestionUrl"), pathwayId, state);
        }

        public string GetBusinessApiPathwayNumbersUrl(string pathwayTitle)
        {
            return string.Format(GetBusinessApiUrlWithDomain("BusinessApiPathwayNumbersUrl"), pathwayTitle);
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

        public string GetBusinessApiListOutcomesUrl()
        {
            return GetBusinessApiUrlWithDomain("BusinessApiListOutcomesUrl");
        }

        private string GetBusinessApiUrlWithDomain(string endpointUrlkey)
        {
            var businessApiDomain = ConfigurationManager.AppSettings["BusinessApiProtocolandDomain"];
            var buinessEndpointconfigValue = ConfigurationManager.AppSettings[endpointUrlkey];
            if (IsAbsoluteUrl(buinessEndpointconfigValue))
            {
                return buinessEndpointconfigValue;
            }
            return businessApiDomain + buinessEndpointconfigValue;
        }


        bool IsAbsoluteUrl(string url)
        {
            Uri result;
            return Uri.TryCreate(url, UriKind.Absolute, out result);
        }

    }



    public interface IConfiguration
    {
        string ItkDispatchApiUrl { get; }
        string GPSearchUrl { get; }
        string GPSearchApiUrl { get; }
        string GPSearchByIdUrl { get; }
        string GetBusinessApiPathwayUrl(string pathwayId);
        string GetBusinessApiGroupedPathwaysUrl(string searchString);
        string GetBusinessApiPathwayIdUrl(string pathwayNumber, string gender, int age);
        string GetBusinessApiPathwaySymptomGroupUrl(string symptonGroups);
        string GetBusinessApiNextNodeUrl(string pathwayId, string journeyId, string state);
        string GetBusinessApiQuestionByIdUrl(string pathwayId, string questionId);
        string GetBusinessApiCareAdviceUrl(int age, string gender, string careAdviceMarkers);
        string GetBusinessApiFirstQuestionUrl(string pathwayId, string state);
        string GetBusinessApiPathwayNumbersUrl(string pathwayTitle);
        string GetBusinessApiPathwayIdFromTitleUrl(string pathwayTitle, string gender, int age);
        string GetBusinessApiJustToBeSafePartOneUrl(string pathwayId);
        string GetBusinessApiJustToBeSafePartTwoUrl(string pathwayId, string questionId, string jtbsQuestionIds,bool hasAnswwers);
        string GetBusinessApiInterimCareAdviceUrl(string dxCode, string ageGroup, string gender);
        string GetBusinessApiListOutcomesUrl();

        string BusinessDosCheckCapacitySummaryUrl { get; }
        string BusinessDosServicesByClinicalTermUrl { get; }
        string BusinessDosServiceDetailsByIdUrl { get; }
        string FeedbackAddFeedbackUrl { get; }
        string FeedbackAuthorization { get; }
        string PostcodeSearchByIdApiUrl { get; }
        string PostcodeSubscriptionKey { get; }
        string IntegrationApiItkDispatcher { get; }
        string RedisConnectionString { get; }
        string DosUsername { get; }
        string DosPassword { get; }
        string DOSMobileBaseUrl { get; }
        string DOSMobileUsername { get; }
        string DOSMobilePassword { get; }

        string BusinessApiListOutcomesUrl { get; }
    }
}