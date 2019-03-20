using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Newtonsoft.Json;

namespace NHS111.Models.Models.Web.FromExternalServices
{
    public class SuccessObject<T>
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
        public List<T> Services { get; set; }

        public T RecommendedService
        {
            get { return this.Services.FirstOrDefault(); }
        }
    }
}
