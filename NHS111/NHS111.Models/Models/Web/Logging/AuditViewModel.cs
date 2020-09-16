
using NHS111.Models.Models.Web.Enums;

namespace NHS111.Models.Models.Web.Logging
{
    using Newtonsoft.Json;
    using System;

    public class AuditViewModel
    {
        private string _pathwayId = string.Empty;
        private string _journeyId = string.Empty;
        private string _pathwayTitle = string.Empty;
        private string _answerTitle = string.Empty;
        private string _answerOrder = string.Empty;
        private string _questionTitle = string.Empty;
        private string _questionNo = string.Empty;
        private string _questionId = string.Empty;
        private string _dxCode = string.Empty;
        private string _eventData = string.Empty;
        private EventType _eventKey = EventType.None;
        private string _eventValue = string.Empty;
        private string _dosRequest = string.Empty;
        private string _dosResponse = string.Empty;
        private string _page = string.Empty;
        private string _campaign = string.Empty;

        [JsonProperty(PropertyName = "sessionId")]
        public Guid SessionId { get; set; }

        [JsonProperty(PropertyName = "journeyId")]
        public string JourneyId
        {
            get { return _journeyId; }
            set { _journeyId = value; }
        }

        [JsonProperty(PropertyName = "pathwayId")]
        public string PathwayId
        {
            get { return _pathwayId; }
            set { _pathwayId = value; }
        }

        [JsonProperty(PropertyName = "pathwayTitle")]
        public string PathwayTitle
        {
            get { return _pathwayTitle; }
            set { _pathwayTitle = value; }
        }

        [JsonProperty(PropertyName = "answerTitle")]
        public string AnswerTitle
        {
            get { return _answerTitle; }
            set { _answerTitle = value; }
        }

        [JsonProperty(PropertyName = "answerOrder")]
        public string AnswerOrder
        {
            get { return _answerOrder; }
            set { _answerOrder = value; }
        }

        [JsonProperty(PropertyName = "questionTitle")]
        public string QuestionTitle
        {
            get { return _questionTitle; }
            set { _questionTitle = value; }
        }

        [JsonProperty(PropertyName = "questionNo")]
        public string QuestionNo
        {
            get { return _questionNo; }
            set { _questionNo = value; }
        }

        [JsonProperty(PropertyName = "questionId")]
        public string QuestionId
        {
            get { return _questionId; }
            set { _questionId = value; }
        }

        [JsonProperty(PropertyName = "dxCode")]
        public string DxCode
        {
            get { return _dxCode; }
            set { _dxCode = value; }
        }

        [JsonProperty(PropertyName = "eventData")]
        public string EventData
        {
            get { return _eventData; }
            set { _eventData = value; }
        }

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

        [JsonProperty(PropertyName = "dosRequest")]
        public string DosRequest
        {
            get { return _dosRequest; }
            set { _dosRequest = value; }
        }

        [JsonProperty(PropertyName = "dosResponse")]
        public string DosResponse
        {
            get { return _dosResponse; }
            set { _dosResponse = value; }
        }

        [JsonProperty(PropertyName = "page")]
        public string Page
        {
            get { return _page; }
            set { _page = value; }
        }

        [JsonProperty(PropertyName = "campaign")]
        public string Campaign
        {
            get { return _campaign; }
            set { _campaign = value; }
        }
    }
}