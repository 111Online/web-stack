using FluentValidation.Attributes;
using Newtonsoft.Json;
using NHS111.Models.Models.Domain;
using NHS111.Models.Models.Web.FromExternalServices;
using NHS111.Models.Models.Web.Validators;
using System;

namespace NHS111.Models.Models.Web
{
    [Validator(typeof(QuestionViewModelValidator))]
    public class QuestionViewModel : JourneyViewModel
    {
        private string _answerInputValue;
        public string AnswerInputValue
        {
            get
            {
                if (_answerInputValue == null) return _answerInputValue;

                // This ensures phone numbers with spaces are allowed to be entered but the spaces are stripped
                if (QuestionType == QuestionType.Telephone) return _answerInputValue.Replace(" ", "");
                return _answerInputValue;
            }
            set { _answerInputValue = value; }
        }

        public string SelectedAnswer { get; set; }

        public void ResetAnswerInputValue()
        {
            this.AnswerInputValue = String.Empty;
        }
        public DateTimeViewModel DateAnswer { get; set; }
        public JourneyStep ToStep()
        {
            var answer = JsonConvert.DeserializeObject<Answer>(SelectedAnswer);
            return new JourneyStep
            {
                QuestionType = QuestionType,
                QuestionNo = QuestionNo,
                QuestionTitle = Title,
                Answer = answer,
                QuestionId = Id,
                State = StateJson,
                NodeType = NodeType,
                AnswerInputValue = (this.QuestionType != QuestionType.Date) ? this.AnswerInputValue : this.DateAnswer.Date.Value.ToString("s")
            };
        }
    }
}
