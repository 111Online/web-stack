using System.Collections.Generic;
using Newtonsoft.Json;

namespace NHS111.Models.Models.Web.FromExternalServices
{
    public class DosServicesByClinicalTermResult
    {
        [JsonProperty(PropertyName = "success")]
        public SuccessObject Success { get; set; }

        [JsonProperty(PropertyName = "error")]
        public SuccessObject Error { get; set; }

        public class SuccessObject
        {
            [JsonProperty(PropertyName = "code")]
            public int Code { get; set; }
            [JsonProperty(PropertyName = "transactionId")]
            public string TransactionId { get; set; }
            [JsonProperty(PropertyName = "servicesReturnedAreCatchAll")]
            public string ServicesReturnedAreCatchAll { get; set; }
            [JsonProperty(PropertyName = "serviceCount")]
            public int ServiceCount { get; set; }
            [JsonProperty(PropertyName = "services")]
            public List<Service> Services { get; set; }
        }

        public class ErrorObject
        {
            [JsonProperty(PropertyName = "code")]
            public int Code { get; set; }
            [JsonProperty(PropertyName = "message")]
            public string Message { get; set; }
        }

        public class Type
        {
            [JsonProperty(PropertyName = "id")]
            public string Id { get; set; }
            [JsonProperty(PropertyName = "name")]
            public string Name { get; set; }
        }

        public class Phone
        {
            [JsonProperty(PropertyName = "@public")]
            public string Public { get; set; }
            [JsonProperty(PropertyName = "nonpublic")]
            public string NonPublic { get; set; }
            [JsonProperty(PropertyName = "fax")]
            public string Fax { get; set; }
        }

        public class Day
        {
            [JsonProperty(PropertyName = "day")]
            public string Value { get; set; }
            [JsonProperty(PropertyName = "sessions")]
            public List<object> Sessions { get; set; }
        }

        public class OpeningTimes
        {
            [JsonProperty(PropertyName = "allHours")]
            public bool AllHours { get; set; }
            [JsonProperty(PropertyName = "days")]
            public List<Day> Days { get; set; }
            [JsonProperty(PropertyName = "specifiedDates")]
            public List<object> SpecifiedDates { get; set; }
        }

        public class ReferralInstructions
        {
            [JsonProperty(PropertyName = "callHandler")]
            public string CallHandler { get; set; }
            [JsonProperty(PropertyName = "other")]
            public string Other { get; set; }
        }

        public class Status
        {
            [JsonProperty(PropertyName = "rag")]
            public string Rag { get; set; }
            [JsonProperty(PropertyName = "human")]
            public string Human { get; set; }
            [JsonProperty(PropertyName = "hex")]
            public string Hex { get; set; }
        }

        public class Capacity
        {
            [JsonProperty(PropertyName = "status")]
            public Status Status { get; set; }
        }

        public class Service
        {
            [JsonProperty(PropertyName = "id")]
            public string Id { get; set; }
            [JsonProperty(PropertyName = "name")]
            public string Name { get; set; }
            [JsonProperty(PropertyName = "type")]
            public Type Type { get; set; }
            [JsonProperty(PropertyName = "odsCode")]
            public string OdsCode { get; set; }
            [JsonProperty(PropertyName = "address")]
            public List<string> Address { get; set; }
            [JsonProperty(PropertyName = "postcode")]
            public string Postcode { get; set; }
            [JsonProperty(PropertyName = "easting")]          
            public string Easting { get; set; }
            [JsonProperty(PropertyName = "northing")]
            public string Northing { get; set; }
            [JsonProperty(PropertyName = "phone")]
            public Phone Phone { get; set; }
            [JsonProperty(PropertyName = "web")]
            public string Web { get; set; }
            [JsonProperty(PropertyName = "openingTimes")]
            public OpeningTimes OpeningTimes { get; set; }
            [JsonProperty(PropertyName = "referralInstructions")]
            public ReferralInstructions ReferralInstructions { get; set; }
            [JsonProperty(PropertyName = "capacity")]
            public Capacity Capacity { get; set; }
            [JsonProperty(PropertyName = "endpoints")]
            public List<object> Endpoints { get; set; }
            [JsonProperty(PropertyName = "patientDistance")]
            public string PatientDistance { get; set; }
        }
    }
}
