using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NHS111.Models.Models.Web
{
    public class SMSRegistrationViewModel
    {
        public string ViewName { get; set; }

        public bool VerificationCodeIncorrect { get; set; }

        public SendSmsOutcomeViewModel SendSmsOutcomeViewModel;

        public SMSRegistrationViewModel(SendSmsOutcomeViewModel sendSmsOutcomeViewModelModel)
        {
            SendSmsOutcomeViewModel = sendSmsOutcomeViewModelModel;
        }
    }
}
