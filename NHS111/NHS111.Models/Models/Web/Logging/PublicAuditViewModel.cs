using FluentValidation.Attributes;
using Newtonsoft.Json;
using NHS111.Models.Models.Web.Enums;
using NHS111.Models.Models.Web.Validators;
using System;

namespace NHS111.Models.Models.Web.Logging
{

    // This is a limited AuditViewModel for use by the public endpoint to prevent
    // anyone from being able to POST data in any column of the database.
    [Validator(typeof(PublicAuditViewModelValidator))]
    public class PublicAuditViewModel
    {
        private EventType _eventKey = EventType.None;
        private string _eventValue = string.Empty;
        private string _page = string.Empty;

        [JsonProperty(PropertyName = "journeyId")]
        public string JourneyId { get; set; }

        [JsonProperty(PropertyName = "eventKey")]
        public EventType EventKey
        {
            get { return _eventKey; }
            set { _eventKey = value; }
        }

        [JsonProperty(PropertyName = "eventValue")]
        public string EventValue
        {
            get { return _eventValue; }
            set { _eventValue = value; }
        }

        [JsonProperty(PropertyName = "page")]
        public string Page
        {
            get { return _page; }
            set { _page = value; }
        }
    }
}
