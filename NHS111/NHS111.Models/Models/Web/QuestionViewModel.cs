using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FluentValidation.Attributes;
using Newtonsoft.Json;
using NHS111.Models.Models.Domain;
using NHS111.Models.Models.Web.FromExternalServices;
using NHS111.Models.Models.Web.Validators;

namespace NHS111.Models.Models.Web
{
    [Validator(typeof(QuestionViewModelValidator))]
    public class QuestionViewModel : JourneyViewModel
    {
        public string AnswerInputValue { get; set; }
        public string SelectedAnswer { get; set; }
        public JourneyStep ToStep()
        {
            var answer = JsonConvert.DeserializeObject<Answer>(SelectedAnswer);
            return new JourneyStep
            {
                QuestionNo = QuestionNo,
                QuestionTitle = Title,
                Answer = answer,
                QuestionId = Id,
                State = StateJson,
                NodeType = NodeType
            };
        }
    }
}
